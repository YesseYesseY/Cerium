using Cerium.Attributes;

namespace Cerium.Controller;

[CeriumController]
public static class FortniteController
{
    [CeriumRoute("POST", "/fortnite/api/game/v2/tryPlayOnPlatform/account/{accountId}")]
    public static IResult PostTryPlayOnPlatform(string accountId, HttpRequest request)
    {
        return Results.Text("true");
    }

    [CeriumRoute("GET", "/fortnite/api/v2/versioncheck/Windows")]
    public static IResult GetVersionCheck(HttpRequest request)
    {
        return Results.Json(new
        {
            type = "NO_UPDATE"
        });
    }

    [CeriumRoute("GET", "/fortnite/api/game/v2/enabled_features")]
    public static IResult GetEnableFeatures(HttpRequest request)
    {
        return Results.Json(Array.Empty<object>());
    }

    [CeriumRoute("POST", "/datarouter/api/v1/public/data")]
    public static IResult GetDataRouterData(HttpRequest request)
    {
        return Results.NoContent();
    }

    public record StatRow(
        string name,
        int value,
        string window,
        int ownerType = 1);

    [CeriumRoute("GET", "/fortnite/api/stats/accountId/{accountId}/bulk/window/{windowId}")]
    public static IResult GetStats(Guid accountId, string windowId, HttpRequest request)
    {
        // Stats from 7.30
        string[] stats =
        [
            "br_placetop1",
            "br_placetop10",
            "br_placetop25",
            "br_placetop5",
            "br_placetop12",
            "br_placetop3",
            "br_placetop6",
            "br_kills",
            "br_matchesplayed",
            "br_minutesplayed",
        ];

        string[] postfixes =
        [
            "_pc_m0_p2",
            "_pc_m0_p10",
            "_pc_m0_p9",
        ];

        List<StatRow> ret = [];
        foreach (var p in postfixes)
            ret.AddRange(stats.Select(s => new StatRow($"{s}{p}", 69420, windowId)));

        return Results.Json(ret);
    }

    // Doesn't show anything, just here to stop crashing
    [CeriumRoute("GET", "/fortnite/api/game/v2/leaderboards/cohort/{accountId}")]
    public static IResult GetLeaderboards(Guid accountId, HttpRequest request)
    {
        return Results.Json(new
        {
            accountId = accountId,
            playlist = request.Query["playlist"].ToString(),
            cohortAccounts = Array.Empty<string>(),
            expiresAt = DateTime.UtcNow.AddHours(-2)
        });
    }
}