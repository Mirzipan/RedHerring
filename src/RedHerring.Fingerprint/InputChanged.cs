namespace RedHerring.Fingerprint;

public readonly record struct InputChanged(int DeviceId, Input Input, bool IsDown, float AnalogValue);