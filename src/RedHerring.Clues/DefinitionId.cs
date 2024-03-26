using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace RedHerring.Clues;

[StructLayout(LayoutKind.Explicit)]
[Serializable]
public readonly struct DefinitionId : IComparable, IComparable<DefinitionId>, IEquatable<DefinitionId>
{
    private const int CharacterCount = 8;
    private const char PaddingCharacter = '_';
    
    public static readonly DefinitionId Empty;
    
    [FieldOffset(00)]
    private readonly long _pv;
    [FieldOffset(08)]
    private readonly long _sv;
    
    [FieldOffset(00)]
    [NonSerialized]
    private readonly char _a;
    [FieldOffset(02)]
    [NonSerialized]
    private readonly char _b;
    [FieldOffset(04)]
    [NonSerialized]
    private readonly char _c;
    [FieldOffset(06)]
    [NonSerialized]
    private readonly char _d;
    [FieldOffset(08)]
    [NonSerialized]
    private readonly char _e;
    [FieldOffset(10)]
    [NonSerialized]
    private readonly char _f;
    [FieldOffset(12)]
    [NonSerialized]
    private readonly char _g;
    [FieldOffset(14)]
    [NonSerialized]
    private readonly char _h;
    
    public bool IsEmpty => _pv == 0 && _sv == 0;

    public DefinitionId(long primary, long secondary)
    {
        _pv = primary;
        _sv = secondary;
    }

    public DefinitionId(char a, char b, char c, char d, char e, char f, char g, char h)
    {
        _a = a;
        _b = b;
        _c = c;
        _d = d;
        _e = e;
        _f = f;
        _g = g;
        _h = h;
    }

    public DefinitionId(char[] characters)
    {
        _a = characters.Length > 0 ? characters[0] : PaddingCharacter;
        _b = characters.Length > 1 ? characters[1] : PaddingCharacter;
        _c = characters.Length > 2 ? characters[2] : PaddingCharacter;
        _d = characters.Length > 3 ? characters[3] : PaddingCharacter;
        _e = characters.Length > 4 ? characters[4] : PaddingCharacter;
        _f = characters.Length > 5 ? characters[5] : PaddingCharacter;
        _g = characters.Length > 6 ? characters[6] : PaddingCharacter;
        _h = characters.Length > 7 ? characters[7] : PaddingCharacter;
    }

    public DefinitionId(Span<char> characters)
    {
        _a = characters.Length > 0 ? characters[0] : PaddingCharacter;
        _b = characters.Length > 1 ? characters[1] : PaddingCharacter;
        _c = characters.Length > 2 ? characters[2] : PaddingCharacter;
        _d = characters.Length > 3 ? characters[3] : PaddingCharacter;
        _e = characters.Length > 4 ? characters[4] : PaddingCharacter;
        _f = characters.Length > 5 ? characters[5] : PaddingCharacter;
        _g = characters.Length > 6 ? characters[6] : PaddingCharacter;
        _h = characters.Length > 7 ? characters[7] : PaddingCharacter;
    }

    public DefinitionId(ReadOnlySpan<char> characters)
    {
        _a = characters.Length > 0 ? characters[0] : PaddingCharacter;
        _b = characters.Length > 1 ? characters[1] : PaddingCharacter;
        _c = characters.Length > 2 ? characters[2] : PaddingCharacter;
        _d = characters.Length > 3 ? characters[3] : PaddingCharacter;
        _e = characters.Length > 4 ? characters[4] : PaddingCharacter;
        _f = characters.Length > 5 ? characters[5] : PaddingCharacter;
        _g = characters.Length > 6 ? characters[6] : PaddingCharacter;
        _h = characters.Length > 7 ? characters[7] : PaddingCharacter;
    }

    public DefinitionId(string id)
    {
        if (id.Length < CharacterCount)
        {
            id = id.PadRight(CharacterCount, PaddingCharacter);
        }
        
        _a = id[0];
        _b = id[1];
        _c = id[2];
        _d = id[3];
        _e = id[4];
        _f = id[5];
        _g = id[6];
        _h = id[7];
    }

    public override string ToString()
    {
        return $"{_a}{_b}{_c}{_d}{_e}{_f}{_g}{_h}";
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_pv, _sv);
    }

    public int CompareTo(object? value)
    {
        if (value is null)
        {
            return 1;
        }
        
        if (value is not DefinitionId)
        {
            return 1;
        }
        
        DefinitionId id = (DefinitionId)value;

        if (id._pv != _pv)
        {
            return GetResult(_pv, id._pv);
        }

        if (id._sv != _sv)
        {
            return GetResult(_sv, id._sv);
        }

        return 0;
    }

    public int CompareTo(DefinitionId other)
    {
        if (other._pv != _pv)
        {
            return GetResult(_pv, other._pv);
        }

        if (other._sv != _sv)
        {
            return GetResult(_sv, other._sv);
        }

        return 0;
    }
    
    public override bool Equals(object? obj) => obj is DefinitionId other && Equals(this, other);

    public bool Equals(DefinitionId other) => Equals(this, other);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool Equals(DefinitionId left, DefinitionId right) => left._pv == right._pv && left._sv == right._sv;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int GetResult(long @this, long other) => @this < other ? -1 : 1;

    public static bool operator ==(DefinitionId left, DefinitionId right) => Equals(left, right);

    public static bool operator !=(DefinitionId left, DefinitionId right) => !Equals(left, right);
}