using Cerium.Attributes;
using Cerium.Extensions;
using Cerium.Managers;

namespace Cerium.Controller;

[CeriumController]
public static class ProfileController
{
    [CeriumRoute("POST", "/fortnite/api/game/v2/profile/{accountId}/client/{operation}")]
    public static IResult PostProfileOperation(Guid accountId, string operation, HttpRequest request)
    {
        var buildInfo = request.GetBuildInfo();

        var account = AccountManager.GetFromAccountId(accountId);
        if (account is null)
            return Results.NotFound();

        string profileId = "common_core";

        if (request.Query.TryGetValue("profileId", out var val))
            profileId = val.ToString();

        List<object> changes = new();
        var profile = account.GetProfile(profileId);

        if (profile is not null)
        {
            var change = new
            {
                changeType = "fullProfileUpdate",
                profile = new
                {
                    _id = "yes",
                    created = account.Created,
                    updated = account.Created,
                    rvn = profile.Rvn,
                    wipeNumber = 0,
                    accountId = account.Id,
                    profileId = profileId,
                    version = "uwu",
                    items = profile.Items
                        .Where(i => i.BuildLimit < 0.0f || buildInfo.Build >= i.BuildLimit )
                        .ToDictionary(i => i.Id, i => i.Objectify()),
                    stats = new
                    {
                        attributes = profile.Attributes
                    },
                    commandRevision = profile.Rvn
                }
            };

            if (profileId == "athena")
                change.profile.stats.attributes["season_num"] = buildInfo.Season;

            changes.Add(change);
        }
        else
        {
            Console.WriteLine($"Profile not found: {profileId}");
        }

        return Results.Json(new
        {
            profileRevision = 1,
            profileId = profileId,
            profileChangesBaseRevision = 1,
            profileChanges = changes,
            profileCommandRevision = 1,
            serverTime = DateTime.UtcNow,
            responseVersion = 1
        });
    }
}