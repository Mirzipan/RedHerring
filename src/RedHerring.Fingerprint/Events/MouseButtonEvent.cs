namespace RedHerring.Fingerprint.Events;

public readonly record struct MouseButtonEvent(MouseButton Button, Modifiers Modifiers, bool IsDown);