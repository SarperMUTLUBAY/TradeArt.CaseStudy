using GraphQL;
using GraphQL.Client.Abstractions;
using Moq;
using TradeArt.CaseStudy.Integration.GraphQL.BlockTap.Responses;

namespace TradeArt.CaseStudy.Integration.Tests.GraphQL.BlockTap.BlockTapGraphQLIntegration;

[TestFixture]
public class GetPricesAsyncTests {
	private Mock<IGraphQLClient> _mockGraphQlClient;

	[SetUp]
	public void Setup() {
		_mockGraphQlClient = new Mock<IGraphQLClient>();
	}

	[Test]
	public async Task GetPricesAsync_Returns_MoreThanOneData() {
		//Arrange
		var priceSymbols = new List<string> {
			"BTC",
			"ETH"
		};

		var response = new GraphQLResponse<AssetPriceResponse> {
			Data = new AssetPriceResponse {
				Markets = new List<Price> {
					new() {
						MarketSymbol = "BTCUSD",
						BaseSymbol = "BTC",
						Ticker = new Ticker {LastPrice = "100"}
					},
					new() {
						MarketSymbol = "ETHUSD",
						BaseSymbol = "ETH",
						Ticker = new Ticker {LastPrice = "200"}
					}
				}
			}
		};

		_mockGraphQlClient.Setup(x => x.SendQueryAsync<AssetPriceResponse>(It.IsAny<GraphQLRequest>(), CancellationToken.None))
						  .ReturnsAsync(response);

		var integration = new Integration.GraphQL.BlockTap.BlockTapGraphQLIntegration(_mockGraphQlClient.Object);

		//Act
		var result = await integration.GetPricesAsync(priceSymbols, CancellationToken.None);

		//Assert
		Assert.NotNull(result);
		Assert.True(result is {Count: > 0});
		Assert.True(result.Any(x => x.MarketSymbol == "BTCUSD"));
		Assert.True(result.Any(x => x.BaseSymbol == "BTC"));
		Assert.True(result.All(x => x.Ticker != null));
		Assert.True(result.Any(x => x.BaseSymbol == "BTC" && x.Ticker.LastPrice == "100"));
	}

	[Test]
	public async Task GetPricesAsync_Returns_Null() {
		//Arrange
		var priceSymbols = new List<string>();
		var response = new GraphQLResponse<AssetPriceResponse> {
			Data = new AssetPriceResponse {
				Markets = new List<Price> {
					new() {
						MarketSymbol = "BTCUSD",
						BaseSymbol = "BTC"
					},
					new() {
						MarketSymbol = "ETHUSD",
						BaseSymbol = "ETH"
					}
				}
			}
		};

		_mockGraphQlClient.Setup(x => x.SendQueryAsync<AssetPriceResponse>(It.IsAny<GraphQLRequest>(), CancellationToken.None))
						  .ReturnsAsync(response);

		var integration = new Integration.GraphQL.BlockTap.BlockTapGraphQLIntegration(_mockGraphQlClient.Object);

		//Act
		var result = await integration.GetPricesAsync(priceSymbols, CancellationToken.None);

		//Assert
		Assert.IsNull(result);
	}
}