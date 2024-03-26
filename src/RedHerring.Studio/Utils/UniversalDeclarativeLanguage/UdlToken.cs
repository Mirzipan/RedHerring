using System.Globalization;

namespace UniversalDeclarativeLanguage;

internal readonly struct UdlToken
{
	public readonly UdlTokenKind Kind = UdlTokenKind.Invalid;
	
	public readonly string? Value = null;
	public readonly int     IntValue;
	public readonly float   FloatValue;
	public readonly bool    BoolValue;

	public UdlToken(UdlTokenKind kind)
	{
		Kind = kind;
	}

	public UdlToken(UdlTokenKind kind, string value)
	{
		Kind  = kind;
		Value = value;
	}

	public UdlToken(int value)
	{
		Kind       = UdlTokenKind.IntNumber;
		Value      = value.ToString();
		IntValue   = value;
		FloatValue = value;
	}

	public UdlToken(float value)
	{
		Kind       = UdlTokenKind.FloatNumber;
		Value      = value.ToString(CultureInfo.InvariantCulture);
		IntValue   = (int)value;
		FloatValue = value;
	}

	public UdlToken(bool value)
	{
		Kind      = UdlTokenKind.Bool;
		BoolValue = value;
	}
}