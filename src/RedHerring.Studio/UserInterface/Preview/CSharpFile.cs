using System.Numerics;
using ImGuiNET;
using RedHerring.Numbers;
using static ImGuiNET.ImGui;

namespace RedHerring.Studio.UserInterface.Editor;

internal static class CSharpFile
{
    private const string Extension = ".cs";
    
    private static readonly Vector4 KeywordColor = Color.DeepPink.ToVector4();
    private static readonly Vector4 CommentColor = Color.Gray.ToVector4();

    private const string SingleLineComment = @"//";
    private const string MultilineCommentStart = @"/*";
    private const string MultilineCommentEnd = @"*/";

    private static readonly List<string> Keywords = new()
    {
        "abstract",
        "as",
        "base",
        "bool",
        "break",
        "byte",
        "case",
        "catch",
        "char",
        "checked",
        "class",
        "const",
        "continue",
        "decimal",
        "default",
        "delegate",
        "do",
        "double",
        "else",
        "enum",
        "event",
        "explicit",
        "extern",
        "false",
        "finally",
        "fixed",
        "float",
        "for",
        "foreach",
        "goto",
        "if",
        "implicit",
        "in",
        "int",
        "interface",
        "internal",
        "is",
        "lock",
        "long",
        "namespace",
        "new",
        "null",
        "object",
        "operator",
        "out",
        "override",
        "params",
        "private",
        "protected",
        "public",
        "readonly",
        "ref",
        "return",
        "sbyte",
        "sealed",
        "short",
        "sizeof",
        "stackalloc",
        "static",
        "string",
        "struct",
        "switch",
        "this",
        "throw",
        "true",
        "try",
        "typeof",
        "uint",
        "ulong",
        "unchecked",
        "unsafe",
        "ushort",
        "using",
        "virtual",
        "void",
        "volatile",
        "while",
    };

    public static bool HasExtension(string extension) => string.Compare(extension, Extension, StringComparison.OrdinalIgnoreCase) == 0;

    public static void Draw(IReadOnlyList<string> lines)
    {
        // TODO(mirzi): push some monospace font here
        
        bool singleLineComment = false;
        bool multilineComment = false;

        int lineNumberWidth = lines.Count switch
        {
            >= 1000 => 4,
            >= 100 => 3,
            >= 10 => 2,
            >= 1 => 1,
            _ => 1
        };

        for (int i = 0; i < lines.Count; i++)
        {
            DrawLineNumber(i, lineNumberWidth);

            string line = lines[i];
            string[] parts = line.Split(' ');
            if (parts.Length == 0)
            {
                Text(line);
            }
            
            singleLineComment = false;
            
            for (int j = 0; j < parts.Length; j++)
            {
                string part = parts[j].Trim();
                bool isColorPushed = false;
                if (part.StartsWith(SingleLineComment) || singleLineComment || multilineComment)
                {
                    PushStyleColor(ImGuiCol.Text, CommentColor);
                    isColorPushed = true;
                    singleLineComment = true;
                }
                else if (part.StartsWith(MultilineCommentStart))
                {
                    PushStyleColor(ImGuiCol.Text, CommentColor);
                    isColorPushed = true;
                    multilineComment = true;
                }
                else if (part.StartsWith(MultilineCommentEnd))
                {
                    PushStyleColor(ImGuiCol.Text, CommentColor);
                    isColorPushed = true;
                    multilineComment = false;
                }
                else if (Keywords.Contains(part))
                {
                    PushStyleColor(ImGuiCol.Text, KeywordColor);
                    isColorPushed = true;
                }
                
                Text(parts[j]);
                
                if (isColorPushed)
                {
                    PopStyleColor();
                }

                if (j < parts.Length - 1)
                {
                    SameLine();
                }
            }
        }
    }

    private static void DrawLineNumber(int i, int lineNumberWidth)
    {
        PushStyleColor(ImGuiCol.Text, CommentColor);
        
        string lineNumber = (i + 1).ToString().PadLeft(lineNumberWidth, ' ');
        Text(lineNumber);
        SameLine();
        
        PopStyleColor();
    }
}