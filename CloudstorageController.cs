namespace Cerium;

public static class CloudstorageController
{
    public static IResult GetSystemFiles(HttpRequest request)
    {
        return Results.Json(Array.Empty<object>());
    }

    public static IResult GetUserFiles(string accountId, HttpRequest request)
    {
        return Results.Json(Array.Empty<object>());
    }
}