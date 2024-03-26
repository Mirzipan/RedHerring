using System.Numerics;
using RedHerring.Alexandria;
using RedHerring.Alexandria.Identifiers;
using RedHerring.Core;
using RedHerring.Fingerprint;
using RedHerring.Fingerprint.Layers;
using RedHerring.Infusion.Attributes;
using RedHerring.Numbers;
using RedHerring.Render;
using RedHerring.Studio.Constants;

namespace RedHerring.Studio.Systems;

public sealed class StudioCamera : EngineSystem, Drawable
{
    public const float FieldOfViewMin = MathF.PI / 4;
    public const float ClipPlaneNearMin = 0.01f;
    public const float ClipPlaneFarDefault = 1000f;

    public const float MovementSpeedMin = 0.01f;
    public const float MovementSpeedMax = 10f;
    public const float MovementSpeedDefault = 0.01f;
    public const float MovementSpeedStep = 0.01f;

    [Infuse]
    private RendererContext _rendererContext = null!;
    [Infuse]
    private InputLayer _layer = null!;

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

    public Matrix4x4 WorldMatrix
    {
        get 
        {
            if (_isDirty)
            {
                CalculateMatrices();
            }

            return _worldMatrix;
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
    }

    public void EndDraw()
    {
    }

    public void Activate()
    {
        _isActive = true;
        _layer.Push();
    }

    public void Deactivate()
    {
        _isActive = false;
        _layer.Pop();
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
        _position = Vector3Direction.Backward * 10;
        _target = Vector3Direction.Forward * 30;
        _up = Vector3Direction.Up;
        
        _viewMatrix = Matrix4x4.Identity;
        _projectionMatrix = Matrix4x4.Identity;
        
        SetFieldOfView(FieldOfViewMin);
        SetClipPlanes(ClipPlaneNearMin, ClipPlaneFarDefault);
        SetMovementSpeed(MovementSpeedDefault);

        _isDirty = true;
    }

    private void SetupInput()
    {
        _layer.Name = "studio camera";
        _layer.Layer = new OctoByte("camera");
        _layer.ConsumesAllInput = false;
        
        _layer.Bind(InputAction.MoveLeft, InputState.Down, OnMoveLeft);
        _layer.Bind(InputAction.MoveRight, InputState.Down, OnMoveRight);
        _layer.Bind(InputAction.MoveUp, InputState.Down, OnMoveUp);
        _layer.Bind(InputAction.MoveDown, InputState.Down, OnMoveDown);
        _layer.Bind(InputAction.MoveForward, InputState.Down, OnMoveForward);
        _layer.Bind(InputAction.MoveBackward, InputState.Down, OnMoveBackward);
        
        _layer.Bind(InputAction.MoveSpeedIncrease, InputState.Down, OnMoveSpeedIncrease);
        _layer.Bind(InputAction.MoveSpeedDecrease, InputState.Down, OnMoveSpeedDecrease);
    }

    private void SubmitToRenderer()
    {
        if (_isDirty)
        {
            CalculateMatrices();
        }
        
        _rendererContext.SetCameraViewMatrix(_worldMatrix, _viewMatrix, _projectionMatrix, FieldOfView, ClipPlaneNear, ClipPlaneFar);
    }

    private void CalculateMatrices()
    {
        var size = Context.Window.Size;
        float aspectRatio = 16f / 9;
        if (size.Y != 0)
        {
            aspectRatio = (float)size.X / size.Y;
        }

        _worldMatrix = Matrix4x4.CreateLookAt(_position, _position + Vector3Direction.Forward, _up);
        Matrix4x4.Invert(_worldMatrix, out _viewMatrix);
        _projectionMatrix = Matrix4x4.CreatePerspectiveFieldOfView(FieldOfView, aspectRatio, ClipPlaneNear, ClipPlaneFar);
        
        _isDirty = false;
    }

    #endregion Private

    #region Bindings
    

    private void OnMoveLeft(ref ActionEvent evt)
    {
        evt.Consumed = true;
        Position += Vector3Direction.Left * _movementSpeed;
    }

    private void OnMoveRight(ref ActionEvent evt)
    {
        evt.Consumed = true;
        Position += Vector3Direction.Right * _movementSpeed;
    }

    private void OnMoveUp(ref ActionEvent evt)
    {
        evt.Consumed = true;
        Position += Vector3Direction.Up * _movementSpeed;
    }

    private void OnMoveDown(ref ActionEvent evt)
    {
        evt.Consumed = true;
        Position += Vector3Direction.Down * _movementSpeed;
    }

    private void OnMoveForward(ref ActionEvent evt)
    {
        evt.Consumed = true;
        Position += Vector3Direction.Forward * _movementSpeed;
    }

    private void OnMoveBackward(ref ActionEvent evt)
    {
        evt.Consumed = true;
        Position += Vector3Direction.Backward * _movementSpeed;
    }

    private void OnMoveSpeedIncrease(ref ActionEvent evt)
    {
        evt.Consumed = true;
        SetMovementSpeed(_movementSpeed + MovementSpeedStep * evt.Value);
    }

    private void OnMoveSpeedDecrease(ref ActionEvent evt)
    {
        evt.Consumed = true;
        SetMovementSpeed(_movementSpeed - MovementSpeedStep * evt.Value);
    }

    #endregion Bindings
}