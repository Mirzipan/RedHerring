namespace UniversalDeclarativeLanguage;

public sealed class UdlParserError(string ParserMessage) : Exception
{
	public override string ToString()
	{
		return $"{ParserMessage}\n{base.ToString()}";
	}
}
