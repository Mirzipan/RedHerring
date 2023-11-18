using System.Numerics;
using ImGuiNET;
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
}