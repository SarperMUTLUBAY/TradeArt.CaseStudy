using Microsoft.AspNetCore.Mvc;
using TradeArt.CaseStudy.Facade.Interfaces;
using TradeArt.CaseStudy.Model;
using TradeArt.CaseStudy.Model.Requests.CaseStudy;

namespace TradeArt.CaseStudy.Api.Controllers; 

public class CaseStudyController : BaseController {
	
	private readonly ICaseStudyFacade _facade;

	public CaseStudyController(ICaseStudyFacade facade) {
		_facade = facade;
	}
	
	[HttpPost]
	public BaseResult Invert([FromBody] InvertRequest request) => _facade.InvertText(request);
}