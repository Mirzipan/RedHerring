namespace RedHerring.Inputs.Layers;

public readonly record struct ActionBinding(InputState State, ActionEventHandler Handler);