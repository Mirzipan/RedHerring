using System.Numerics;
using ImGuiNET;
using RedHerring.Fingerprint;
using RedHerring.Infusion.Attributes;
using RedHerring.Studio.Constants;
using static ImGuiNET.ImGui;
using static RedHerring.Studio.Utils.ImGuiExtensions;

namespace RedHerring.Studio.Systems;

public sealed class ToolDebug_StudioCamera
{
    private static readonly string[] Actions = 
    { 
        InputAction.MoveForward,
        InputAction.MoveBackward,
        InputAction.MoveLeft,
        InputAction.MoveRight,
        InputAction.MoveUp,
        InputAction.MoveDown,
        InputAction.MoveSpeedIncrease,
        InputAction.MoveSpeedDecrease,
    };
    
    [Infuse]
    private Input _input = null!;

    private readonly StudioCamera _camera;

    public ToolDebug_StudioCamera(StudioCamera camera)
    {
        _camera = camera;
    }

    public void Draw()
    {
        bool open = false;

        SetNextWindowSize(new Vector2(300, 200), ImGuiCond.FirstUseEver);
        if (!Begin("Studio Camera Debug", ref open))
        {
            End();
            return;
        }

        PrintBindings();

        Spacing();
        Separator();
        Spacing();

        Text($"Position: {_camera.Position}");
        Text($"Target: {_camera.Target}");
        Text($"Up: {_camera.Up}");
        Text($"Speed: {_camera.MovementSpeed}");

        Spacing();
        Separator();
        Spacing();

        Text($"FOV: {_camera.FieldOfView}");
        Text($"Clip Plane Near: {_camera.ClipPlaneNear}");
        Text($"Clip Plane Far: {_camera.ClipPlaneFar}");

        Spacing();
        Separator();
        Spacing();

        Matrix("World", _camera.WorldMatrix);
        Matrix("View", _camera.ViewMatrix);
        Matrix("Projection", _camera.ProjectionMatrix);
        
        End();
    }

    private void PrintBindings()
    {
        if (_input.Bindings is null)
        {
            return;
        }
        
        InputBindings("Camera Controls", _input.Bindings!, Actions);
    }
}