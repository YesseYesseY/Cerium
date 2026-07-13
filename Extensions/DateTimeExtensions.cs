using System.Globalization;

namespace Cerium.Extensions;

public static class DateTimeExtensions
{
    public static string ToIsoString(this DateTime dt) => dt.ToString("yyyy-MM-dd'T'HH:mm:ss.fff'Z'");
}