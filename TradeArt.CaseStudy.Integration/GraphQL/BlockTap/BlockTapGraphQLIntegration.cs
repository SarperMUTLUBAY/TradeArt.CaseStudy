using GraphQL;
using GraphQL.Client.Abstractions;
using TradeArt.CaseStudy.Integration.GraphQL.BlockTap.Responses;

namespace TradeArt.CaseStudy.Integration.GraphQL.BlockTap;

public class BlockTapGraphQLIntegration : IBlockTapGraphQLIntegration {
	private readonly IGraphQLClient _graphQlClient;

	public BlockTapGraphQLIntegration(IGraphQLClient graphQlClient) {
		_graphQlClient = graphQlClient;
	}

	public async Task<ICollection<Asset>> GetAssetsAsync(CancellationToken cancellationToken) {
		var request = new GraphQLRequest {
			Query = @"
		        query Assets {
		          assets(sort: [{marketCapRank: ASC}]) {
		            assetName
		            assetSymbol
		            marketCap
		          }
				}",
			OperationName = "Assets",
			Variables = null
		};

		var assetResponse = await _graphQlClient.SendQueryAsync<AssetResponse>(request, cancellationToken);
		var assetData = assetResponse.Data?.Assets;
		return assetData;
	}

	public async Task<ICollection<Price>> GetPricesAsync(List<string> assetSymbols, CancellationToken cancellationToken) {
		if (assetSymbols == null || assetSymbols.Count == 0) {
			return null;
		}

		var request = new GraphQLRequest {
			Query = @$"
                            query Prices {{
                              markets(filter: {{baseSymbol: {{_in: [{string.Join(",", assetSymbols.Select(x => $@"""{x}"""))}]}}}}) {{
                                marketSymbol
                                baseSymbol
                                ticker {{
                                   lastPrice
                                }}
                              }}
                            }}",
			OperationName = "Prices",
			Variables = null
		};

		var priceResponse = await _graphQlClient.SendQueryAsync<AssetPriceResponse>(request, cancellationToken);
		var priceData = priceResponse.Data?.Markets;
		return priceData;
	}
}