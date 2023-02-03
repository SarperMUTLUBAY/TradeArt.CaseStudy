using Moq;
using TradeArt.CaseStudy.Facade.Interfaces;
using TradeArt.CaseStudy.Model;
using TradeArt.CaseStudy.Model.Requests.CaseStudy;

namespace TradeArt.CaseStudy.Api.Tests.CaseStudyController;

[TestFixture]
public class CalculateSHATests {
	private Mock<ICaseStudyFacade> _mockCaseStudyFacade;

	[SetUp]
	public void Setup() {
		_mockCaseStudyFacade = new Mock<ICaseStudyFacade>();
	}

	[Test]
	public async Task CalculateSHA_Returns_Success() {
		//Arrange
		var request = new CalculateShaRequest {FileUrl = "file-url"};
		var response = new SuccessResult<string>("calculated-sha");

		_mockCaseStudyFacade.Setup(x => x.CalculateSHA(request, CancellationToken.None))
							.ReturnsAsync(response);

		var controller = new Controllers.CaseStudyController(_mockCaseStudyFacade.Object);

		//Act
		var result = await controller.CalculateSHA(request, CancellationToken.None);

		//Assert
		Assert.True(result.IsSuccess);
		Assert.Null(result.Message);
		Assert.That((string)result.Data, Is.EqualTo((string)response.Data));
	}
}