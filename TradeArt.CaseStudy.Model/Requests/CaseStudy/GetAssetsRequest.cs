using System.ComponentModel;

namespace TradeArt.CaseStudy.Model.Requests.CaseStudy; 

public class GetAssetsRequest {
	[DefaultValue(100)]
	public int TotalCount { get; set; }
	[DefaultValue(20)]
	public int BatchSize { get; set; }
}