using System.Numerics;
using ImGuiNET;
using RedHerring.Numbers;
using Gui = ImGuiNET.ImGui;

namespace RedHerring.Studio.UserInterface.Editor;

internal static class CSharpFile
{
    private static readonly Vector4 KeywordColor = Color.DeepPink.ToVector4();

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

    public static void Draw(IReadOnlyList<string> lines)
    {
        for (int i = 0; i < lines.Count; i++)
        {
            string line = lines[i];
            string[] parts = line.Split(' ');
            if (parts.Length == 0)
            {
                Gui.Text(line);
            }
            
            for (int j = 0; j < parts.Length; j++)
            {
                string part = parts[j].Trim();
                bool isColorPushed = false;
                if (Keywords.Contains(part))
                {
                    Gui.PushStyleColor(ImGuiCol.Text, KeywordColor);
                    isColorPushed = true;
                }
                
                Gui.Text(parts[j]);
                
                if (isColorPushed)
                {
                    Gui.PopStyleColor();
                }

                if (j < parts.Length - 1)
                {
                    Gui.SameLine();
                }
            }
        }
    }
}