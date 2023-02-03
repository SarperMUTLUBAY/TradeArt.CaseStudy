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

	[HttpPost]
	public BaseResult Iteration(IterationRequest request) => _facade.Iteration(request);

	[HttpPost]
	public async Task<BaseResult> CalculateSHA([FromBody] CalculateShaRequest request, CancellationToken cancellationToken) => await _facade.CalculateSHA(request, cancellationToken);

	[HttpPost]
	public async Task<BaseResult> GetAssets(CancellationToken cancellationToken) {
		var data = await _facade.GetAssets(cancellationToken);
		return data;
	}
}