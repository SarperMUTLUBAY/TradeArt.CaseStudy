using TradeArt.CaseStudy.Api.Controllers;
using TradeArt.CaseStudy.Facade.Implementations;
using TradeArt.CaseStudy.Model.Requests.CaseStudy;

namespace TradeArt.CaseStudy.Api.Tests; 

[TestFixture]
public class CaseStudyControllerTests {
	private CaseStudyController _caseStudyController;

	[SetUp]
	public void Setup() {
		_caseStudyController = new CaseStudyController(new CaseStudyFacade());
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