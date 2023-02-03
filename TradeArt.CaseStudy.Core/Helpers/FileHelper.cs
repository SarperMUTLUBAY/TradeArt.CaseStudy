using TradeArt.CaseStudy.Common;

namespace TradeArt.CaseStudy.Core.Helpers;

public static class FileHelper {
	/// <summary>
	/// Download file from url and save it to the specified path.
	/// </summary>
	/// <param name="url">The url of the file to be downloaded</param>
	/// <param name="pathToSave">save path for downloaded file</param>
	/// <param name="fileName">file name for local file</param>
	/// <param name="overwrite">delete file if exists</param>
	/// <param name="cancellationToken">cancellation token for async process</param>
	/// <returns></returns>
	/// <exception cref="ArgumentNullException">returns this exception when required parameters is null.</exception>
	/// <exception cref="CaseStudyException">returns this exception when file content null or file exists in the directory and overwrite parameter is false.</exception>
	public static async Task<string> DownloadFileAsync(string url, string pathToSave, string fileName, bool overwrite = false, CancellationToken cancellationToken = default) {
		if (string.IsNullOrWhiteSpace(url))
			throw new ArgumentNullException(nameof(url), "Url cannot be null or whitespace.");

		if (string.IsNullOrWhiteSpace(pathToSave))
			throw new ArgumentNullException(nameof(pathToSave), "File path cannot be null or whitespace.");

		if (string.IsNullOrWhiteSpace(fileName))
			throw new ArgumentNullException(nameof(fileName), "Filename cannot be null or whitespace.");

		var fullPath = $"{pathToSave}/{fileName}";

		if (File.Exists(fullPath) && !overwrite)
			throw new CaseStudyException("File already exists.");

		var content = await GetUrlContentAsync(url, cancellationToken);

		if (content is null)
			throw new CaseStudyException("Url content is null.");

		if (!Directory.Exists(pathToSave))
			Directory.CreateDirectory(pathToSave);

		File.Delete(fullPath);
		await File.WriteAllBytesAsync(fullPath, content, cancellationToken);
		return fullPath;
	}
	
	/// <summary>
	/// Get file name from url.
	/// </summary>
	/// <param name="url">The url of the file to be downloaded</param>
	/// <returns>File name in url</returns>
	/// <exception cref="ArgumentNullException">returns this exception when the url parameter is null or whitespace.</exception>
	public static string GetFileNameFromUrl(string url) {
		if (url is null)
			throw new ArgumentNullException(nameof(url), "Url cannot be null or whitespace.");
		
		if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
			uri = new Uri(url);

		return Path.GetFileName(uri.LocalPath);
	}

	/// <summary>
	/// Get file content from url.
	/// </summary>
	/// <param name="url">The url of the file to be downloaded</param>
	/// <param name="cancellationToken">cancellation token for async process</param>
	/// <returns>File bytes or null</returns>
	public static async Task<byte[]> GetUrlContentAsync(string url, CancellationToken cancellationToken = default) {
		using var client = new HttpClient();
		using var result = await client.GetAsync(url, cancellationToken);

		return result.IsSuccessStatusCode ? await result.Content.ReadAsByteArrayAsync(cancellationToken) : null;
	}
}