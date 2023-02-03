using System.ComponentModel;

namespace TradeArt.CaseStudy.Model.Requests.CaseStudy;

public class CalculateShaRequest {
	[DefaultValue("https://file-examples.com/wp-content/uploads/2017/02/zip_10MB.zip")]
	public string FileUrl { get; set; }
}