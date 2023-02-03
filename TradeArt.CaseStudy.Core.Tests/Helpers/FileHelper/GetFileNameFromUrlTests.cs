namespace TradeArt.CaseStudy.Core.Tests.Helpers.FileHelper;

[TestFixture]
public class GetFileNameFromUrlTests {
	[Test]
	public void GetFileNameFromUrl_Returns_Success() {
		//Arrange
		var url = "https://speed.hetzner.de/100MB.bin";
		var resultMessage = "100MB.bin";

		// Act
		var result = TradeArt.CaseStudy.Core.Helpers.FileHelper.GetFileNameFromUrl(url);

		//Assert
		Assert.NotNull(result);
		Assert.That(resultMessage, Is.EqualTo(result));
	}

	[Test]
	public void GetFileNameFromUrl_WhenUrlIsNull_ThrowsArgumentNullException() {
		//Arrange
		var paramName = "url";
		var exceptionMessage = "Url cannot be null or whitespace. (Parameter 'url')";

		// Act
		var exception = Assert.Throws<ArgumentNullException>(() => TradeArt.CaseStudy.Core.Helpers.FileHelper.GetFileNameFromUrl(null));

		//Assert
		Assert.NotNull(exception);
		Assert.That(paramName, Is.EqualTo(exception.ParamName));
		Assert.That(exceptionMessage, Is.EqualTo(exception.Message));
	}
}