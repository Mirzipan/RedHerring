namespace RedHerring.Fingerprint.Events;

public readonly record struct GamepadButtonChanged(GamepadButton Button, Modifiers Modifiers, bool IsDown);