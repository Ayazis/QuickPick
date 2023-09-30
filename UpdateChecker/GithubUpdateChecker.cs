using System;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using UpdateDownloader;

namespace UpdateDownloader;

public interface IUpdateChecker
{
	Task<bool> IsUpdateAvailableAsync(UpdateType updateType, Version currentVersion);
	Task<(Version version, string downloadUrl)> GetLatestVersionAsync(UpdateType updateType);
}

public class GitHubUpdateChecker : IUpdateChecker
{
	private readonly string _repoOwner;
	private readonly string _repoName;

	public GitHubUpdateChecker(string repoOwner, string repoName)
	{
		_repoOwner = repoOwner;
		_repoName = repoName;
	}

	public async Task<bool> IsUpdateAvailableAsync(UpdateType updateType, Version currentVersion)
	{
		Version? latestVersion = (await GetLatestVersionAsync(updateType)).version;

		return latestVersion == null ? false : latestVersion > currentVersion;
	}

	public async Task<(Version version, string downloadUrl)> GetLatestVersionAsync(UpdateType updateType)
	{
		using (HttpClient httpClient = new HttpClient())
		{
			httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(_repoName);
			string url = $"https://api.github.com/repos/{_repoOwner}/{_repoName}/releases";

			if (updateType == UpdateType.Stable)
			{
				return await GetLatestStableRelease(httpClient, url);
			}

			if (updateType == UpdateType.Pre_Release)
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
		string fileDownloadUrl = ExtractBetweenParentheses(bodyUrl);


		return (new Version(version), fileDownloadUrl);
	}

	public string ExtractBetweenParentheses(string input)
	{
		int startIndex = input.IndexOf('(') + 1;
		int endIndex = input.IndexOf(')');

		if (startIndex != -1 && endIndex != -1 && endIndex > startIndex)
		{
			return input.Substring(startIndex, endIndex - startIndex);
		}

		return null;
	}

}

