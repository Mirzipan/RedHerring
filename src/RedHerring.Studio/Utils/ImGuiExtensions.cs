using System.Numerics;
using ImGuiNET;
using RedHerring.Inputs;
using static ImGuiNET.ImGui;

namespace RedHerring.Studio.Utils;

public static class ImGuiExtensions
{
    private const string MatrixFloatFormat = "F4";

    public static void Matrix(string label, in Matrix4x4 matrix)
    {
        if (!CollapsingHeader(label, ImGuiTreeNodeFlags.DefaultOpen))
        {
            return;
        }
        
        if (!BeginTable(label, 4, ImGuiTableFlags.RowBg | ImGuiTableFlags.Borders))
        {
            EndTable();
            return;
        }

        for (int row = 0; row < 4; row++)
        {
            TableNextRow();
            
            for (int column = 0; column < 4; column++)
            {
                TableNextColumn();
                Text(matrix[row, column].ToString(MatrixFloatFormat));
            }
        }
        
        EndTable();
    }

    public static void InputBindings(string label, ShortcutBindings bindings, params string[] actions)
    {
        if (!CollapsingHeader(label))
        {
            return;
        }
        
        if (!BeginTable(label, 2, ImGuiTableFlags.RowBg | ImGuiTableFlags.Borders))
        {
            EndTable();
            return;
        }

        TableSetupColumn("Action", ImGuiTableColumnFlags.WidthFixed);
        TableSetupColumn("Shortcut");
        TableHeadersRow();
        
        for (int i = 0; i < actions.Length; i++)
        {
            TableNextRow();
            InputBinding(bindings, actions[i]);
        }
        
        EndTable();
    }

    private static void InputBinding(ShortcutBindings bindings, string action)
    {
        TableNextColumn();
        Text(action);
        
        TableNextColumn();
        
        var shortcut = bindings.PrimaryShortcut(action);
        if (shortcut is null || shortcut.Shortcut.Positive == Input.Unknown)
        {
            Text("<unmapped>");
            return;
        }

        Text(shortcut.Shortcut.ToString());
    }
}