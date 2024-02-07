using System.Globalization;
using System.Text;

namespace UniversalDeclarativeLanguage;

internal sealed class UdlLexer
{
	private readonly Stream        _source;
	private          int           _column        = 1;
	private          int           _line          = 1;
	private readonly StringBuilder _stringBuilder = new();
	private          char          _readBuffer    = '\0';

	public int    Column         => _column;
	public int    Line           => _line;
	public string PositionString => $"line: {_line} column: {_column}";

	public UdlLexer(Stream source)
	{
		_source = source;
	}

	public UdlToken NextToken()
	{
		while (true)
		{
			if (!TryReadNextChar(out char ch))
			{
				return new UdlToken(UdlTokenKind.Eof);
			}

			switch (ch)
			{
				case '{':
					return new UdlToken(UdlTokenKind.OpenBrace);
				case '}':
					return new UdlToken(UdlTokenKind.CloseBrace);
				case ':':
					return new UdlToken(UdlTokenKind.Colon);
				case '=':
					return new UdlToken(UdlTokenKind.EqualSign);
				case '"':
					_stringBuilder.Clear();
					while (TryReadNextChar(out ch))
					{
						if (ch == '"')
						{
							return new UdlToken(UdlTokenKind.String, _stringBuilder.ToString());
						}
						_stringBuilder.Append(ch);
					}
					return new UdlToken(UdlTokenKind.Invalid);
				case '/':
					if (!TryReadNextChar(out ch))
					{
						return new UdlToken(UdlTokenKind.Invalid);
					}

					if (ch == '/')
					{
						// single line comment - skip to next line or eof
						while (TryReadNextChar(out ch))
						{
							if (ch == '\n')
							{
								break;
							}
						}
						continue;
					}

					if (ch == '*')
					{
						// multiline comment - skip to */
						bool wasAsterisk = false;
						while (TryReadNextChar(out ch))
						{
							if (ch == '*')
							{
								wasAsterisk = true;
								continue;
							}
							
							if (ch == '/' && wasAsterisk)
							{
								break;
							}
						
							wasAsterisk = false;
						}
						continue;
					}
					return new UdlToken(UdlTokenKind.Invalid);
			}

			if (char.IsLetter(ch))
			{
				_stringBuilder.Clear();
				_stringBuilder.Append(ch);
				while (TryReadNextChar(out ch))
				{
					if (char.IsLetter(ch) || char.IsNumber(ch) || ch == '_')
					{
						_stringBuilder.Append(ch);
						continue;
					}

					StoreToBuffer(ch);
					break;
				}

				string identifier = _stringBuilder.ToString();
				if (identifier == "true")
				{
					return new UdlToken(true);
				}

				if (identifier == "false")
				{
					return new UdlToken(false);
				}
				
				return new UdlToken(UdlTokenKind.Identifier, identifier);
			}

			if (char.IsNumber(ch) || ch == '-')
			{
				_stringBuilder.Clear();
				_stringBuilder.Append(ch);
				bool wasDot = false;
				while (TryReadNextChar(out ch))
				{
					if (char.IsNumber(ch))
					{
						_stringBuilder.Append(ch);
						continue;
					}

					if (ch == '.')
					{
						if (wasDot)
						{
							return new UdlToken(UdlTokenKind.Invalid);
						}

						wasDot = true;
						_stringBuilder.Append(ch);
						continue;
					}

					if (ch == '_' || char.IsLetter(ch))
					{
						return new UdlToken(UdlTokenKind.Invalid);
					}

					StoreToBuffer(ch);
					break;
				}

				if (wasDot)
				{
					if (float.TryParse(_stringBuilder.ToString(), NumberStyles.Number, CultureInfo.InvariantCulture, out float floatNumber))
					{
						return new UdlToken(floatNumber);
					}
					return new UdlToken(UdlTokenKind.Invalid);
				}

				if (int.TryParse(_stringBuilder.ToString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out int intNumber))
				{
					return new UdlToken(intNumber);
				}
				return new UdlToken(UdlTokenKind.Invalid);
			}
			
			// other characters are ignored by definition
		}
	}

	private bool TryReadNextChar(out char ch)
	{
		if (_readBuffer != '\0')
		{
			ch          = _readBuffer;
			_readBuffer = '\0';
			return true;
		}

		int b = _source.ReadByte();
		if (b == -1)
		{
			ch = '\0';
			return false;
		}

		ch = (char) b;
		if (ch == '\n')
		{
			++_line;
			_column = 1;
		}
		else
		{
			++_column;
		}

		return true;
	}

	private void StoreToBuffer(char ch)
	{
		_readBuffer = ch;
	}
}