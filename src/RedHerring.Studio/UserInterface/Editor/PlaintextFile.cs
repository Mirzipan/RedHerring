namespace RedHerring.Studio.UserInterface.Editor;
using Gui = ImGuiNET.ImGui;

internal static class PlaintextFile
{
    public static void Draw(IReadOnlyList<string> lines)
    {
        for (int i = 1; i < lines.Count; i++)
        {
            string line = lines[i];
            Gui.Text(line);
        }
    }
}