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
}