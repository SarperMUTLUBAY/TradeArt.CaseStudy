using Microsoft.Extensions.Options;
using Moq;
using TradeArt.CaseStudy.Core.Clients.RabbitMQ;
using TradeArt.CaseStudy.Core.Configs;
using TradeArt.CaseStudy.Integration.GraphQL.BlockTap;
using TradeArt.CaseStudy.Model.Requests.CaseStudy;

namespace TradeArt.CaseStudy.Facade.Tests.CaseStudyFacade;

[TestFixture]
public class InvertTests {
	private Mock<IRabbitMqClient> _mockRabbitClient;
	private Mock<IOptions<RabbitMQQueueConfiguration>> _mockRabbitMqQueueConfigurationOption;
	private Mock<IBlockTapGraphQLIntegration> _mockBlockTapGraphQlIntegration;
	
	[SetUp]
	public void Setup() {
		_mockRabbitClient = new Mock<IRabbitMqClient>();
		_mockRabbitMqQueueConfigurationOption = new Mock<IOptions<RabbitMQQueueConfiguration>>();
		_mockBlockTapGraphQlIntegration = new Mock<IBlockTapGraphQLIntegration>();
	}

	[Test]
	public void InvertText_Returns_Success() {
		//Arrange
		var text = "This is a test text.";
		var invertedText = ".txet tset a si sihT";
		var request = new InvertRequest {Text = text};
		var facade = new Implementations.CaseStudyFacade(_mockRabbitClient.Object, _mockRabbitMqQueueConfigurationOption.Object, _mockBlockTapGraphQlIntegration.Object);

		//Act
		var result = facade.InvertText(request);

		//Assert
		Assert.True(result.IsSuccess);
		Assert.Null(result.Message);
		Assert.That(invertedText, Is.EqualTo((string)result.Data));
	}

	[Test]
	public void InvertText_Returns_TextIsNotNull() {
		//Arrange
		var request = new InvertRequest();
		var facade = new Implementations.CaseStudyFacade(_mockRabbitClient.Object, _mockRabbitMqQueueConfigurationOption.Object, _mockBlockTapGraphQlIntegration.Object);

		//Act
		var result = facade.InvertText(request);

		//Assert
		Assert.False(result.IsSuccess);
		Assert.That(result.Message, Is.EqualTo("The Text field is required."));
	}
}