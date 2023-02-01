using TradeArt.CaseStudy.Facade.Interfaces;
using TradeArt.CaseStudy.Model;
using TradeArt.CaseStudy.Model.Requests.CaseStudy;

namespace TradeArt.CaseStudy.Facade.Implementations;

public class CaseStudyFacade : ICaseStudyFacade {
	public BaseResult InvertText(InvertRequest request) {
		if (string.IsNullOrWhiteSpace(request.Text))
			return new ErrorResult("The Text field is required.");

		var charArray = request.Text.ToCharArray();
		Array.Reverse(charArray);
		return new SuccessResult<string>(new string(charArray));
	}
}