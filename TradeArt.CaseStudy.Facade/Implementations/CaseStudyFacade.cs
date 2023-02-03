using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using TradeArt.CaseStudy.Core.Clients.RabbitMQ;
using TradeArt.CaseStudy.Core.Configs;
using TradeArt.CaseStudy.Facade.Interfaces;
using TradeArt.CaseStudy.Model;
using TradeArt.CaseStudy.Model.Dtos;
using TradeArt.CaseStudy.Model.Requests.CaseStudy;
using System;
using TradeArt.CaseStudy.Core.Helpers;

namespace TradeArt.CaseStudy.Facade.Implementations;

public class CaseStudyFacade : ICaseStudyFacade {
	private readonly IRabbitMqClient _rabbitMqClient;
	private readonly RabbitMQQueueConfiguration _rabbitMqQueueConfiguration;

	public CaseStudyFacade(IRabbitMqClient rabbitMqClient, IOptions<RabbitMQQueueConfiguration> rabbitMqQueueConfigurationOptions) {
		_rabbitMqClient = rabbitMqClient;
		_rabbitMqQueueConfiguration = rabbitMqQueueConfigurationOptions.Value;
	}

	public BaseResult InvertText(InvertRequest request) {
		if (string.IsNullOrWhiteSpace(request.Text))
			return new ErrorResult("The Text field is required.");

		var charArray = request.Text.ToCharArray();
		Array.Reverse(charArray);
		return new SuccessResult<string>(new string(charArray));
	}

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

	public async Task<BaseResult> CalculateSHA(CalculateShaRequest request, CancellationToken cancellationToken) {
		try {
			string fileName = FileHelper.GetFileNameFromUrl(request.FileUrl) ?? Guid.NewGuid().ToString("D");
			var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), "Files");
			var fileResult = await FileHelper.DownloadFileAsync(request.FileUrl, pathToSave, fileName, true, cancellationToken);
			if (string.IsNullOrWhiteSpace(fileResult)) {
				return new ErrorResult("File not found.");
			}

			using var md5 = MD5.Create();
			await using var stream = File.OpenRead(fileResult);
			var hash = await md5.ComputeHashAsync(stream, cancellationToken);
			var sha = BitConverter.ToString(hash).Replace("-", string.Empty).ToLowerInvariant();

			return new SuccessResult<string>(sha);
		} catch (Exception e) {
			return new ErrorResult(e.Message);
		}
	}
}