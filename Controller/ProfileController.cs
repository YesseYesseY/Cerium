using System.Text;
using Cerium.Attributes;
using Cerium.Extensions;
using Cerium.Managers;

namespace Cerium.Controller;

public record SetBattleRoyaleBannerBody(string homebaseBannerIconId, string homebaseBannerColorId)
{
}

[CeriumController]
public static class ProfileController
{
    public static object StatModified(string name, string value)
    {
        return new
        {
            changeType = "statModified",
            name = name,
            value = value
        };
    }

    [CeriumRoute("POST", "/fortnite/api/game/v2/profile/{accountId}/client/{operation}")]
    public static async Task<IResult> PostProfileOperation(Guid accountId, string operation, HttpRequest request)
    {
        var buildInfo = request.GetBuildInfo();

        var account = AccountManager.GetFromAccountId(accountId);
        if (account is null)
            return Results.NotFound();

        var profileId = "common_core";
        if (request.Query.TryGetValue("profileId", out var val))
            profileId = val.ToString();

        List<object> changes = new();
        var profile = account.GetOrCreateProfile(profileId);

        var baseRvn = profile.Rvn;

        Console.WriteLine($"Operation: \"{operation}\"");

        var increaseRvn = false;
        switch (operation)
        {
            case "SetBattleRoyaleBanner":
                var body = await request.ReadFromJsonAsync<SetBattleRoyaleBannerBody>();
                if (body is null)
                    break;

                if (profile.TrySetAttribute("banner_color", body.homebaseBannerColorId))
                {
                    changes.Add(StatModified("banner_color", body.homebaseBannerColorId));
                    increaseRvn = true;
                }

                if (profile.TrySetAttribute("banner_icon", body.homebaseBannerIconId))
                {
                    changes.Add(StatModified("banner_icon", body.homebaseBannerIconId));
                    increaseRvn = true;
                }
                break;
            default:
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
                break;
        }

        if (increaseRvn)
        {
            profile.Rvn++;
            AccountManager.Save(account);
        }

        return Results.Json(new
        {
            profileRevision = profile.Rvn,
            profileId = profileId,
            profileChangesBaseRevision = baseRvn,
            profileChanges = changes,
            profileCommandRevision = profile.Rvn,
            serverTime = DateTime.UtcNow,
            responseVersion = 1
        });
    }
}