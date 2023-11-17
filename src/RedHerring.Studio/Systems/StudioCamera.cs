﻿using System.Numerics;
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
    public const float MinFieldOfView = MathF.PI / 4;
    public const float MinClipPlaneNear = 0.01f;
    public const float DefaultClipPlaneFar = 1000f;
    public const float DefaultMovementSpeed = 100f;

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
        SetFieldOfView(MinFieldOfView);
        SetClipPlanes(MinClipPlaneNear, DefaultClipPlaneFar);
        SetMovementSpeed(DefaultMovementSpeed);
        
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
        _fieldOfView = float.Clamp(fov, MinFieldOfView, MathF.PI);
        _isDirty = true;
    }

    public void SetClipPlanes(float near, float far)
    {
        _clipPlaneNear = MathF.Max(near, MinClipPlaneNear);
        _clipPlaneFar = MathF.Max(far, near + 0.01f);
        _isDirty = true;
    }

    public void SetMovementSpeed(float value)
    {
        _movementSpeed = float.Clamp(value, 1, 1000);
    }

    #endregion Public

    #region Private

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