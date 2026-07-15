using System.Text.Json.Serialization;

namespace Cerium;

public class Loadout
{
    [JsonInclude] public string Character = "";
    [JsonInclude] public string Backpack = "";
    [JsonInclude] public string Pickaxe = "AthenaPickaxe:DefaultPickaxe";
    [JsonInclude] public string Glider = "AthenaGlider:DefaultGlider";
    [JsonInclude] public string Contrail = "";
    [JsonInclude] public string LoadingScreen = "";
    [JsonInclude] public string MusicPack = "";
    [JsonInclude] public string[] Dance = ["AthenaDance:EID_DanceMoves", "", "", "", "", ""];
    [JsonInclude] public string[] ItemWrap = ["", "", "", "", "", "", ""];
    [JsonInclude] public string BannerColor = "DefaultColor1";
    [JsonInclude] public string BannerIcon = "StandardBanner1";

    // There's probably better ways to do this

    private static object? SetValue<T>(ref T field, T value)
    {
        if (field is null || field.Equals(value))
            return null;

        field = value;
        return field;
    }

    private object? SetValueArr<T>(ref T[] field, T value, int slot)
    {
        if (field[slot] is null || field[slot]!.Equals(value))
            return false;

        field[slot] = value;
        return true;
    }

    private static bool SetValueBool<T>(ref T field, T value)
    {
        return SetValue(ref field, value) is not null;
    }

    public bool SetBannerColor(string bannerColor) => SetValueBool(ref BannerColor, bannerColor);
    public bool SetBannerIcon(string bannerIcon) => SetValueBool(ref BannerIcon, bannerIcon);
    public object? SetCharacter(string character) => SetValue(ref Character, character);
    public object? SetPickaxe(string pickaxe) => SetValue(ref Pickaxe, pickaxe);
    public object? SetBackpack(string backpack) => SetValue(ref Backpack, backpack);
    public object? SetGlider(string glider) => SetValue(ref Glider, glider);
    public object? SetContrail(string contrail) => SetValue(ref Contrail, contrail);
    public object? SetLoadingScreen(string loadingScreen) => SetValue(ref LoadingScreen, loadingScreen);
    public object? SetMusicPack(string musicPack) => SetValue(ref MusicPack, musicPack);

    public object? SetDance(string dance, int slot)
    {
        if (Dance[slot] == dance)
            return null;

        Dance[slot] = dance;
        return Dance;
    }

    public object? SetItemWrap(string itemWrap, int slot)
    {
        if (ItemWrap[slot] == itemWrap)
            return null;

        ItemWrap[slot] = itemWrap;
        return ItemWrap;
    }
}