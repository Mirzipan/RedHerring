namespace RedHerring.Fingerprint.Events;

public readonly record struct MouseButtonChanged(MouseButton Button, Modifiers Modifiers, bool IsDown);