using RedHerring.Numbers;

namespace RedHerring.Alexandria.Extensions;

public static partial class StringExtensions
{
    public static string? Substring(this string @this, Region region)
    {
        return region.IsValid ? @this.Substring(region.Start, region.Length) : null;
    }
    
    public static ReadOnlySpan<char> AsSpan(this string @this, Region region)
    {
        return @this.AsSpan(region.Start, region.Length);
    }
    
    public static ReadOnlySpan<char> AsSpanIfValid(this string @this, Region region)
    {
        return region.IsValid ? @this.AsSpan(region.Start, region.Length) : null;
    }
}