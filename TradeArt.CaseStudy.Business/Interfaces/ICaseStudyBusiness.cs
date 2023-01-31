using TradeArt.CaseStudy.Model;
using TradeArt.CaseStudy.Model.Requests.CaseStudy;

namespace TradeArt.CaseStudy.Business.Interfaces; 

public interface ICaseStudyBusiness {
	BaseResult InvertText(InvertRequest request);
}