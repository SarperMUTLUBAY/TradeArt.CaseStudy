using TradeArt.CaseStudy.Business.Implementations;
using TradeArt.CaseStudy.Business.Interfaces;
using TradeArt.CaseStudy.Model.Requests.CaseStudy;

namespace TradeArt.CaseStudy.Business.Tests; 

[TestFixture]
public class CaseStudyBusinessTests {
	private ICaseStudyBusiness _caseStudyBusiness;

	[SetUp]
	public void Setup() {
		_caseStudyBusiness = new CaseStudyBusiness();
	}
	
	[Test]
	public void InvertText_Returns_Success() {
		//Arrange
		var text = "This is a test text.";
		var invertedText = ".txet tset a si sihT";
		var request = new InvertRequest {
			Text = text
		};

		//Act
		var result = _caseStudyBusiness.InvertText(request);

		//Assert
		Assert.True(result.IsSuccess);
		Assert.Null(result.Message);
		Assert.That(invertedText, Is.EqualTo((string)result.Data));
	}
	
	[Test]
	public void InvertText_Returns_TextIsNotNull() {
		//Arrange
		var request = new InvertRequest();

		//Act
		var result = _caseStudyBusiness.InvertText(request);

		//Assert
		Assert.False(result.IsSuccess);
		Assert.That(result.Message, Is.EqualTo("The Text field is required."));
	}
}