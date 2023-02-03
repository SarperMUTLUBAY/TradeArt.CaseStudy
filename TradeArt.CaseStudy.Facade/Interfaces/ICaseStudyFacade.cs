using TradeArt.CaseStudy.Model;
using TradeArt.CaseStudy.Model.Requests.CaseStudy;

namespace TradeArt.CaseStudy.Facade.Interfaces;

public interface ICaseStudyFacade {
	/// <summary>
	/// Invert text
	/// </summary>
	/// <param name="request">InvertRequest model for inverting text</param>
	/// <returns>returns SuccessResult on success, ErrorResult on failure</returns>
	BaseResult InvertText(InvertRequest request);

	/// <summary>
	/// makes Iteration and send data to queue as many as the entered number
	/// </summary>
	/// <param name="request">It contains IterationCount for decide to how many iteration</param>
	/// <returns>returns SuccessResult on success, ErrorResult on failure</returns>
	BaseResult Iteration(IterationRequest request);

	/// <summary>
	/// Calculate a SHA hash (in hex form)
	/// </summary>
	/// <param name="request">It contains FileUrl for file to be download</param>
	/// <param name="cancellationToken">cancellation token for async process</param>
	/// <returns>returns SuccessResult with file sha value on success, ErrorResult on failure</returns>
	Task<BaseResult> CalculateSHA(CalculateShaRequest request, CancellationToken cancellationToken);

	/// <summary>
	/// Get market assets and prices
	/// </summary>
	/// <param name="cancellationToken">cancellation token for async process</param>
	/// <returns>returns SuccessResult with Assets on success, ErrorResult on failure</returns>
	Task<BaseResult> GetAssets(CancellationToken cancellationToken);
}