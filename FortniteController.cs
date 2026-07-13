namespace Cerium;

public static class FortniteController
{
    public static IResult PostTryPlayOnPlatform(string accountId, HttpRequest request)
    {
        return Results.Text("true");
    }

    public static IResult GetVersionCheck(HttpRequest request)
    {
        return Results.Json(new
        {
            type = "NO_UPDATE"
        });
    }

    public static IResult GetEnableFeatures(HttpRequest request)
    {
        return Results.Json(Array.Empty<object>());
    }
}