using System.Runtime.CompilerServices;
using System.Text;

namespace RedHerring.Alexandria.Extensions;

public static partial class StringExtensions
{
    #region Queries
        
    /// <summary>
    /// Returns true if this <see cref="string"/> is not null or empty.
    /// </summary>
    /// <param name="this"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool NotNullOrEmpty(this string @this) => !string.IsNullOrEmpty(@this);

    /// <summary>
    /// Returns true if this <see cref="string"/> is null or whitespace.
    /// </summary>
    /// <param name="this"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNullOrWhitespace(this string @this) => string.IsNullOrWhiteSpace(@this);
        
    /// <summary>
    /// Returns true if this <see cref="string"/> is not null or whitespace.
    /// </summary>
    /// <param name="this"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool NotNullOrWhitespace(this string @this) => !string.IsNullOrWhiteSpace(@this);

    /// <summary>
    /// Returns this if this <see cref="string"/> is not null or empty, otherwise default value is returned.
    /// </summary>
    /// <param name="this"></param>
    /// <param name="default"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ValueOrDefault(this string @this, string @default) => !string.IsNullOrEmpty(@this) ? @this : @default;

    /// <summary>
    /// Returns null if this <see cref="string"/> is null or empty.
    /// </summary>
    /// <param name="this"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string NullIfEmpty(this string @this) => !string.IsNullOrEmpty(@this) ? @this : null;

    /// <summary>
    /// Returns empty <see cref="string"/> if this is null or empty.
    /// </summary>
    /// <param name="this"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string EmptyIfNull(this string @this) => !string.IsNullOrEmpty(@this) ? @this : string.Empty;

    #endregion Queries

    #region Manipulation

    public static string PrettyCamelCase(this string @this)
    {
        if (@this.Length <= 1)
        {
            return @this;
        }
        
        var sb = new StringBuilder();
        int index = 0;
        while (index < @this.Length && @this[index] == '_')
        {
            index += 1;
        }
        
        if (index == @this.Length - 1)
        {
            goto build;
        }
        
        sb.Append(char.ToUpper(@this[index]));
        if (index == @this.Length - 1)
        {
            goto build;
        }

        index += 1;
        for (;index < @this.Length; index++)
        {
            char prev = @this[index - 1];
            char curr = @this[index];
            if ((char.IsUpper(curr) || char.IsNumber(curr)) && !char.IsUpper(prev) && !char.IsNumber(prev))
            {
                sb.Append(' ');
            }

            sb.Append(curr);
        }

        build:
        return sb.ToString();
    }

    #endregion Manipulation
}