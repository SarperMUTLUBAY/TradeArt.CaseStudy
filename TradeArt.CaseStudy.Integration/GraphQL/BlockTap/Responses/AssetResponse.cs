namespace TradeArt.CaseStudy.Integration.GraphQL.BlockTap.Responses;

public class AssetResponse {
	public ICollection<Asset> Assets { get; set; }
}

public class Asset {
	public string AssetName { get; set; }
	public string AssetSymbol { get; set; }
	public long? MarketCap { get; set; }
}