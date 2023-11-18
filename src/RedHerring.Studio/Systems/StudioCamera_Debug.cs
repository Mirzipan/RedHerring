using System.Numerics;
using ImGuiNET;
using static ImGuiNET.ImGui;
using static RedHerring.Studio.Utils.ImGuiExtensions;

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

        Matrix("World", _worldMatrix);
        Matrix("View", _viewMatrix);
        Matrix("Projection", _projectionMatrix);
        
        End();
    }
}