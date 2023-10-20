using System.Runtime.InteropServices;

namespace RedHerring.Numbers;

[StructLayout(LayoutKind.Sequential)]
public record struct Region(int Start, int Length)
{
    public int Start = Start;
    public int Length = Length;

    public bool IsEmpty => Length == 0;
    public bool IsValid => Start >= 0 && Length > 0;

    public int Next => Start + Length;
    public int End => Start + Length - 1;
}