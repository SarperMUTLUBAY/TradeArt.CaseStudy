using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using TradeArt.CaseStudy.Core.Clients.RabbitMQ;
using TradeArt.CaseStudy.Core.Configs;
using TradeArt.CaseStudy.Facade.Interfaces;
using TradeArt.CaseStudy.Model;
using TradeArt.CaseStudy.Model.Dtos;
using TradeArt.CaseStudy.Model.Requests.CaseStudy;
using System;

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
			using var md5 = MD5.Create();
			await using var stream = new FileStream(request.FileUrl, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);
			var buffer = new byte[4096];
			int bytesRead;
			do {
				bytesRead = await stream.ReadAsync(buffer.AsMemory(0, 4096), cancellationToken);
				if (bytesRead > 0) {
					md5.TransformBlock(buffer, 0, bytesRead, null, 0);
				}
			} while (bytesRead > 0);

			md5.TransformFinalBlock(buffer, 0, 0);
			var sha = BitConverter.ToString(md5.Hash)
								  .Replace("-", "")
								  .ToLowerInvariant();

			return new SuccessResult<string>(sha);
		} catch (Exception e) {
			return new ErrorResult(e.Message);
		}
	}
}