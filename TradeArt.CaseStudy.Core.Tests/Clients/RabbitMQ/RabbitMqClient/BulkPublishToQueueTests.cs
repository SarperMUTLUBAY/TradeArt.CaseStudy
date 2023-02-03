using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using TradeArt.CaseStudy.Common;
using TradeArt.CaseStudy.Core.Configs;

namespace TradeArt.CaseStudy.Core.Tests.Clients.RabbitMQ.RabbitMqClient;

[TestFixture]
public class BulkPublishToQueueTests {
	private Mock<ILogger<Core.Clients.RabbitMQ.RabbitMqClient>> _mockRabbitMqClientLogger;
	private Mock<IOptions<RabbitMQConnectionConfiguration>> _mockRabbitMqConnectionConfigurationOption;

	[SetUp]
	public void Setup() {
		_mockRabbitMqClientLogger = new Mock<ILogger<Core.Clients.RabbitMQ.RabbitMqClient>>();
		_mockRabbitMqConnectionConfigurationOption = new Mock<IOptions<RabbitMQConnectionConfiguration>>();
	}

	[Test]
	public void BulkPublishToQueue_WhenQueueNameIsNull_ThrowsArgumentNullException() {
		//Arrange
		string queueName = null;
		var paramName = "queue";
		var exceptionMessage = "Queue cannot be null. (Parameter 'queue')";
		var data = new List<object>(1);
		var client = new Core.Clients.RabbitMQ.RabbitMqClient(_mockRabbitMqClientLogger.Object, _mockRabbitMqConnectionConfigurationOption.Object);

		//Act
		var exception = Assert.Throws<ArgumentNullException>(() => { client.BulkPublishToQueue(queueName, data); });

		//Assert
		Assert.NotNull(exception);
		Assert.That(paramName, Is.EqualTo(exception.ParamName));
		Assert.That(exceptionMessage, Is.EqualTo(exception.Message));
	}

	[Test]
	public void BulkPublishToQueue_WhenMessageIsNull_ThrowsArgumentNullException() {
		//Arrange
		string queueName = "test-queue";
		var paramName = "messages";
		var exceptionMessage = "Messages cannot be null or empty collection. (Parameter 'messages')";
		List<object> data = null;
		var client = new Core.Clients.RabbitMQ.RabbitMqClient(_mockRabbitMqClientLogger.Object, _mockRabbitMqConnectionConfigurationOption.Object);

		//Act
		var exception = Assert.Throws<ArgumentNullException>(() => { client.BulkPublishToQueue(queueName, data); });

		//Assert
		Assert.NotNull(exception);
		Assert.That(paramName, Is.EqualTo(exception.ParamName));
		Assert.That(exceptionMessage, Is.EqualTo(exception.Message));
	}

	[Test]
	public void BulkPublishToQueue_WhenRabbitHostIsNullOrWhitespace_ThrowsCaseStudyException() {
		//Arrange
		string queueName = "test-queue";
		var exceptionMessage = "RabbitMQ connection hostname cannot be null whitespace.";
		var data = new List<string>(1) { "test-message"};
		var options = new RabbitMQConnectionConfiguration {HostName = null};
		var mockOptions = new Mock<IOptions<RabbitMQConnectionConfiguration>>();
		mockOptions.Setup(x => x.Value).Returns(options);
		var client = new Core.Clients.RabbitMQ.RabbitMqClient(_mockRabbitMqClientLogger.Object, mockOptions.Object);

		//Act
		var exception = Assert.Throws<CaseStudyException>(() => { client.BulkPublishToQueue(queueName, data); });

		//Assert
		Assert.NotNull(exception);
		Assert.That(exceptionMessage, Is.EqualTo(exception.Message));
	}
}