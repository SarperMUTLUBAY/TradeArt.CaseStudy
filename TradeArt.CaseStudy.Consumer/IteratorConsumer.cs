using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using TradeArt.CaseStudy.Consumer.Configs;
using TradeArt.CaseStudy.Consumer.Models;

namespace TradeArt.CaseStudy.Consumer;

public class IteratorConsumer {
	private readonly ILogger<IteratorConsumer> _logger;
	private readonly RabbitMQConfigurations _rabbitMqConfigurations;

	public IteratorConsumer(ILogger<IteratorConsumer> logger, IOptions<RabbitMQConfigurations> rabbitMqConfigurationOptions) {
		_logger = logger;
		_rabbitMqConfigurations = rabbitMqConfigurationOptions.Value;
	}

	public void Consume() {
		var rabbitHostName = _rabbitMqConfigurations.Connection.HostName;
		if (string.IsNullOrWhiteSpace(rabbitHostName))
			throw new Exception($"RabbitMQ connection hostname cannot be null whitespace.");

		var factory = new ConnectionFactory {HostName = rabbitHostName};

		using var connection = factory.CreateConnection();
		using var channel = connection.CreateModel();
		channel.QueueDeclare(_rabbitMqConfigurations.Queues.IteratorQueue, false, false, false, null);

		RunWorker(channel);
	}

	private void RunWorker(IModel channel) {
		var consumer = new EventingBasicConsumer(channel);

		consumer.Received += async (_, ea) => {
								 try {
									 var body = ea.Body.ToArray();
									 var message = Encoding.UTF8.GetString(body);
									 _logger.LogInformation($"Received {message}");

									 var data = JsonSerializer.Deserialize<IteratorMessageDto>(message);
									 var result = await ProcessData(data);
									 if (result)
										 channel.BasicAck(ea.DeliveryTag, false);
									 else
										 channel.BasicNack(ea.DeliveryTag, false, true);
								 } catch (Exception e) {
									 channel.BasicNack(ea.DeliveryTag, false, true);
									 _logger.LogError(e.Message);
								 }
							 };

		channel.BasicConsume(_rabbitMqConfigurations.Queues.IteratorQueue, false, consumer);

		Console.WriteLine("Press [enter] to exit.");
		Console.ReadLine();
	}

	private async Task<bool> ProcessData(IteratorMessageDto data) {
		_logger.LogInformation($"Data processing started for iteration Date : {data.IterationDate} and Iteration Id : {data.IterationId}");

		// Do something with data
		await Task.Delay(TimeSpan.FromMilliseconds(100));
		_logger.LogInformation($"Data processing finished for iteration Date : {data.IterationDate} and Iteration Id : {data.IterationId}");

		return true;
	}
}