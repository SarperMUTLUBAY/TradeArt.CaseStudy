namespace TradeArt.CaseStudy.Model.Responses.CaseStudy; 

public class GetAssetsResponse {
	public ICollection<Asset> Assets { get; } = new List<Asset>();
}

public class Asset
{
	public string AssetName { get; set; }
	public string AssetSymbol { get; set; }
	public long? MarketCap { get; set; }
	public IEnumerable<AssetPrice> Prices { get; set; }
}

public class AssetPrice
{
	public string MarketSymbol { get; set; }
	public string Price { get; set; }
}