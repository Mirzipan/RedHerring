namespace RedHerring.Fingerprint;

public readonly record struct InputEvent(int DeviceId, Input Input, bool IsDown, float AnalogValue);