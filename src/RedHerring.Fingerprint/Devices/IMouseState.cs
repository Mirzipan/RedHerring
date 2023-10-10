﻿namespace RedHerring.Fingerprint.Devices;

public interface IMouseState : IInputState
{
    bool IsButtonUp(MouseButton button);
    bool IsButtonPressed(MouseButton button);
    bool IsButtonDown(MouseButton button);
    bool IsButtonReleased(MouseButton button);
    bool IsMoved(MouseAxis axis);
}