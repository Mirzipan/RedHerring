using System.Numerics;
using ImGuiNET;
using RedHerring.Alexandria.Extensions;
using RedHerring.Numbers;
using RedHerring.Studio.UserInterface.Editor.Markdown;
using Gui = ImGuiNET.ImGui;

namespace RedHerring.Studio.UserInterface.Editor;

internal static class MarkdownFile
{
    private static readonly Vector4[] HeadingColors = new[]
    {
        Color.OrangeRed.ToVector4(),
        Color.Yellow.ToVector4(),
        Color.GreenYellow.ToVector4(),
        Color.LightCyan.ToVector4(),
        Color.SkyBlue.ToVector4(),
        Color.PaleVioletRed.ToVector4(),
    };
    
    public static void Draw(IReadOnlyList<string> lines)
    {
        Line lineMeta = default;
        ResetLine(ref lineMeta);
        
        for (int i = 0; i < lines.Count; i++)
        {
            string line = lines[i];
            for (int ci = 0; ci < line.Length; ci++)
            {
                char c = line[ci];
                if (lineMeta.IsLeadingSpace)
                {
                    if (c == ' ')
                    {
                        lineMeta.LeadingSpaceCount += 1;
                        continue;
                    }

                    lineMeta.IsLeadingSpace = false;
                    lineMeta.LastRenderPosition = ci - 1;
                    if (c == '#')
                    {
                        lineMeta.HeadingCount += 1;
                        bool continueChecking = true;
                        int j = ci;
                        while (++j < line.Length && continueChecking)
                        {
                            c = line[j];
                            switch (c)
                            {
                                case '#':
                                    lineMeta.HeadingCount += 1;
                                    break;
                                case ' ':
                                    lineMeta.LastRenderPosition = j - 1;
                                    ci = j;
                                    lineMeta.IsHeading = true;
                                    continueChecking = false;
                                    break;
                                default:
                                    lineMeta.IsHeading = false;
                                    continueChecking = false;
                                    break;
                            }
                        }

                        if (lineMeta.IsHeading)
                        {
                            continue;
                        }
                    }
                }

                if (!lineMeta.IsHeading && ci == line.Length - 1)
                {
                    lineMeta.LineEnd = ci;

                    lineMeta.LineStart = 0;
                    lineMeta.LastRenderPosition = ci;
                }
            }
            
            DrawLine(line, lineMeta);

            ResetLine(ref lineMeta);
        }
    }

    private static void DrawLine(string line, Line lineMeta)
    {
        int indentStart = 0;
        if (lineMeta.IsUnorderedListStart)
        {
            indentStart = 1;
        }

        for (int i = indentStart; i < lineMeta.LeadingSpaceCount / 2; ++i)
        {
            Gui.Indent();
        }
        
        int textStart = lineMeta.LastRenderPosition + 1;

        if (lineMeta.IsUnorderedListStart)
        {
            // TODO(mirzi): magic
            Gui.TextWrapped(line.AsSpan(textStart));
        }
        else if (lineMeta.IsHeading)
        {
            int index = (lineMeta.HeadingCount - 1).Clamp(0, HeadingColors.Length - 1);
            Gui.PushStyleColor(ImGuiCol.Text, HeadingColors[index]);
            
            Gui.TextWrapped(line.AsSpan(textStart));
            
            Gui.PopStyleColor();
        }
        else if (lineMeta.IsEmphasis)
        {
            Gui.TextWrapped(line.AsSpan(textStart));
        }
        else
        {
            Gui.TextWrapped(line);
        }
        
        for (int i = indentStart; i < lineMeta.LeadingSpaceCount / 2; ++i)
        {
            Gui.Unindent();
        }
    }

    private static void ResetLine(ref Line line)
    {
        line = default;
        line.IsLeadingSpace = true;
    } 
}