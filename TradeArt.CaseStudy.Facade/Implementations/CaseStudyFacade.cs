using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using TradeArt.CaseStudy.Core.Clients.RabbitMQ;
using TradeArt.CaseStudy.Core.Configs;
using TradeArt.CaseStudy.Core.Helpers;
using TradeArt.CaseStudy.Facade.Interfaces;
using TradeArt.CaseStudy.Integration.GraphQL.BlockTap;
using TradeArt.CaseStudy.Model;
using TradeArt.CaseStudy.Model.Dtos;
using TradeArt.CaseStudy.Model.Requests.CaseStudy;
using TradeArt.CaseStudy.Model.Responses.CaseStudy;

namespace TradeArt.CaseStudy.Facade.Implementations;

public class CaseStudyFacade : ICaseStudyFacade {
	private readonly IRabbitMqClient _rabbitMqClient;
	private readonly RabbitMQQueueConfiguration _rabbitMqQueueConfiguration;
	private readonly IBlockTapGraphQLIntegration _blockTapGraphQlIntegration;

	public CaseStudyFacade(IRabbitMqClient rabbitMqClient, IOptions<RabbitMQQueueConfiguration> rabbitMqQueueConfigurationOptions, IBlockTapGraphQLIntegration blockTapGraphQlIntegration) {
		_rabbitMqClient = rabbitMqClient;
		_rabbitMqQueueConfiguration = rabbitMqQueueConfigurationOptions.Value;
		_blockTapGraphQlIntegration = blockTapGraphQlIntegration;
	}

	/// <summary>
	/// Invert text
	/// </summary>
	/// <param name="request">InvertRequest model for inverting text</param>
	/// <returns>returns SuccessResult on success, ErrorResult on failure</returns>
	public BaseResult InvertText(InvertRequest request) {
		if (string.IsNullOrWhiteSpace(request.Text))
			return new ErrorResult("The Text field is required.");

		var charArray = request.Text.ToCharArray();
		Array.Reverse(charArray);
		return new SuccessResult<string>(new string(charArray));
	}

	/// <summary>
	/// makes Iteration and send data to queue as many as the entered number
	/// </summary>
	/// <param name="request">It contains IterationCount for decide to how many iteration</param>
	/// <returns>returns SuccessResult on success, ErrorResult on failure</returns>
	public BaseResult Iteration(IterationRequest request) {
		try {
			if (request.IterationCount <= 0)
				return new ErrorResult("Iteration count must be greater than 0.");

			var iterationDate = DateTime.UtcNow;
			var data = Enumerable.Range(1, request.IterationCount)
								 .Select(x => new IteratorMessageDto {
										     IterationId = x,
										     IterationDate = iterationDate
									     })
								 .ToList();

			_rabbitMqClient.BulkPublishToQueue(_rabbitMqQueueConfiguration.IteratorQueue, data);
			return new SuccessResult<bool>(true);
		} catch (Exception e) {
			return new ErrorResult(e.Message);
		}
	}

	/// <summary>
	/// Calculate a SHA hash (in hex form)
	/// </summary>
	/// <param name="request">It contains FileUrl for file to be download</param>
	/// <param name="cancellationToken">cancellation token for async process</param>
	/// <returns>returns SuccessResult with file sha value on success, ErrorResult on failure</returns>
	public async Task<BaseResult> CalculateSHA(CalculateShaRequest request, CancellationToken cancellationToken) {
		try {
			string fileName = FileHelper.GetFileNameFromUrl(request.FileUrl) ??
							  Guid.NewGuid()
								  .ToString("D");

			var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), "Files");
			var fileResult = await FileHelper.DownloadFileAsync(request.FileUrl, pathToSave, fileName, true, cancellationToken);
			if (string.IsNullOrWhiteSpace(fileResult))
				return new ErrorResult("File not found.");

			using var md5 = MD5.Create();
			await using var stream = File.OpenRead(fileResult);
			var hash = await md5.ComputeHashAsync(stream, cancellationToken);
			var sha = BitConverter.ToString(hash)
								  .Replace("-", string.Empty)
								  .ToLowerInvariant();

			return new SuccessResult<string>(sha);
		} catch (Exception e) {
			return new ErrorResult(e.Message);
		}
	}

	/// <summary>
	/// Get market assets and prices
	/// </summary>
	/// <param name="cancellationToken">cancellation token for async process</param>
	/// <returns>returns SuccessResult with Assets on success, ErrorResult on failure</returns>
	public async Task<BaseResult> GetAssets(CancellationToken cancellationToken) {
		var assetResult = await _blockTapGraphQlIntegration.GetAssetsAsync(cancellationToken);
		if (assetResult == null)
			return new ErrorResult("No assets returned");

		var response = new GetAssetsResponse();


		for (var i = 0; i <= 100; i += 20) {
			var assetSymbols = assetResult.Skip(i)
										  .Take(20)
										  .Select(x => x.AssetSymbol)
										  .ToList();

			var priceData = await _blockTapGraphQlIntegration.GetPricesAsync(assetSymbols, cancellationToken);
			if (priceData == null)
				continue;

			var groupedPrices = priceData.Where(x => x.Ticker != null)
										 .GroupBy(x => x.BaseSymbol);

			foreach (var prices in groupedPrices) {
				var assetItem = assetResult.FirstOrDefault(x => string.Equals(x.AssetSymbol, prices.Key, StringComparison.InvariantCultureIgnoreCase));
				if (assetItem == null)
					continue;

				response.Assets.Add(new Asset {
									    AssetSymbol = assetItem.AssetSymbol,
									    AssetName = assetItem.AssetName,
									    MarketCap = assetItem.MarketCap,
									    Prices = prices.Select(x => new AssetPrice {
															       Price = x.Ticker.LastPrice,
															       MarketSymbol = x.MarketSymbol
														       })
								    });
			}
		}

		return new SuccessResult<GetAssetsResponse>(response);
	}
}