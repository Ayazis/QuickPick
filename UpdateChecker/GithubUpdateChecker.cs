using Newtonsoft.Json.Linq;

namespace UpdateDownloader;

public interface IUpdateChecker
{
	Task<bool> IsUpdateAvailableAsync(eUpdateType updateType, Version currentVersion);
	Task<(Version version, string downloadUrl)> GetLatestVersionAsync(eUpdateType updateType);
}

public class GitHubUpdateChecker : IUpdateChecker
{
	private readonly string _repoOwner;
	private readonly string _repoName;

	public GitHubUpdateChecker(string repoOwner, string repoName)
	{
		if (string.IsNullOrWhiteSpace(repoOwner) || string.IsNullOrWhiteSpace(repoName))
			throw new ArgumentNullException("repoOwner and repoName cannot be null or empty.");

		_repoOwner = repoOwner;
		_repoName = repoName;
	}

	public async Task<bool> IsUpdateAvailableAsync(eUpdateType updateType, Version currentVersion)
	{
		(Version version, string downloadUrl) result = await GetLatestVersionAsync(updateType);
		Version? latestVersion = result.version;

		return latestVersion == null ? false : latestVersion > currentVersion;
	}

	public async Task<(Version version, string downloadUrl)> GetLatestVersionAsync(eUpdateType updateType)
	{
		using (HttpClient httpClient = new HttpClient())
		{
			httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(_repoName);
			string url = $"https://api.github.com/repos/{_repoOwner}/{_repoName}/releases";

			if (updateType == eUpdateType.Stable)
			{
				return await GetLatestStableRelease(httpClient, url);
			}

			if (updateType == eUpdateType.Pre_Release)
			{
				return await GetLatestPreRelease(httpClient, url);

			}
		}

		return default;
	}
	private async Task<(Version version, string downloadUrl)> GetLatestStableRelease(HttpClient httpClient, string url)
	{
		url += "/latest"; // pre-releases are not included in this result
		var response = await httpClient.GetStringAsync(url);
		JObject release = JObject.Parse(response);
		return GetVersionAndDownloadLink(release);
	}
	private async Task<(Version version, string downloadUrl)> GetLatestPreRelease(HttpClient httpClient, string url)
	{
		var response = await httpClient.GetStringAsync(url);
		JArray releases = JArray.Parse(response);
		
		// Filter and sort the releases
		var sortedReleases = releases
			.Where(r => r["prerelease"].ToObject<bool>() == true)
			.OrderByDescending(r => r["tag_name"].ToString())
			.ToList();

		if (sortedReleases.Count > 0)
		{
			return GetVersionAndDownloadLink(sortedReleases[0]);

		}
		else
			return default;
	}

	private (Version version, string downloadUrl) GetVersionAndDownloadLink(JToken release)
	{
		string version = release["tag_name"].ToString();
		string bodyUrl = release["body"].ToString();
		string fileDownloadUrl = GetStringBetweenParentheses(bodyUrl);


		return (new Version(version), fileDownloadUrl);
	}

	private string GetStringBetweenParentheses(string input)
	{
		int startIndex = input.IndexOf('(') + 1;
		int endIndex = input.IndexOf(')');

		if (startIndex != -1 && endIndex != -1 && endIndex > startIndex)
		{
			return input.Substring(startIndex, endIndex - startIndex);
		}

		return input;
	}

}

