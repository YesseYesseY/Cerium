using System.Text.Json.Serialization;

namespace Cerium;

public class Loadout
{
    public string Character { get; set; } = "";
    public string Backpack { get; set; } = "";
    public string Pickaxe { get; set; } = "";
    public string Glider { get; set; } = "";
    public string SkyDiveContrail { get; set; } = "";
    public string LoadingScreen { get; set; } = "";
    public string MusicPack { get; set; } = "";
    public string[] Dance { get; set; } = Enumerable.Repeat("", 6).ToArray();
    public string[] ItemWrap { get; set; } = Enumerable.Repeat("", 8).ToArray();
    public string BannerIcon { get; set; } = "StandardBanner1";
    public string BannerColor { get; set; } = "DefaultColor1";

    public Loadout()
    {
    }

    public Loadout(Loadout other)
    {
        CopyFrom(other);
    }

    public void CopyFrom(Loadout other)
    {
        Character = other.Character;
        Backpack = other.Backpack;
        Pickaxe = other.Pickaxe;
        Glider = other.Glider;
        SkyDiveContrail = other.SkyDiveContrail;
        LoadingScreen = other.LoadingScreen;
        MusicPack = other.MusicPack;
        other.Dance.CopyTo(Dance);
        other.ItemWrap.CopyTo(ItemWrap);
        BannerIcon = other.BannerIcon;
        BannerColor = other.BannerColor;
    }

    public object? SetValue(string propName, object value, int index = 0)
    {
        var property = GetType().GetProperty(propName);
        if (property is null)
            return null;

        var val = property.GetValue(this);
        if (val is null)
            return null;

        if (property.PropertyType.IsArray)
        {
            var arr = (object[])val;
            if (index == -1)
            {
                for (var i = 0; i < arr.Length; i++)
                    arr[i] = value;
            }
            else
            {
                if (arr[index] == value)
                    return null;

                arr[index] = value;
            }

            return arr;
        }

        if (val == value)
            return null;

        property.SetValue(this, value);
        return value;
    }
}

public class LoadoutItemSlot
{
    [JsonPropertyName("items")] public string[] Items { get; init; }

    public LoadoutItemSlot(params string[] items)
    {
        Items = items;
    }
}

public class LoadoutItem() : Item("athena", "CosmeticLocker:CosmeticLocker_Athena", 1)
{
    public Loadout ItemLoadout { get; set; } = new();
    public string LoadoutName { get; set; } = "";

    public LoadoutItem(string loadoutName) : this()
    {
        LoadoutName = loadoutName;
    }

    public LoadoutItem(string loadoutName, LoadoutItem loadoutItem) : this(loadoutName)
    {
        ItemLoadout = new Loadout(loadoutItem.ItemLoadout);
        LoadoutName = loadoutName;
    }

    public object GetLockerSlotsData()
    {
        return new
        {
            slots = new
            {
                Character = new LoadoutItemSlot(ItemLoadout.Character),
                Backpack = new LoadoutItemSlot(ItemLoadout.Backpack),
                Pickaxe = new LoadoutItemSlot(ItemLoadout.Pickaxe),
                Glider = new LoadoutItemSlot(ItemLoadout.Glider),
                SkyDiveContrail = new LoadoutItemSlot(ItemLoadout.SkyDiveContrail),
                LoadingScreen = new LoadoutItemSlot(ItemLoadout.LoadingScreen),
                MusicPack = new LoadoutItemSlot(ItemLoadout.MusicPack),
                ItemWrap = new LoadoutItemSlot(ItemLoadout.ItemWrap),
                Dance = new LoadoutItemSlot(ItemLoadout.Dance),
            }
        };
    }

    public override object Attributes(Account account)
    {
        return new
        {
            locker_name = LoadoutName,
            banner_icon_template = ItemLoadout.BannerIcon,
            banner_color_template = ItemLoadout.BannerColor,
            use_count = 1,
            locker_slots_data = GetLockerSlotsData()
        };
    }
}