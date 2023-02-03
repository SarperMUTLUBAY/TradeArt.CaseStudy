namespace TradeArt.CaseStudy.Integration.GraphQL.BlockTap.Responses;

public class AssetPriceResponse {
	public List<Price> Markets { get; set; }
}

public class Price {
	public string BaseSymbol { get; set; }
	public string MarketSymbol { get; set; }
	public Ticker Ticker { get; set; }
}

public class Ticker {
	public string LastPrice { get; set; }
}