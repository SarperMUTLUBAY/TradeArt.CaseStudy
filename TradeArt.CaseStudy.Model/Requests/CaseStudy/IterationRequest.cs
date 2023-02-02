using System.ComponentModel;

namespace TradeArt.CaseStudy.Model.Requests.CaseStudy; 

public class IterationRequest {
	[DefaultValue(1000)]
	public int IterationCount { get; set; }
}