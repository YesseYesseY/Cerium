using Cerium.Attributes;

namespace Cerium.Controller;

[CeriumController]
public static class CloudstorageController
{
    [CeriumRoute("GET", "/fortnite/api/cloudstorage/system")]
    public static IResult GetSystemFiles(HttpRequest request)
    {
        return Results.Json(Array.Empty<object>());
    }

    [CeriumRoute("GET", "/fortnite/api/cloudstorage/user/{accountId}")]
    public static IResult GetUserFiles(string accountId, HttpRequest request)
    {
        return Results.Json(Array.Empty<object>());
    }
}