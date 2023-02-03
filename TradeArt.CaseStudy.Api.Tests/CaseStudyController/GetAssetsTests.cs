using Moq;
using TradeArt.CaseStudy.Facade.Interfaces;
using TradeArt.CaseStudy.Model;
using TradeArt.CaseStudy.Model.Requests.CaseStudy;
using TradeArt.CaseStudy.Model.Responses.CaseStudy;

namespace TradeArt.CaseStudy.Api.Tests.CaseStudyController;

[TestFixture]
public class GetAssetsTests {
	private Mock<ICaseStudyFacade> _mockCaseStudyFacade;

	[SetUp]
	public void Setup() {
		_mockCaseStudyFacade = new Mock<ICaseStudyFacade>();
	}

	[Test]
	public async Task GetAssets_Returns_Success() {
		//Arrange
		var request = new GetAssetsRequest {
			TotalCount = 100,
			BatchSize = 20
		};

		var response = new SuccessResult<GetAssetsResponse>();

		_mockCaseStudyFacade.Setup(x => x.GetAssets(request, CancellationToken.None))
							.ReturnsAsync(response);

		var controller = new Controllers.CaseStudyController(_mockCaseStudyFacade.Object);

		//Act
		var result = await controller.GetAssets(request, CancellationToken.None);

		//Assert
		Assert.True(result.IsSuccess);
		Assert.Null(result.Message);
	}
}