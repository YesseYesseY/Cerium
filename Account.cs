using System.Runtime.InteropServices.JavaScript;
using System.Text.Json.Serialization;
using Cerium.Extensions;

namespace Cerium;

public class Account
{
    public Guid Id { get; init; }
    public string Username { get; init; }
    public DateTime Created { get; init; } = DateTime.UtcNow;
    public List<Item> Items { get; init; } = [];
    public Loadout CurrentLoadout { get; init; } = new();

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

        for (var i = 0; i < 44; i++)
        {
            Items.Add(new Item("common_core", $"HomebaseBannerColor:DefaultColor{i + 1}")
            {
                BuildLimit = i >= 21 ? 11.00f : -1.0f
            });
        }

        for (var i = 0; i < 31; i++)
            Items.Add(new Item("common_core", $"HomebaseBannerIcon:StandardBanner{i + 1}"));

        Items.Add(new Item("athena", "AthenaDance:EID_DanceMoves"));
        Items.Add(new Item("athena", "AthenaDance:EID_BoogieDown"));
        Items.Add(new Item("athena", "AthenaPickaxe:DefaultPickaxe"));
        Items.Add(new Item("athena", "AthenaGlider:DefaultGlider"));
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
                .Where(i => i.ProfileId == profileId)
                .Where(i => i.BuildLimit < 0.0f || CurrentBuildInfo.Build >= i.BuildLimit )
                .ToDictionary(i => i.Id, i => i.Objectify()),
            stats = new
            {
                attributes = new
                {
                    favorite_skydivecontrail = CurrentLoadout.Contrail,
                    favorite_glider = CurrentLoadout.Glider,
                    favorite_pickaxe = CurrentLoadout.Pickaxe,
                    favorite_character = CurrentLoadout.Character,
                    favorite_backpack = CurrentLoadout.Backpack,
                    favorite_loadingscreen = CurrentLoadout.LoadingScreen,
                    favorite_musicpack = CurrentLoadout.MusicPack,
                    favorite_itemwraps = CurrentLoadout.ItemWrap,
                    favorite_dance = CurrentLoadout.Dance,
                    banner_color = CurrentLoadout.BannerColor,
                    banner_icon = CurrentLoadout.BannerIcon,
                    level = 1 ,
                    book_level = 1 ,
                    book_purchased = false,
                    season_num = CurrentBuildInfo.Season
                }
            },
            commandRevision = Rvn
        };
    }
}