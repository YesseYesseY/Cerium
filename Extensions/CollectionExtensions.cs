using System.Collections;

namespace Cerium.Extensions;

public static class EnumerableExtensions
{
    public static bool IsValidIndex(this ICollection collection, int index)
    {
        return index >= 0 && index < collection.Count;
    }
}