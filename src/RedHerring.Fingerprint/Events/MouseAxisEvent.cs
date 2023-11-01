namespace RedHerring.Fingerprint.Events;

public readonly record struct MouseAxisEvent(MouseAxis Axis, float Value);