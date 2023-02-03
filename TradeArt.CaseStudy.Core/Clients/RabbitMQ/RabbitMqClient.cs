using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using TradeArt.CaseStudy.Common;
using TradeArt.CaseStudy.Core.Configs;

namespace TradeArt.CaseStudy.Core.Clients.RabbitMQ;

public class RabbitMqClient : IRabbitMqClient {
	private readonly ILogger<RabbitMqClient> _logger;
	private readonly RabbitMQConnectionConfiguration _rabbitMqConnectionConfigurations;

	public RabbitMqClient(ILogger<RabbitMqClient> logger, IOptions<RabbitMQConnectionConfiguration> rabbitMqConnectionConfigurationOptions) {
		_logger = logger;
		_rabbitMqConnectionConfigurations = rabbitMqConnectionConfigurationOptions.Value;
	}

	/// <summary>
	/// Send message to queue
	/// </summary>
	/// <param name="queue">Queue name for send to message</param>
	/// <param name="message">Message data</param>
	/// <typeparam name="T">Generic message type parameter.</typeparam>
	/// <returns>Returns true when the operation succeeds, false if it fails</returns>
	/// <exception cref="ArgumentNullException">Returns when queue name or message is null</exception>
	/// <exception cref="CaseStudyException">Returns when rabbitmq host name is null</exception>
	public bool PublishToQueue<T>(string queue, T message) {
		if (queue == null) {
			_logger.LogInformation("Queue cannot be null.");
			throw new ArgumentNullException(nameof(queue), "Queue cannot be null.");
		}

		if (message == null) {
			_logger.LogInformation("Message cannot be null.");
			throw new ArgumentNullException(nameof(message), "Message cannot be null.");
		}

		if (string.IsNullOrWhiteSpace(_rabbitMqConnectionConfigurations.HostName)) {
			_logger.LogInformation("RabbitMQ connection hostname cannot be null whitespace.");
			throw new CaseStudyException("RabbitMQ connection hostname cannot be null whitespace.");
		}

		try {
			var factory = new ConnectionFactory {HostName = _rabbitMqConnectionConfigurations.HostName};

			using var connection = factory.CreateConnection();
			using var channel = connection.CreateModel();
			channel.QueueDeclare(queue, false, false, false, null);

			var data = JsonSerializer.Serialize(message);
			var body = Encoding.UTF8.GetBytes(data);

			channel.BasicPublish("", queue, null, body);
			return true;
		} catch (Exception e) {
			_logger.LogError(e.Message);
			return false;
		}
	}

	/// <summary>
	/// Send bulk message to queue
	/// </summary>
	/// <param name="queue">Queue name for send to messages</param>
	/// <param name="messages">Messages data list</param>
	/// <typeparam name="T">Generic message type parameter.</typeparam>
	/// <returns>Returns true when the operation succeeds, false if it fails</returns>
	/// <exception cref="ArgumentNullException">Returns when queue name null or messages is null or empty</exception>
	/// <exception cref="CaseStudyException">Returns when rabbitmq host name is null</exception>
	public bool BulkPublishToQueue<T>(string queue, List<T> messages) {
		if (queue == null) {
			_logger.LogInformation("Queue cannot be null.");
			throw new ArgumentNullException(nameof(queue), "Queue cannot be null.");
		}

		if (messages == null || messages.Count == 0) {
			_logger.LogInformation("Messages cannot be null or empty collection.");
			throw new ArgumentNullException(nameof(messages), "Messages cannot be null or empty collection.");
		}

		if (string.IsNullOrWhiteSpace(_rabbitMqConnectionConfigurations.HostName)) {
			_logger.LogInformation("RabbitMQ connection hostname cannot be null whitespace.");
			throw new CaseStudyException("RabbitMQ connection hostname cannot be null whitespace.");
		}

		try {
			var factory = new ConnectionFactory {HostName = _rabbitMqConnectionConfigurations.HostName};

			using var connection = factory.CreateConnection();
			using var channel = connection.CreateModel();
			channel.QueueDeclare(queue, false, false, false, null);

			var basicPublishBatch = channel.CreateBasicPublishBatch();

			foreach (var message in messages) {
				var data = JsonSerializer.Serialize(message);
				var body = Encoding.UTF8.GetBytes(data);
				var memory = new ReadOnlyMemory<byte>(body);
				basicPublishBatch.Add("", queue, false, null, memory);
			}

			basicPublishBatch.Publish();

			return true;
		} catch (Exception e) {
			_logger.LogError(e.Message);
			return false;
		}
	}
}