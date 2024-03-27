using ImGuiNET;
using static ImGuiNET.ImGui;

namespace RedHerring.Studio.UserInterface.Editor;

internal static class SceneFile
{
    private const string Extension = ".fbx";

    public static bool HasExtension(string extension)
    {
        return string.Compare(extension, Extension, StringComparison.OrdinalIgnoreCase) == 0;
    }

    public static void Draw(SceneDescription description)
    {
        BeginTable("Scene Properties", 2, ImGuiTableFlags.HighlightHoveredColumn);
        PropertyRow("Root Name", description.RootName);
        PropertyRow("Mesh Count", description.MeshCount);
        PropertyRow("Material Count", description.MaterialCount);
        EndTable();
    }

    private static void PropertyRow(string name, string value)
    {
        TableNextRow();
        TableNextColumn();
        Text(name);
        TableNextColumn();
        Text(value);
    }

    private static void PropertyRow(string name, int value)
    {
        TableNextRow();
        TableNextColumn();
        Text(name);
        TableNextColumn();
        Text(value.ToString());
    }
}