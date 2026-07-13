using System.Text.Json.Serialization;

namespace Cerium;

public class Profile(string id)
{
    public string Id { get; set; } = id;
    public List<Item> Items { get; set; } = new();
    public Dictionary<string, object> Attributes { get; set; } = new();
    public int Rvn { get; set; } = 1;

    public void SetAttribute(string key, object val)
    {
        Attributes[key] = val;
    }

    public void AddItem(Item item)
    {
        Items.Add(item);
    }
}

public class CommonPublicProfile() : Profile("common_public");

public class CommonCoreProfile : Profile
{
    public CommonCoreProfile() : base("common_core")
    {
        // TODO 23 more from 11.00
        for (var i = 0; i < 21; i++)
            AddItem(new Item($"HomebaseBannerColor:DefaultColor{i + 1}"));

        for (var i = 0; i < 31; i++)
            AddItem(new Item($"HomebaseBannerIcon:StandardBanner{i + 1}"));
    }
}

public class AthenaProfile : Profile
{
    public AthenaProfile() : base("athena")
    {
        SetAttribute("favorite_skydivecontrail", "" );
        SetAttribute("favorite_glider", "" );
        SetAttribute("favorite_pickaxe", "" );
        SetAttribute("favorite_character", "" );
        SetAttribute("favorite_backpack", "" );
        SetAttribute("favorite_hat", "" );
        SetAttribute("favorite_loadingscreen", "" );
        SetAttribute("favorite_battlebus", "" );
        SetAttribute("favorite_vehicledeco", "" );
        SetAttribute("favorite_callingcard", "" );
        SetAttribute("favorite_mapmarker", "" );
        SetAttribute("favorite_musicpack", "" );
        SetAttribute("favorite_itemwraps", new[] { "", "", "", "", "", "" } );
        SetAttribute("favorite_pet", "" );
        SetAttribute("favorite_dance", new[] { "", "", "", "", "", "" } );
        SetAttribute("favorite_victorypose", "" );
        SetAttribute("favorite_personal_vehicle", "" );
        SetAttribute("banner_color", "" );
        SetAttribute("banner_icon", "" );
        SetAttribute("level", 1 );
        SetAttribute("season_num", 7 );
        SetAttribute("book_level", 1 );
        SetAttribute("book_purchased", false);
    }
}