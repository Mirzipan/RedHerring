using ImGuiNET;
using static ImGuiNET.ImGui;

namespace RedHerring.Studio.UserInterface.Editor;

internal static class SceneFile
{
    private const string Extension = ".fbx";
    
    private const string EmptyCell = "---";

    public static bool HasExtension(string extension)
    {
        return string.Compare(extension, Extension, StringComparison.OrdinalIgnoreCase) == 0;
    }

    public static void Draw(SceneDescription description)
    {
        BeginTable("Scene Properties", 2, ImGuiTableFlags.HighlightHoveredColumn);
        TableHeaderRow();

        PropertyRow("Root Name", description.RootName);
        CollapsiblePropertyRow("Meshes", description.MeshCount.ToString(), description.MeshNames);
        CollapsiblePropertyRow("Materials", description.MaterialCount.ToString(), description.MaterialNames);
        CollapsiblePropertyRow("Animations", description.AnimationCount.ToString(), description.AnimationNames);
        EndTable();
    }

    private static void TableHeaderRow()
    {
        TableNextColumn();
        TableHeader("Property");
        TableNextColumn();
        TableHeader("Value");
    }

    private static void CollapsiblePropertyRow(string name, string value, IList<string> children)
    {
        const ImGuiTreeNodeFlags nodeFlags =
            ImGuiTreeNodeFlags.OpenOnArrow |
            ImGuiTreeNodeFlags.OpenOnDoubleClick |
            ImGuiTreeNodeFlags.SpanFullWidth |
            ImGuiTreeNodeFlags.SpanAllColumns;

        const ImGuiTreeNodeFlags leafFlags =
            nodeFlags |
            ImGuiTreeNodeFlags.Leaf |
            ImGuiTreeNodeFlags.Bullet |
            ImGuiTreeNodeFlags.NoTreePushOnOpen;
        
        TableNextRow();
        TableNextColumn();
        
        if (children.Count == 0)
        {
            TreeNodeEx(name, leafFlags);
            TableNextColumn();
            Text(value);
            return;
        }
        
        bool open = TreeNodeEx(name, nodeFlags);
        
        TableNextColumn();
        Text(value);
        
        if (open) 
        {
            for (int i = 0; i < children.Count; i++)
            {
                string child = children[i];
                PropertyRow(child, EmptyCell);
            }
            
            TreePop();
        }
    }

    private static void PropertyRow(string name, string value)
    {
        TableNextRow();
        TableNextColumn();
        Text(name);
        TableNextColumn();
        Text(value);
    }
}