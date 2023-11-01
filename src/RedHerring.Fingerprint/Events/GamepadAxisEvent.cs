namespace RedHerring.Fingerprint.Events;

public readonly record struct GamepadAxisEvent(GamepadAxis Axis, float Value);