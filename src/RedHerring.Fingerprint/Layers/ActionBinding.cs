namespace RedHerring.Fingerprint.Layers;

public readonly record struct ActionBinding(InputState State, ActionEventHandler Handler);