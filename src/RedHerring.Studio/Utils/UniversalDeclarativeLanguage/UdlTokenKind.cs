namespace UniversalDeclarativeLanguage;

internal enum UdlTokenKind
{
	Eof,
	Identifier,
	String,
	IntNumber,
	FloatNumber,
	Bool,
	OpenBrace,
	CloseBrace,
	Colon,
	EqualSign,
	Invalid,
}
