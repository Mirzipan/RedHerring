using System.Numerics;
using ImGuiNET;
using RedHerring.Alexandria;
using RedHerring.Alexandria.Identifiers;
using RedHerring.Core;
using RedHerring.Fingerprint;
using RedHerring.Fingerprint.Layers;
using RedHerring.Infusion.Attributes;
using RedHerring.Numbers;
using RedHerring.Render;
using RedHerring.Studio.Constants;
using Gui = ImGuiNET.ImGui;

namespace RedHerring.Studio.Systems;

public sealed class StudioCamera : EngineSystem, Drawable
{
    public const float FieldOfViewMin = MathF.PI / 4;
    public const float ClipPlaneNearMin = 0.01f;
    public const float ClipPlaneFarDefault = 1000f;

    public const float MovementSpeedMin = 0.01f;
    public const float MovementSpeedMax = 10f;
    public const float MovementSpeedDefault = 0.01f;

    [Infuse]
    private Renderer _renderer = null!;
    [Infuse]
    private InputReceiver _receiver = null!;

    private Matrix4x4 _worldMatrix;
    private Matrix4x4 _viewMatrix;
    private Matrix4x4 _projectionMatrix;

    private Vector3 _position;
    private Vector3 _target;
    private Vector3 _up;
    
    private float _fieldOfView;
    private float _clipPlaneNear;
    private float _clipPlaneFar;
    private float _movementSpeed;

    private bool _isDirty;
    private bool _isActive;

    public Vector3 Position
    {
        get => _position;
        set
        {
            _isDirty = true;
            _position = value;
        }
    }

    public Vector3 Target
    {
        get => _target;
        set
        {
            _isDirty = true;
            _target = value;
        }
    }

    public Vector3 Up
    {
        get => _up;
        set
        {
            _isDirty = true;
            _up = value;
        }
    }

    public Matrix4x4 ViewMatrix
    {
        get 
        {
            if (_isDirty)
            {
                CalculateMatrices();
            }

            return _viewMatrix;
        }
    }
    
    public Matrix4x4 ProjectionMatrix
    {
        get
        {
            if (_isDirty)
            {
                CalculateMatrices();
            }

            return _projectionMatrix;
        }
    }
    
    public float FieldOfView => _fieldOfView;
    public float ClipPlaneNear => _clipPlaneNear;
    public float ClipPlaneFar => _clipPlaneFar;
    public float MovementSpeed => _movementSpeed;
    public bool IsActive => _isActive;

    public bool IsVisible => true;
    public int DrawOrder => 100;

    #region Lifecycle

    protected override void Init()
    {
        SetupValues();
        SetupInput();
    }

    protected override ValueTask<int> Load()
    {
        Activate();
        return ValueTask.FromResult(0);
    }


    protected override ValueTask<int> Unload()
    {
        Deactivate();
        return ValueTask.FromResult(0);
    }

    public bool BeginDraw() => _isActive;

    public void Draw(GameTime gameTime)
    {
        SubmitToRenderer();
        DebugDraw();
    }

    public void EndDraw()
    {
    }

    public void Activate()
    {
        _isActive = true;
        _receiver.Push();
    }

    public void Deactivate()
    {
        _isActive = false;
        _receiver.Pop();
    }

    #endregion Lifecycle

    #region Public

    public void SetFieldOfView(float fov)
    {
        _fieldOfView = float.Clamp(fov, FieldOfViewMin, MathF.PI);
        _isDirty = true;
    }

    public void SetClipPlanes(float near, float far)
    {
        _clipPlaneNear = MathF.Max(near, ClipPlaneNearMin);
        _clipPlaneFar = MathF.Max(far, near + 0.01f);
        _isDirty = true;
    }

    public void SetMovementSpeed(float value)
    {
        _movementSpeed = float.Clamp(value, MovementSpeedMin, MovementSpeedMax);
    }

    #endregion Public

    #region Private

    private void SetupValues()
    {
        _position = Vector3.Zero;
        _target = Vector3Utils.Forward * 100f;
        _up = Vector3Utils.Up;
        
        _viewMatrix = Matrix4x4.Identity;
        _projectionMatrix = Matrix4x4.Identity;
        
        SetFieldOfView(FieldOfViewMin);
        SetClipPlanes(ClipPlaneNearMin, ClipPlaneFarDefault);
        SetMovementSpeed(MovementSpeedDefault);

        _isDirty = true;
    }

    private void SetupInput()
    {
        _receiver.Name = "studio camera";
        _receiver.Layer = new OctoByte("camera");
        _receiver.ConsumesAllInput = false;
        
        _receiver.Bind(InputAction.MoveLeft, InputState.Down, OnMoveLeft);
        _receiver.Bind(InputAction.MoveRight, InputState.Down, OnMoveRight);
        _receiver.Bind(InputAction.MoveUp, InputState.Down, OnMoveUp);
        _receiver.Bind(InputAction.MoveDown, InputState.Down, OnMoveDown);
        _receiver.Bind(InputAction.MoveForward, InputState.Down, OnMoveForward);
        _receiver.Bind(InputAction.MoveBackward, InputState.Down, OnMoveBackward);
        
        _receiver.Bind(InputAction.MoveSpeedIncrease, InputState.Down, OnMoveSpeedIncrease);
        _receiver.Bind(InputAction.MoveSpeedDecrease, InputState.Down, OnMoveSpeedDecrease);
    }

    private void SubmitToRenderer()
    {
        if (_isDirty)
        {
            CalculateMatrices();
        }
        
        _renderer.SetCameraViewMatrix(_worldMatrix, _viewMatrix, _projectionMatrix, FieldOfView, ClipPlaneNear, ClipPlaneFar);
    }

    private void CalculateMatrices()
    {
        var size = Context.View.Size;
        float aspectRatio = 16f / 9;
        if (size.Y != 0)
        {
            aspectRatio = (float)size.X / size.Y;
        }

        _worldMatrix = Matrix4x4.CreateLookAt(_position, _target, _up);
        Matrix4x4.Invert(_worldMatrix, out _viewMatrix);
        _projectionMatrix = Matrix4x4.CreatePerspectiveFieldOfView(FieldOfView, aspectRatio, ClipPlaneNear, ClipPlaneFar);
        
        _isDirty = false;
    }

    private void DebugDraw()
    {
        bool open = false;
        
        Gui.SetNextWindowSize(new Vector2(300, 200), ImGuiCond.FirstUseEver);
        if (!Gui.Begin("Studio Camera Debug", ref open))
        {
            Gui.End();
            return;
        }
        
        Gui.Text($"Position: {_position}");
        Gui.Text($"Target: {_target}");
        Gui.Text($"Up: {_up}");
        Gui.Text($"Speed: {_movementSpeed}");
        
        Gui.Separator();
        
        Gui.Text($"FOV: {_fieldOfView}");
        Gui.Text($"Clip Plane Near: {_clipPlaneNear}");
        Gui.Text($"Clip Plane Far: {_clipPlaneFar}");
        
        Gui.Separator();
        
        DebugDrawMatrix("World", _worldMatrix);
        DebugDrawMatrix("View", _viewMatrix);
        DebugDrawMatrix("Projection", _projectionMatrix);
        
        Gui.End();
    }

    private void DebugDrawMatrix(string label, in Matrix4x4 matrix)
    {
        Gui.Text(label);
        Gui.Text($"[{matrix.M11:F4}] [{matrix.M12:F4}] [{matrix.M13:F4}] [{matrix.M14:F4}]");
        Gui.Text($"[{matrix.M21:F4}] [{matrix.M22:F4}] [{matrix.M23:F4}] [{matrix.M24:F4}]");
        Gui.Text($"[{matrix.M31:F4}] [{matrix.M32:F4}] [{matrix.M33:F4}] [{matrix.M34:F4}]");
        Gui.Text($"[{matrix.M41:F4}] [{matrix.M42:F4}] [{matrix.M43:F4}] [{matrix.M44:F4}]");
    }

    #endregion Private

    #region Bindings
    

    private void OnMoveLeft(ref ActionEvent evt)
    {
        evt.Consumed = true;
        Position += Vector3Utils.Left * _movementSpeed;
    }

    private void OnMoveRight(ref ActionEvent evt)
    {
        evt.Consumed = true;
        Position += Vector3Utils.Right * _movementSpeed;
    }

    private void OnMoveUp(ref ActionEvent evt)
    {
        evt.Consumed = true;
        Position += Vector3Utils.Up * _movementSpeed;
    }

    private void OnMoveDown(ref ActionEvent evt)
    {
        evt.Consumed = true;
        Position += Vector3Utils.Down * _movementSpeed;
    }

    private void OnMoveForward(ref ActionEvent evt)
    {
        evt.Consumed = true;
        Position += Vector3Utils.Forward * _movementSpeed;
    }

    private void OnMoveBackward(ref ActionEvent evt)
    {
        evt.Consumed = true;
        Position += Vector3Utils.Backward * _movementSpeed;
    }

    private void OnMoveSpeedIncrease(ref ActionEvent evt)
    {
        evt.Consumed = true;
        SetMovementSpeed(_movementSpeed + 10);
    }

    private void OnMoveSpeedDecrease(ref ActionEvent evt)
    {
        evt.Consumed = true;
        SetMovementSpeed(_movementSpeed - 10);
    }

    #endregion Bindings
}