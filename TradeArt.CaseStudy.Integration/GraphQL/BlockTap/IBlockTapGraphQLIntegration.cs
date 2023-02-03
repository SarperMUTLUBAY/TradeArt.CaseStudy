using TradeArt.CaseStudy.Integration.GraphQL.BlockTap.Responses;

namespace TradeArt.CaseStudy.Integration.GraphQL.BlockTap;

public interface IBlockTapGraphQLIntegration {
	Task<ICollection<Asset>> GetAssetsAsync(int limit, CancellationToken cancellationToken);
	Task<ICollection<Price>> GetPricesAsync(List<string> assetSymbols, CancellationToken cancellationToken);
}