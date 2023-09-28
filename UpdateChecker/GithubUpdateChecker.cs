using System;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using UpdateChecker;

namespace UpdateChecker;

public interface IUpdateChecker
{
    Task<bool> IsUpdateAvailableAsync(UpdateType updateType, Version currentVersion);
    Task<Version?> GetLatestVersionAsync(UpdateType updateType);
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
        Version? latestVersion = await GetLatestVersionAsync(updateType);

        return latestVersion == null ? false : latestVersion > currentVersion;
    }

    public async Task<Version?> GetLatestVersionAsync(UpdateType updateType)
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

        return null;
    }
    private static async Task<Version?> GetLatestStableRelease(HttpClient httpClient, string url)
    {
        url += "/latest"; // pre-releases are not included in this result
        var response = await httpClient.GetStringAsync(url);
        JObject release = JObject.Parse(response);
        string version = release["tag_name"]?.ToString() ?? string.Empty;
        return new Version(version);
    }


    private static async Task<Version?> GetLatestPreRelease(HttpClient httpClient, string url)
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
            string version = sortedReleases[0]["tag_name"].ToString();
            return new Version(version);
        }
        else
            return null;
    }
}

