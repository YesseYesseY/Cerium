namespace Cerium;

public class Utils
{
    public static string ConfigPath => Path.Join(AppDomain.CurrentDomain.BaseDirectory, "Configs");

    public static object? SetValue<T>(ref T field, T value)
    {
        if (field is null || field.Equals(value))
            return null;

        field = value;
        return field;
    }

    public object? SetValueArr<T>(ref T[] field, T value, int slot)
    {
        if (field[slot] is null || field[slot]!.Equals(value))
            return null;

        field[slot] = value;
        return field;
    }

    public static bool SetValueBool<T>(ref T field, T value)
    {
        return SetValue(ref field, value) is not null;
    }
}