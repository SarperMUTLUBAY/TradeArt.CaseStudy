using Moq;
using TradeArt.CaseStudy.Facade.Interfaces;
using TradeArt.CaseStudy.Model;
using TradeArt.CaseStudy.Model.Requests.CaseStudy;

namespace TradeArt.CaseStudy.Api.Tests.CaseStudyController;

[TestFixture]
public class InvertTests {
	private Mock<ICaseStudyFacade> _mockCaseStudyFacade;
	
	[SetUp]
	public void Setup() {
		_mockCaseStudyFacade = new Mock<ICaseStudyFacade>();
	}

	[Test]
	public void Invert_Returns_Success() {
		//Arrange
		var text = "This is a test text.";
		var invertedText = ".txet tset a si sihT";
		var request = new InvertRequest {Text = text};
		
		_mockCaseStudyFacade.Setup(x => x.InvertText(request)).Returns(new SuccessResult<string>(invertedText));
		var controller = new Controllers.CaseStudyController(_mockCaseStudyFacade.Object);
		
		//Act
		var result = controller.Invert(request);

		//Assert
		Assert.True(result.IsSuccess);
		Assert.Null(result.Message);
		Assert.That(invertedText, Is.EqualTo((string)result.Data));
	}
}