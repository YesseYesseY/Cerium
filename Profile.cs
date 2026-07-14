using System.Text.Json;
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

    public bool TrySetAttribute(string key, object val)
    {
        if (Attributes[key] == val)
            return false;

        Attributes[key] = val;

        return true;
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
        for (var i = 0; i < 44; i++)
        {
            var item = new Item($"HomebaseBannerColor:DefaultColor{i + 1}");

            if (i >= 21)
                item.BuildLimit = 11.00f;

            AddItem(item);
        }

        for (var i = 0; i < 31; i++)
            AddItem(new Item($"HomebaseBannerIcon:StandardBanner{i + 1}"));
    }
}

public class AthenaProfile : Profile
{
    public AthenaProfile() : base("athena")
    {
        SetAttribute("favorite_skydivecontrail", "" );
        SetAttribute("favorite_glider", "AthenaGlider:DefaultGlider" );
        SetAttribute("favorite_pickaxe", "AthenaPickaxe:DefaultPickaxe" );
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
        SetAttribute("favorite_dance", new[] { "AthenaDance:EID_DanceMoves", "AthenaDance:EID_BoogieDown", "", "", "", "" } );
        SetAttribute("favorite_victorypose", "" );
        SetAttribute("favorite_personal_vehicle", "" );
        SetAttribute("banner_color", "DefaultColor21" );
        SetAttribute("banner_icon", "StandardBanner15" );
        SetAttribute("level", 1 );
        SetAttribute("season_num", 7 );
        SetAttribute("book_level", 1 );
        SetAttribute("book_purchased", false);

        // Free items
        AddItem(new Item("AthenaDance:EID_DanceMoves"));
        AddItem(new Item("AthenaDance:EID_BoogieDown"));

        AddItem(new Item("AthenaPickaxe:DefaultPickaxe"));

        AddItem(new Item("AthenaGlider:DefaultGlider"));
        AddItem(new Item("AthenaGlider:Umbrella_SnowFlake"));
        AddItem(new Item("AthenaGlider:Umbrella_PaperParasol"));
        for (var i = 4; i < 26; i++)
        {
            var item = new Item($"AthenaGlider:Umbrella_Season_{i:D2}");
            item.BuildLimit = i;
            AddItem(item);
        }

        // From 14 Days of Fortnite
        AddItem(new Item("AthenaBackpack:BID_192_WinterHolidayFemale"));
        AddItem(new Item("AthenaItemWrap:Wrap_009_NewYears"));
        AddItem(new Item("AthenaLoadingScreen:LSID_094_HolidaySpecial"));

        // Free reward from the Season 5 Battle Pass
        AddItem(new Item("AthenaSkyDiveContrail:Trails_ID_011_Glitch"));

        // Free reward from the Season 7 Battle Pass
        AddItem(new Item("AthenaMusicPack:MusicPack_005_Disco"));

        AddItem(new Item("CosmeticLocker:CosmeticLocker_Athena"));
        SetAttribute("loadouts", new[]
        {
            "CosmeticLocker:cosmeticlocker_athena"
        });
    }
}