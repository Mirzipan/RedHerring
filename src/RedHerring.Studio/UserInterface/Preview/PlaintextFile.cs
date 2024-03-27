using static ImGuiNET.ImGui;

namespace RedHerring.Studio.UserInterface.Editor;

internal static class PlaintextFile
{
    public static void Draw(IReadOnlyList<string> lines)
    {
        for (int i = 0; i < lines.Count; i++)
        {
            string line = lines[i];
            Text(line);
        }
    }
}