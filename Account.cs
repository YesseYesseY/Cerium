using System.Runtime.InteropServices.JavaScript;
using System.Text.Json.Serialization;
using Cerium.Extensions;

namespace Cerium;

public class Account
{
    public Guid Id { get; init; }
    public string Username { get; init; }
    public DateTime Created { get; init; } = DateTime.UtcNow;
    public Dictionary<Guid, Item> Items { get; init; } = [];

    public bool UseRandomLoadouts { get; set; } = false;
    public List<Guid> Loadouts { get; set; } = [];
    public Guid CurrentLoadoutGuid { get; set; } = Guid.Empty;
    public Loadout LegacyLoadout { get; set; } = new();
    [JsonIgnore] public Loadout CurrentLoadout => ((LoadoutItem)Items[CurrentLoadoutGuid]).ItemLoadout;

    [JsonIgnore] public string Email => $"{Id:N}@yesmail.com";
    [JsonIgnore] public FortniteBuildInfo CurrentBuildInfo = new(0.0f);
    [JsonIgnore] public int Rvn = 1;

    public Account()
    {
        Id = Guid.Empty;
        Username = "NoUsername";
    }

    public Account(string username)
    {
        Id = Guid.NewGuid();
        Username = username;

        AddLoadout();
        CurrentLoadoutGuid = Loadouts[0];

        for (var i = 0; i < 44; i++)
        {
            AddItem(new Item("common_core", $"HomebaseBannerColor:DefaultColor{i + 1}")
            {
                BuildLimit = i >= 21 ? 11.00f : -1.0f
            });
        }

        for (var i = 0; i < 31; i++)
            AddItem(new Item("common_core", $"HomebaseBannerIcon:StandardBanner{i + 1}"));

        LegacyLoadout.Dance[0] = AddItem(new Item("athena", "AthenaDance:EID_DanceMoves")).ItemGuid.ToString();
        AddItem(new Item("athena", "AthenaDance:EID_BoogieDown"));
        LegacyLoadout.Pickaxe = AddItem(new Item("athena", "AthenaPickaxe:DefaultPickaxe")).ItemGuid.ToString();
        LegacyLoadout.Glider = AddItem(new Item("athena", "AthenaGlider:DefaultGlider")).ItemGuid.ToString();

        CurrentLoadout.Dance[0] = GetItem(Guid.Parse(LegacyLoadout.Dance[0]))?.Id ?? "";
        CurrentLoadout.Pickaxe = GetItem(Guid.Parse(LegacyLoadout.Pickaxe))?.Id ?? "";
        CurrentLoadout.Glider = GetItem(Guid.Parse(LegacyLoadout.Glider))?.Id ?? "";
    }

    public Item AddItem(Item item)
    {
        Items[item.ItemGuid] = item;
        return item;
    }

    public Item? GetItem(Guid itemGuid) => Items.GetValueOrDefault(itemGuid);
    public Item? GetItem(string itemId) => Items.Values.FirstOrDefault(item => item.Id == itemId);
    public void RemoveItem(Item item) => RemoveItem(item.ItemGuid);
    public void RemoveItem(Guid itemGuid) => Items.Remove(itemGuid);

    public LoadoutItem AddLoadout(string name = "")
    {
        var item = (LoadoutItem)AddItem(new LoadoutItem(name));
        Loadouts.Add(item.ItemGuid);
        return item;
    }

    public LoadoutItem AddLoadout(string name, LoadoutItem loadoutItem)
    {
        var item = (LoadoutItem)AddItem(new LoadoutItem(name, loadoutItem));
        Loadouts.Add(item.ItemGuid);
        return item;
    }

    private object GetProfileAttributes(string profileId)
    {
        if (profileId == "athena")
        {
            return new
            {
                favorite_skydivecontrail = LegacyLoadout.SkyDiveContrail,
                favorite_glider = LegacyLoadout.Glider,
                favorite_pickaxe = LegacyLoadout.Pickaxe,
                favorite_character = LegacyLoadout.Character,
                favorite_backpack = LegacyLoadout.Backpack,
                favorite_loadingscreen = LegacyLoadout.LoadingScreen,
                favorite_musicpack = LegacyLoadout.MusicPack,
                favorite_itemwraps = LegacyLoadout.ItemWrap,
                favorite_dance = LegacyLoadout.Dance,
                banner_color = LegacyLoadout.BannerColor,
                banner_icon = LegacyLoadout.BannerIcon,
                level = 1,
                book_level = 1,
                book_purchased = false,
                season_num = CurrentBuildInfo.Season,
                use_random_loadout = UseRandomLoadouts,
                last_applied_loadout = CurrentLoadoutGuid,
                loadouts = Loadouts
            };
        }

        return new
        {

        };
    }

    public object GetProfile(string profileId)
    {
        return new
        {
            _id = profileId,
            created = Created,
            updated = Created,
            rvn = Rvn,
            wipeNumber = 0,
            accountId = Id,
            profileId = profileId,
            version = "uwu",
            items = Items
                .Where(i => i.Value.ProfileId == profileId)
                .Where(i => i.Value.BuildLimit < 0.0f || CurrentBuildInfo.Build >= i.Value.BuildLimit )
                .ToDictionary(i => i.Key, i => i.Value.Objectify(this)),
            stats = new
            {
                attributes = GetProfileAttributes(profileId)
            },
            commandRevision = Rvn
        };
    }
}