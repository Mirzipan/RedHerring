namespace RedHerring.Fingerprint.Events;

public readonly record struct KeyEvent(Key Key, Modifiers Modifiers, bool IsDown);