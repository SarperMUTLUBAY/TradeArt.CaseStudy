using TradeArt.CaseStudy.Model;
using TradeArt.CaseStudy.Model.Requests.CaseStudy;

namespace TradeArt.CaseStudy.Facade.Interfaces; 

public interface ICaseStudyFacade {
	BaseResult InvertText(InvertRequest request);
}