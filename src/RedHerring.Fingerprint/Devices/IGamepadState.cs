﻿using System.Numerics;

namespace RedHerring.Fingerprint.Devices;

public interface IGamepadState : IInputState
{
    Vector2 LeftThumb { get; }
    Vector2 RightThumb { get; }
    float LeftTrigger { get; }
    float RightTrigger { get; }
}