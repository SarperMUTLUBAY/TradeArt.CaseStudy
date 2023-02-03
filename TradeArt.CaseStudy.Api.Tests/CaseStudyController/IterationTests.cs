using Moq;
using TradeArt.CaseStudy.Facade.Interfaces;
using TradeArt.CaseStudy.Model;
using TradeArt.CaseStudy.Model.Requests.CaseStudy;

namespace TradeArt.CaseStudy.Api.Tests.CaseStudyController;

[TestFixture]
public class IterationTests {
	private Mock<ICaseStudyFacade> _mockCaseStudyFacade;
	[SetUp]
	public void Setup() {
		_mockCaseStudyFacade = new Mock<ICaseStudyFacade>();
	}
	
	[Test]
	public void Iteration_Returns_Success() {
		//Arrange
		var request = new IterationRequest {IterationCount = 1000};
		var response = new SuccessResult<bool>(true);
		
		_mockCaseStudyFacade.Setup(x => x.Iteration(request)).Returns(response);
		var controller = new Controllers.CaseStudyController(_mockCaseStudyFacade.Object);
		
		//Act
		var result = controller.Iteration(request);

		//Assert
		Assert.True(result.IsSuccess);
		Assert.Null(result.Message);
		Assert.That((bool)result.Data, Is.EqualTo((bool)response.Data));
	}
}