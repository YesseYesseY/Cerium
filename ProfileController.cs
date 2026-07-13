using System.Globalization;

namespace Cerium;

public static class ProfileController
{
    public static IResult PostProfileOperation(string accountId, string operation, HttpRequest request)
    {
        string profileId = "common_core";

        if (request.Query.TryGetValue("profileId", out var val))
            profileId = val.ToString();

        return Results.Json(new
        {
            profileRevision = 1,
            profileId = profileId,
            profileChangesBaseRevision = 1,
            profileChanges = Array.Empty<object>(),
            profileCommandRevision = 1,
            serverTime = DateTime.UtcNow.ToString("o", CultureInfo.InvariantCulture),
            responseVersion = 1
        });
    }
}