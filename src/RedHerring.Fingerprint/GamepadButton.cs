namespace RedHerring.Fingerprint;

public enum GamepadButton
{
    /// <summary>
    /// Indicates that the input backend was unable to determine a button name for the button in question, or it does not support it.
    /// </summary>
    Unknown = -1, // 0xFFFFFFFF
    /// <summary>The A button.</summary>
    A = 0,
    /// <summary>The B button.</summary>
    B = 1,
    /// <summary>The X button.</summary>
    X = 2,
    /// <summary>The Y button.</summary>
    Y = 3,
    /// <summary>The left bumper.</summary>
    LeftBumper = 4,
    /// <summary>The right bumper.</summary>
    RightBumper = 5,
    /// <summary>The back button.</summary>
    Back = 6,
    /// <summary>The start button.</summary>
    Start = 7,
    /// <summary>The home button.</summary>
    Home = 8,
    /// <summary>Clicking the left stick.</summary>
    LeftStick = 9,
    /// <summary>Clicking the right stick.</summary>
    RightStick = 10, // 0x0000000A
    /// <summary>Up on the D-Pad.</summary>
    DPadUp = 11, // 0x0000000B
    /// <summary>Right on the D-Pad.</summary>
    DPadRight = 12, // 0x0000000C
    /// <summary>Down on the D-Pad.</summary>
    DPadDown = 13, // 0x0000000D
    /// <summary>Left on the D-Pad.</summary>
    DPadLeft = 14, // 0x0000000E
}