using RedHerring.Numbers;

namespace RedHerring.Alexandria.Extensions;

public static partial class StringExtensions
{
    public static ReadOnlySpan<char> AsSpan(this string @this, Region region)
    {
        return @this.AsSpan(region.Start, region.Length);
    }
}