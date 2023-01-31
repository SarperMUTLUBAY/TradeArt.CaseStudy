namespace TradeArt.CaseStudy.Model;

public abstract class BaseResult {
	public bool IsSuccess { get; set; }
	public string Message { get; set; }
	public object Data { get; set; }

	protected BaseResult(string message = null) {
		Message = message;
	}
}

public abstract class BaseResult<T> : BaseResult {
	protected BaseResult(T data, string message = null) : base(message) {
		Data = data;
	}
}

public class SuccessResult<T> : BaseResult<T> {
	public SuccessResult(T data = default, string message = null) : base(data, message) {
		IsSuccess = true;
	}
}

public class ErrorResult : BaseResult {
	public ErrorResult(string message) : base(message) {
		IsSuccess = false;
	}
}