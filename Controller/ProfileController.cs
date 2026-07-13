using Cerium.Attributes;
using Cerium.Extensions;

namespace Cerium.Controller;

[CeriumController]
public static class ProfileController
{
    [CeriumRoute("POST", "/fortnite/api/game/v2/profile/{accountId}/client/{operation}")]
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
            serverTime = DateTime.UtcNow.ToIsoString(),
            responseVersion = 1
        });
    }
}