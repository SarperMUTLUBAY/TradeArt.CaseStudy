using Microsoft.Extensions.Options;
using Moq;
using TradeArt.CaseStudy.Api.Controllers;
using TradeArt.CaseStudy.Core.Clients.RabbitMQ;
using TradeArt.CaseStudy.Core.Configs;
using TradeArt.CaseStudy.Facade.Implementations;
using TradeArt.CaseStudy.Model.Requests.CaseStudy;

namespace TradeArt.CaseStudy.Api.Tests; 

[TestFixture]
public class CaseStudyControllerTests {
	private CaseStudyController _caseStudyController;

	[SetUp]
	public void Setup() {
		RabbitMQQueueConfiguration mockRabbitMqQueueConfiguration = new RabbitMQQueueConfiguration{IteratorQueue= "test"};
		var mockRabbitMqQueueConfigurationOption = new Mock<IOptions<RabbitMQQueueConfiguration>>();
		mockRabbitMqQueueConfigurationOption.Setup(ap => ap.Value).Returns(mockRabbitMqQueueConfiguration);
		
		var mockRabbitClient = new Mock<IRabbitMqClient>();
		mockRabbitClient.Setup(func => func.PublishToQueue<string>(default, null)).Returns(true);
		mockRabbitClient.Setup(func => func.BulkPublishToQueue<string>(default, null)).Returns(true);
		_caseStudyController = new CaseStudyController(new CaseStudyFacade(mockRabbitClient.Object, mockRabbitMqQueueConfigurationOption.Object));
	}
	
	[Test]
	public void Invert_Returns_Success() {
		//Arrange
		var text = "This is a test text.";
		var invertedText = ".txet tset a si sihT";
		var request = new InvertRequest {
			Text = text
		};
		
		//Act
		var result = _caseStudyController.Invert(request);

		//Assert
		Assert.True(result.IsSuccess);
		Assert.Null(result.Message);
		Assert.That(invertedText, Is.EqualTo((string)result.Data));
	}
}