namespace RedHerring.Fingerprint.Events;

public readonly record struct GamepadAxisMoved(GamepadAxis Axis, float Value);