using System.Text;

namespace UniversalDeclarativeLanguage;

public sealed class UdlNode
{
	public readonly string  Identifier;
	public readonly string? Name;
	public readonly string? Type;

	public readonly object? Value;
	public          string? StringValue => Value as string;
	public          int     IntValue    => Value is int iValue ? iValue : 0;
	public          float   FloatValue  => Value is float fValue ? fValue : 0;
	public          bool    BoolValue   => Value is bool bValue && bValue;
	
	public readonly IReadOnlyList<UdlNode>? Children;

	public readonly int SourceColumn; // column in source code where this node begins
	public readonly int SourceLine;   // line in source code where this node begins

	public UdlNode(
		string                  identifier,
		string?                 name,
		string?                 type,
		object?                 value,
		IReadOnlyList<UdlNode>? children,
		int                     sourceColumn,
		int                     sourceLine
	)
	{
		Identifier   = identifier;
		Name         = name;
		Type         = type;
		Value        = value;
		Children     = children;
		SourceColumn = sourceColumn;
		SourceLine   = sourceLine;
	}

	// for debug purposes
	public override string ToString()
	{
		StringBuilder stringBuilder = new();

		stringBuilder.Append("Identifier:");
		stringBuilder.Append(Identifier);
		
		stringBuilder.Append(" Name:");
		stringBuilder.Append(Name);

		stringBuilder.Append(" Type:");
		stringBuilder.Append(Type);

		stringBuilder.Append(" Value:");
		stringBuilder.Append(Value);

		stringBuilder.Append(" Children:");
		if (Children == null)
		{
			stringBuilder.Append("null");
		}
		else
		{
			stringBuilder.Append('[');
			for(int i=0;i <Children.Count;++i)
			{
				if (i > 0)
				{
					stringBuilder.Append(',');
				}
				stringBuilder.Append(Children[i]);
			}
			stringBuilder.Append(']');
		}

		stringBuilder.Append('\n');
		return stringBuilder.ToString();
	}
}