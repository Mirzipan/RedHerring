namespace RedHerring.Fingerprint.Events;

public readonly record struct KeyChanged(Key Key, Modifiers Modifiers, bool IsDown);