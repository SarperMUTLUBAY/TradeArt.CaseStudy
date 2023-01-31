using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TradeArt.CaseStudy.Common.Constants;

namespace TradeArt.CaseStudy.Model.Requests.CaseStudy; 

public class InvertRequest {
	[Required]
	[DefaultValue(CaseStudyContants.TextToBeInverted)]
	public string Text { get; set; }
}