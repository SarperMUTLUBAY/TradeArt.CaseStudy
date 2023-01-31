using Microsoft.AspNetCore.Mvc;
using TradeArt.CaseStudy.Business.Interfaces;
using TradeArt.CaseStudy.Common.Constants;
using TradeArt.CaseStudy.Model;
using TradeArt.CaseStudy.Model.Requests.CaseStudy;

namespace TradeArt.CaseStudy.Api.Controllers; 

public class CaseStudyController : BaseController {
	
	private readonly ICaseStudyBusiness _business;

	public CaseStudyController(ICaseStudyBusiness business) {
		_business = business;
	}
	
	[HttpPost]
	public BaseResult Invert([FromBody] InvertRequest request) => _business.InvertText(request);
}