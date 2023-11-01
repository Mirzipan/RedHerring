namespace RedHerring.Fingerprint.Events;

public readonly record struct GamepadButtonEvent(GamepadButton Button, Modifiers Modifiers, bool IsDown);