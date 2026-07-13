using System.Text.RegularExpressions;

namespace Cerium.Extensions;

public struct FortniteBuildInfo
{
    private static readonly Regex UserAgentRegex = new Regex(@"\+\+Fortnite\+Release-([0-9]+\.[0-9]+)-CL-([0-9]+)");

    public readonly float Build;
    public readonly int Season;

    public FortniteBuildInfo(float build)
    {
        Build = build;
        Season = (int)MathF.Floor(build);
    }

    public FortniteBuildInfo(string userAgent)
    {
        var match = UserAgentRegex.Match(userAgent);
        if (!match.Success)
        {
            Build = 0.0f;
            Season = 0;
        }
        else
        {
            Build = float.Parse(match.Groups[1].Value);
            Season = (int)MathF.Floor(Build);
        }
    }
}

public static class HttpRequestExtensions
{
    public static FortniteBuildInfo GetBuildInfo(this HttpRequest request)
    {
        if (request.Headers.TryGetValue("User-Agent", out var userAgent))
        {
            return new FortniteBuildInfo(userAgent.ToString());
        }

        return new FortniteBuildInfo(0.0f);
    }
}