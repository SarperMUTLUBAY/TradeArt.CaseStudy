using Microsoft.Extensions.Options;
using Moq;
using TradeArt.CaseStudy.Core.Clients.RabbitMQ;
using TradeArt.CaseStudy.Core.Configs;
using TradeArt.CaseStudy.Integration.GraphQL.BlockTap;
using TradeArt.CaseStudy.Model.Requests.CaseStudy;

namespace TradeArt.CaseStudy.Facade.Tests.CaseStudyFacade;

[TestFixture]
public class IterationTests {
	private Mock<IRabbitMqClient> _mockRabbitClient;
	private Mock<IOptions<RabbitMQQueueConfiguration>> _mockRabbitMqQueueConfigurationOption;
	private Mock<IBlockTapGraphQLIntegration> _mockBlockTapGraphQlIntegration;

	[SetUp]
	public void Setup() {
		_mockRabbitClient = new Mock<IRabbitMqClient>();
		var rabbitMqQueueConfiguration = new RabbitMQQueueConfiguration {IteratorQueue = "test-queue"};

		_mockRabbitMqQueueConfigurationOption = new Mock<IOptions<RabbitMQQueueConfiguration>>();
		_mockRabbitMqQueueConfigurationOption.SetupGet(x => x.Value)
											 .Returns(rabbitMqQueueConfiguration);

		_mockBlockTapGraphQlIntegration = new Mock<IBlockTapGraphQLIntegration>();
	}

	[Test]
	public void Iteration_Returns_Success() {
		//Arrange
		var request = new IterationRequest {IterationCount = 1000};
		_mockRabbitClient.Setup(x => x.BulkPublishToQueue(It.IsAny<string>(), It.IsAny<List<It.IsAnyType>>()))
						 .Returns(true);

		var facade = new Implementations.CaseStudyFacade(_mockRabbitClient.Object, _mockRabbitMqQueueConfigurationOption.Object, _mockBlockTapGraphQlIntegration.Object);

		//Act
		var result = facade.Iteration(request);

		//Assert
		Assert.True(result.IsSuccess);
		Assert.Null(result.Message);
	}

	[Test]
	public void Iteration_Returns_IterationCountMustBeGreaterThanZeroError() {
		//Arrange
		var errorMessage = "Iteration count must be greater than 0.";
		var request = new IterationRequest {IterationCount = 0};
		var facade = new Implementations.CaseStudyFacade(_mockRabbitClient.Object, _mockRabbitMqQueueConfigurationOption.Object, _mockBlockTapGraphQlIntegration.Object);

		//Act
		var result = facade.Iteration(request);

		//Assert
		Assert.False(result.IsSuccess);
		Assert.That(errorMessage, Is.EqualTo(result.Message));
	}

	[Test]
	public void Iteration_Returns_Error() {
		//Arrange
		var exceptionMessage = "An error occured.";
		var request = new IterationRequest {IterationCount = 1000};
		_mockRabbitClient.Setup(x => x.BulkPublishToQueue(It.IsAny<string>(), It.IsAny<List<It.IsAnyType>>()))
						 .Throws(new Exception(exceptionMessage));
		var facade = new Implementations.CaseStudyFacade(_mockRabbitClient.Object, _mockRabbitMqQueueConfigurationOption.Object, _mockBlockTapGraphQlIntegration.Object);

		//Act
		var result = facade.Iteration(request);

		//Assert
		Assert.False(result.IsSuccess);
		Assert.That(exceptionMessage, Is.EqualTo(result.Message));
	}
}