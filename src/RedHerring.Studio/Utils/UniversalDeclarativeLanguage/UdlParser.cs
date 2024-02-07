using System.Text;

namespace UniversalDeclarativeLanguage;

public sealed class UdlParser
{
	private UdlLexer _lexer = null!;
	private UdlToken _token;

	public UdlNode? ParseUTF8(string source)
	{
		return Parse(new MemoryStream(Encoding.UTF8.GetBytes(source)));
	}

	public UdlNode? Parse(Stream source)
	{
		_lexer = new(source);

		_token = _lexer.NextToken();
		if (_token.Kind == UdlTokenKind.Eof)
		{
			return null;
		}

		UdlNode node = Node();
		if (_token.Kind != UdlTokenKind.Eof)
		{
			throw new UdlParserError($"End of file expected at {_lexer.PositionString}");
		}
		
		return node;
	}

	private UdlNode Node()
	{
		int            column     = _lexer.Column;
		int            line       = _lexer.Line;
		string         identifier = Identifier();
		string?        name       = Name();
		string?        type       = Type();
		object?        value      = Value();
		List<UdlNode>? block      = Block();

		return new UdlNode(identifier, name, type, value, block, column, line);
	}

	private string Identifier()
	{
		if (_token.Kind != UdlTokenKind.Identifier)
		{
			throw new UdlParserError($"Missing identifier at {_lexer.PositionString}");
		}
		
		string identifier = _token.Value!;
		_token = _lexer.NextToken();
		return identifier;
	}

	private string? Name()
	{
		if (_token.Kind != UdlTokenKind.String)
		{
			return null;
		}

		string name = _token.Value!;
		_token = _lexer.NextToken();
		return name;
	}

	private string? Type()
	{
		if (_token.Kind != UdlTokenKind.Colon)
		{
			return null;
		}

		_token = _lexer.NextToken();
		if (_token.Kind == UdlTokenKind.Eof)
		{
			throw new UdlParserError($"Missing type identifier at {_lexer.PositionString}");
		}

		string type = _token.Value!;
		_token = _lexer.NextToken();
		return type;
	}

	private object? Value()
	{
		if (_token.Kind != UdlTokenKind.EqualSign)
		{
			return null;
		}

		_token = _lexer.NextToken();
		if (_token.Kind == UdlTokenKind.Eof)
		{
			throw new UdlParserError($"Missing value at {_lexer.PositionString}");
		}

		object? value = _token.Kind switch
		{
			UdlTokenKind.String      => _token.Value,
			UdlTokenKind.IntNumber   => _token.IntValue,
			UdlTokenKind.FloatNumber => _token.FloatValue,
			UdlTokenKind.Bool        => _token.BoolValue,
			_ => throw new UdlParserError($"Invalid value at {_lexer.PositionString}")
		};

		_token = _lexer.NextToken();
		return value;
	}

	private List<UdlNode>? Block()
	{
		if (_token.Kind != UdlTokenKind.OpenBrace)
		{
			return null;
		}
		_token = _lexer.NextToken();

		if (_token.Kind == UdlTokenKind.Eof)
		{
			throw new UdlParserError($"Missing }} at {_lexer.PositionString}");
		}

		List<UdlNode> block = new();
		while (_token.Kind != UdlTokenKind.CloseBrace)
		{
			block.Add(Node());
		}
		_token = _lexer.NextToken();

		return block;
	}
}