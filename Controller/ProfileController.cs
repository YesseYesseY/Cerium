using System.Text;
using Cerium.Attributes;
using Cerium.Extensions;
using Cerium.Managers;

namespace Cerium.Controller;

public record SetBattleRoyaleBannerBody(string homebaseBannerIconId, string homebaseBannerColorId);

public record EquipBattleRoyaleCustomizationBody(
    string slotName,
    string itemToSlot,
    int indexWithinSlot,
    object[] variantUpdates);

public record MarkItemSeenBody(Guid[] itemIds);

[CeriumController]
public static class ProfileController
{
    public static Dictionary<string, string> CosmeticTypeToAttribute = new()
    {
        { "Glider", "favorite_glider"},
        { "Pickaxe", "favorite_pickaxe"},
        { "Character", "favorite_character"},
        { "Dance", "favorite_dance"},
        { "ItemWrap", "favorite_itemwraps"},
        { "Backpack", "favorite_backpack"},
        { "SkyDiveContrail", "favorite_skydivecontrail" },
        { "LoadingScreen", "favorite_loadingscreen" },
        { "MusicPack", "favorite_musicpack"}
    };

    private static object StatModified(string name, object value)
    {
        return new
        {
            changeType = "statModified",
            name = name,
            value = value
        };
    }

    private static object ItemAttributeChanged(Guid itemGuid, string attribName, object attribValue)
    {
        return new
        {
            changeType = "itemAttrChanged",
            itemId = itemGuid,
            attributeName = attribName,
            attributeValue = attribValue
        };
    }

    // From 13.40 I could find:
    //  - itemAdded
    //  - itemRemoved
    //  - itemAttrChanged
    //  - itemQuantityChanged
    //  - statModified
    //  - fullProfileUpdate

    [CeriumRoute("POST", "/fortnite/api/game/v2/profile/{accountId}/client/{operation}")]
    public static async Task<IResult> PostProfileOperation(Guid accountId, string operation, HttpRequest request)
    {
        var buildInfo = request.GetBuildInfo();

        var account = AccountManager.GetFromAccountId(accountId);
        if (account is null)
            return Results.NotFound();

        var legacyLoadout = account.LegacyLoadout;

        var profileId = "common_core";
        if (request.Query.TryGetValue("profileId", out var val))
            profileId = val.ToString();

        List<object> changes = [];

        var baseRvn = account.Rvn;
        var isFullProfileUpdate = false;
        switch (operation)
        {
            case "MarkItemSeen":
                var markseenbody = await request.ReadFromJsonAsync<MarkItemSeenBody>();
                if (markseenbody is null)
                    break;

                foreach (var guid in markseenbody.itemIds)
                {
                    var item = account.GetItem(guid);
                    if (item is null)
                        continue;

                    item.Seen = true;

                    // IDK if this is needed? Like they mark it as seen the moment it sends the request not on this change
                    // I'm doing this because it will save the profile.
                    changes.Add(ItemAttributeChanged(item.ItemGuid, "item_seen", item.Seen));
                }
                break;
            case "SetBattleRoyaleBanner":
                var body = await request.ReadFromJsonAsync<SetBattleRoyaleBannerBody>();
                if (body is null)
                    break;

                if (legacyLoadout.SetValue("BannerColor", body.homebaseBannerColorId) is not null)
                {
                    changes.Add(StatModified("banner_color", body.homebaseBannerColorId));
                }

                if (legacyLoadout.SetValue("BannerIcon", body.homebaseBannerIconId) is not null)
                {
                    changes.Add(StatModified("banner_icon", body.homebaseBannerIconId));
                }
                break;
            case "EquipBattleRoyaleCustomization":
                var equipbody = await request.ReadFromJsonAsync<EquipBattleRoyaleCustomizationBody>();
                if (equipbody is null)
                    break;

                var statValue = legacyLoadout.SetValue(equipbody.slotName, equipbody.itemToSlot, equipbody.indexWithinSlot);

                if (statValue is not null)
                {
                    if (!CosmeticTypeToAttribute.TryGetValue(equipbody.slotName, out var attributeName))
                    {
                        Console.WriteLine($"Cosmetic type not found: \"{equipbody.slotName}\"");
                        break;
                    }

                    changes.Add(StatModified(attributeName, statValue));
                }

                break;
            default:
                Console.WriteLine($"Operation: \"{operation}\"");

                changes.Add(new
                {
                    changeType = "fullProfileUpdate",
                    profile = account.GetProfile(profileId)
                });
                isFullProfileUpdate = true;
                break;
        }

        if (!isFullProfileUpdate && changes.Count > 0)
        {
            account.Rvn++;
            AccountManager.Save(account);
        }

        return Results.Json(new
        {
            profileRevision = account.Rvn,
            profileId = profileId,
            profileChangesBaseRevision = baseRvn,
            profileChanges = changes,
            profileCommandRevision = account.Rvn,
            serverTime = DateTime.UtcNow,
            responseVersion = 1
        });
    }
}