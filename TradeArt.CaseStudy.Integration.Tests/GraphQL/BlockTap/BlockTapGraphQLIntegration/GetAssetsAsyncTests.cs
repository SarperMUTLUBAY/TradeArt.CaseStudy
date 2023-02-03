using GraphQL;
using GraphQL.Client.Abstractions;
using Moq;
using TradeArt.CaseStudy.Integration.GraphQL.BlockTap.Responses;

namespace TradeArt.CaseStudy.Integration.Tests.GraphQL.BlockTap.BlockTapGraphQLIntegration;

[TestFixture]
public class GetAssetsAsyncTests {
	private Mock<IGraphQLClient> _mockGraphQlClient;

	[SetUp]
	public void Setup() {
		_mockGraphQlClient = new Mock<IGraphQLClient>();
	}

	[Test]
	public async Task GetAssetsAsync_Returns_MoreThanOneData() {
		//Arrange
		var response = new GraphQLResponse<AssetResponse> {
			Data = new AssetResponse {
				Assets = new List<Asset> {
					new() {
						AssetName = "Bitcoin",
						AssetSymbol = "BTC",
						MarketCap = 1000000000000
					},
					new() {
						AssetName = "Ethereum",
						AssetSymbol = "ETH",
						MarketCap = 100000000000
					}
				}
			}
		};

		_mockGraphQlClient.Setup(x => x.SendQueryAsync<AssetResponse>(It.IsAny<GraphQLRequest>(), CancellationToken.None))
						  .ReturnsAsync(response);

		var integration = new Integration.GraphQL.BlockTap.BlockTapGraphQLIntegration(_mockGraphQlClient.Object);

		//Act
		var result = await integration.GetAssetsAsync(It.IsAny<int>(), CancellationToken.None);

		//Assert
		Assert.NotNull(result);
		Assert.True(result is {Count: > 0});
		Assert.True(result.Any(x => x.AssetName == "Bitcoin"));
		Assert.True(result.Any(x => x.AssetSymbol == "BTC"));
		Assert.True(result.Any(x => x.MarketCap == 1000000000000));
	}

	[Test]
	public async Task GetAssetsAsync_Returns_Null() {
		//Arrange
		var response = new GraphQLResponse<AssetResponse>();

		_mockGraphQlClient.Setup(x => x.SendQueryAsync<AssetResponse>(It.IsAny<GraphQLRequest>(), CancellationToken.None))
						  .ReturnsAsync(response);

		var integration = new Integration.GraphQL.BlockTap.BlockTapGraphQLIntegration(_mockGraphQlClient.Object);

		//Act
		var result = await integration.GetAssetsAsync(It.IsAny<int>(), CancellationToken.None);

		//Assert
		Assert.IsNull(result);
	}
}