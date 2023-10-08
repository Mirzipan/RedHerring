using Silk.NET.Input;

namespace RedHerring.Fingerprint;

public struct Shortcut
{
    public Key PositiveKey { get; set; }
    public Key NegativeKey { get; set; }
    
    public MouseButton PositiveMouse { get; set; }
    public MouseButton NegativeMouse { get; set; }
}