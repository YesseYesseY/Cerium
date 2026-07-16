using System.Text;
using Cerium.Attributes;
using Cerium.Extensions;
using Cerium.Managers;

namespace Cerium.Controller;

// ReSharper disable InconsistentNaming

public record SetBattleRoyaleBannerBody(
    string homebaseBannerIconId,
    string homebaseBannerColorId);

public record EquipBattleRoyaleCustomizationBody(
    string slotName,
    string itemToSlot,
    int indexWithinSlot,
    object[] variantUpdates);

public record MarkItemSeenBody(
    Guid[] itemIds);

public record CopyCosmeticLoadoutBody(
    int sourceIndex,
    int targetIndex,
    string optNewNameForTarget);

public record SetCosmeticLockerNameBody(
    Guid lockerItem,
    string name);

public record SetCosmeticLockerSlotBody(
    Guid lockerItem,
    string category,
    string itemToSlot,
    int slotIndex,
    object[] variantUpdates,
    int optLockerUseCountOverride);

public record SetRandomCosmeticLoadoutFlagBody(
    bool random);

public record DeleteCosmeticLoadoutBody(
    int index,
    int fallbackLoadoutIndex,
    bool leaveNullSlot);

public record SetCosmeticLockerBannerBody(
    Guid lockerItem,
    string bannerIconTemplateName,
    string bannerColorTemplateName);

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

    private static object ItemRemoved(Guid itemGuid)
    {
        return new
        {
            changeType = "itemRemoved",
            itemId = itemGuid
        };
    }

    private static object ItemAdded(Guid itemId, object item)
    {
        return new
        {
            changeType = "itemAdded",
            item = item,
            itemId = itemId
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
            case "PurchaseCatalogEntry":
            {
                await request.WriteBody();
                break;
            }
            case "SetCosmeticLockerBanner":
            {
                var body = await request.ReadFromJsonAsync<SetCosmeticLockerBannerBody>();
                if (body is null)
                    break;

                var loadoutItem = (LoadoutItem?)account.GetItem(body.lockerItem);
                if (loadoutItem is null)
                    break;

                var loadout = loadoutItem.ItemLoadout;
                if (loadout.BannerColor != body.bannerColorTemplateName)
                {
                    loadout.BannerColor = body.bannerColorTemplateName;
                    changes.Add(ItemAttributeChanged(body.lockerItem, "banner_color_template", loadout.BannerColor));
                }

                if (loadout.BannerIcon != body.bannerIconTemplateName)
                {
                    loadout.BannerIcon = body.bannerIconTemplateName;
                    changes.Add(ItemAttributeChanged(body.lockerItem, "banner_icon_template", loadout.BannerIcon));
                }
                break;
            }
            case "DeleteCosmeticLoadout":
            {
                var body = await request.ReadFromJsonAsync<DeleteCosmeticLoadoutBody>();
                if (body is null)
                    break;

                var itemGuid = account.Loadouts[body.index];
                var item = account.GetItem(itemGuid);
                if (item is null)
                    break;

                account.RemoveItem(item);
                account.Loadouts.RemoveAt(body.index);

                changes.Add(ItemRemoved(itemGuid));
                changes.Add(StatModified("loadouts", account.Loadouts));
                break;
            }
            case "SetRandomCosmeticLoadoutFlag":
            {
                var body = await request.ReadFromJsonAsync<SetRandomCosmeticLoadoutFlagBody>();
                if (body is null)
                    break;

                if (account.UseRandomLoadouts != body.random)
                {
                    account.UseRandomLoadouts = body.random;
                    changes.Add(StatModified("use_random_loadout", body.random));
                }
                break;
            }
            case "SetCosmeticLockerName":
            {
                var body = await request.ReadFromJsonAsync<SetCosmeticLockerNameBody>();
                if (body is null)
                    break;

                var item = (LoadoutItem?)account.GetItem(body.lockerItem);
                if (item is null)
                    break;

                item.LoadoutName = body.name;

                changes.Add(ItemAttributeChanged(item.ItemGuid, "locker_name", item.LoadoutName));
                break;
            }
            case "SetCosmeticLockerSlot":
            {
                var body = await request.ReadFromJsonAsync<SetCosmeticLockerSlotBody>();
                if (body is null)
                    break;

                var loadout = (LoadoutItem?)account.GetItem(body.lockerItem);
                if (loadout is null)
                    break;

                if (loadout.ItemLoadout.SetValue(body.category, body.itemToSlot, body.slotIndex) is not null)
                {
                    changes.Add(ItemAttributeChanged(loadout.ItemGuid, "locker_slots_data",
                        loadout.GetLockerSlotsData()));
                }
                break;
            }
            case "CopyCosmeticLoadout":
            {
                var body = await request.ReadFromJsonAsync<CopyCosmeticLoadoutBody>();
                if (body is null || account.Loadouts.Count <= body.sourceIndex)
                    break;

                var sourceGuid = account.Loadouts[body.sourceIndex];
                var sourceLoadout = ((LoadoutItem?)account.GetItem(sourceGuid));
                if (sourceLoadout is null)
                    break;

                var chosenName = sourceLoadout.LoadoutName == "" ? body.optNewNameForTarget : sourceLoadout.LoadoutName;

                if (account.Loadouts.IsValidIndex(body.targetIndex))
                {
                    var targetGuid = account.Loadouts[body.targetIndex];
                    var targetLoadoutItem = ((LoadoutItem?)account.GetItem(targetGuid));
                    if (targetLoadoutItem is null)
                        break;

                    var targetLoadout = targetLoadoutItem.ItemLoadout;
                    targetLoadout.CopyFrom(sourceLoadout.ItemLoadout);
                    targetLoadoutItem.LoadoutName = chosenName;

                    changes.Add(ItemAttributeChanged(targetGuid, "locker_name", chosenName));
                    changes.Add(ItemAttributeChanged(targetGuid, "banner_color_template", targetLoadout.BannerColor));
                    changes.Add(ItemAttributeChanged(targetGuid, "banner_icon_template", targetLoadout.BannerIcon));
                    changes.Add(ItemAttributeChanged(targetGuid, "locker_slots_data",
                        targetLoadoutItem.GetLockerSlotsData()));
                }
                else
                {
                    var newLoadoutItem = account.AddLoadout(chosenName, sourceLoadout);
                    changes.Add(ItemAdded(newLoadoutItem.ItemGuid, newLoadoutItem.Objectify(account)));
                    changes.Add(StatModified("loadouts", account.Loadouts));
                }

                break;
            }
            case "MarkItemSeen":
            {
                var body = await request.ReadFromJsonAsync<MarkItemSeenBody>();
                if (body is null)
                    break;

                foreach (var guid in body.itemIds)
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
            }
            case "SetBattleRoyaleBanner":
            {
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
            }
            case "EquipBattleRoyaleCustomization":
            {
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
            }
            default:
            {
                Console.WriteLine($"Operation: \"{operation}\"");
                changes.Add(new
                {
                    changeType = "fullProfileUpdate",
                    profile = account.GetProfile(profileId)
                });
                isFullProfileUpdate = true;
                break;
            }
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