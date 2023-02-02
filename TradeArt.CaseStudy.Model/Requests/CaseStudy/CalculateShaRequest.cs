using System.ComponentModel;

namespace TradeArt.CaseStudy.Model.Requests.CaseStudy;

public class CalculateShaRequest {
	[DefaultValue("https://speed.hetzner.de/100MB.bin")]
	public string FileUrl { get; set; }
}