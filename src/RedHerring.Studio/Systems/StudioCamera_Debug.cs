using System.Numerics;
using ImGuiNET;
using static ImGuiNET.ImGui;

namespace RedHerring.Studio.Systems;

public sealed partial class StudioCamera
{
    private void DebugDraw()
    {
        bool open = false;

        SetNextWindowSize(new Vector2(300, 200), ImGuiCond.FirstUseEver);
        if (!Begin("Studio Camera Debug", ref open))
        {
            End();
            return;
        }

        Text($"Position: {_position}");
        Text($"Target: {_target}");
        Text($"Up: {_up}");
        Text($"Speed: {_movementSpeed}");

        Spacing();
        Separator();
        Spacing();

        Text($"FOV: {_fieldOfView}");
        Text($"Clip Plane Near: {_clipPlaneNear}");
        Text($"Clip Plane Far: {_clipPlaneFar}");

        Spacing();
        Separator();
        Spacing();

        DebugDrawMatrix("World", _worldMatrix);
        DebugDrawMatrix("View", _viewMatrix);
        DebugDrawMatrix("Projection", _projectionMatrix);

        End();
    }

    private void DebugDrawMatrix(string label, in Matrix4x4 matrix)
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
                Text(matrix[row, column].ToString("F4"));
            }
        }
        
        EndTable();
    }
}