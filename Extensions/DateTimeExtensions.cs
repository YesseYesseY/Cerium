using System.Globalization;

namespace Cerium.Extensions;

public static class DateTimeExtensions
{
    public static string ToIsoString(this DateTime dt) => dt.ToString("o", CultureInfo.InvariantCulture);
}