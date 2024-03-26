# Input API

*Disclaimer: this is just a proposal for the API, it is in no way considered final or even implemented.*

## Unified Codes

All buttons (keyboard, mouse, gamepad) exist within a single enum, to greatly simplify access and avoid the need for having essentially having to define the same functions multiple times.
The division of keys into ones with an actual character representing them and ones that are just keycodes, is inspired by the approach used by SDL3.
Names of keys are chosen so that the common part is always in the front, so `ShiftLeft` and `ShiftRight` instead of `LeftShift` or `LShift`.
In order to avoid confusion, there are no alternate names for any values.
All axes prefer using coordinates in their names, so instead of `MouseHorizontal` or `MouseVertical`, it's `MouseX` and `MouseY`.
Unified codes also include all axes as full axis, positive side, and negative side.

```csharp
[Flags]
public enum KeyState : byte
{
    Up = 0,
    Pressed = 1 << 0,
    Down = 1 << 1,
    Released = 1 << 2,
    Any = Pressed | Down | Released,
}

[Flags]
public enum Modifier
{
    None = 0,
    Alt = 1 << 0,
    Control = 1 << 1,
    Shift = 1 << 2,
    Super = 1 << 3,
    
    AltControl = Alt | Control,
    AltShift = Alt | Shift,
    AltSuper = Alt | Super,
    ControlShift = Control | Shift,
    ControlSuper = Control | Super,
    ShiftSuper = Shift | Super,
    
    AltControlShift = Alt | Control | Shift,
    AltControlSuper = Alt | Control | Super,
    AltShiftSuper = Alt | Shift | Super,
    ControlShiftSuper = Control | Shift | Super,
        
    All = Alt | Control | Shift | Super,
}

// if only positive is defined, works as a basic keypress shortuct.
// if negative is also defined, the analog values for negative value will be inverted
// if modifiers are defined, it works as multi-key shortcut
public struct Shortcut
{
    public Input Positive;
    public Input Negative;
    public Modifier Modifiers;
}

// characters use their own codes
// keys without characters are shifted by 0x40000000 (1 << 30)
// mouse buttons are shifted by 0x04000000 (1 << 26)
// gamepad buttons are shifted by 0x00400000 (1 << 22)
public enum Input
{
    Unknown = 0x00, // characters start
    Backspace = 0x08,
    Tab = 0x09,
    ...
    Delete = 0x7F, // characters end
    ...
    GamepadA = 0x00400001, // cross
    GamepadB = 0x00400002, // circle
    GamepadX = 0x00400003, // square
    GamepadY = 0x00400004, // triangle
    GamepadBumperLeft = 0x00400005,
    GamepadBumperRight = 0x00400006,
    GamepadBack = 0x00400007,
    GamepadStart = 0x00400008,
    GamepadHome = 0x00400009,
    GamepadStickLeft = 0x0040000A,
    GamepadStickRight = 0x0040000B,
    GamepadDPadUp = 0x0040000C,
    GamepadDPadRight = 0x0040000D,
    GamepadDPadDown = 0x0040000E,
    GamepadDPadLeft = 0x0040000F,
    ...
    GamepadStickLeftX = 0x00400041, // gamepad axes start (optional)
    GamepadStickLeftRight = 0x00400042,
    GamepadStickLeftY = 0x00400043,
    GamepadStickLeftDown = 0x00400044,
    GamepadStickRightLeft = 0x00400045,
    GamepadStickRightRight = 0x00400046,
    GamepadStickRightUp = 0x00400047,
    GamepadStickRightDown = 0x00400048,
    GamepadTriggerLeft = 0x00400049,
    GamepadTriggerRight = 0x0040004A, // gamepad axes end (optional)
    ...
    MouseLeft = 0x04000001, // mouse start
    MouseRight = 0x04000002,
    MouseMiddle = 0x04000003,
    Mouse4 = 0x04000004,
    Mouse5 = 0x04000005,
    Mouse6 = 0x04000006,
    Mouse7 = 0x04000007,
    Mouse8 = 0x04000008,
    Mouse9 = 0x04000009,
    Mouse10 = 0x0400000A,
    Mouse11 = 0x0400000B,
    Mouse12 = 0x0400000C, // mouse end
    ...
    MouseX = 0x04000041, // mouse axes start (optional)
    MouseXPositive= 0x04000042,
    MouseXNegative= 0x04000043,
    MouseY = 0x04000044,
    MouseYPositive= 0x04000045,
    MouseYNegative= 0x04000046,
    MouseDeltaX = 0x04000047,
    MouseDeltaY = 0x04000048,
    MouseWheelX = 0x04000049,
    MouseWheelY = 0x0400004A, // mouse axes end (optional)
    ...
    CapsLock = 0x40000039, // characterless start
    ...
    Eject = 0x40000119,
    Sleep = 0x4000011A, // characterless end
}
```


## Usage

### Query

```csharp
// keyboard
{
    bool isUp = Interaction.IsUp(Input.Enter);
    bool isPressed = Interaction.IsPressed(Input.Enter);
    bool isDown = Interaction.IsDown(Input.Enter);
    bool isReleased = Interaction.IsReleased(Input.Enter);
    
    KeyState state = Interact[Input.Enter];
    
    float analog = Interaction.AnalogValue(Input.Enter);
    
    // extension methods for Input enum
    bool isUp = Input.Enter.IsUp();
    bool isPressed = Input.Enter.IsPressed();
    bool isDown = Input.Enter.IsDown();
    bool isReleased = Input.Enter.IsReleased();
    
    float analog = Input.Enter.AnalogValue();
    
    bool isDown = (Modifier.Control | Modifier.Shift).IsDown(); 
}
    
// mouse
{
    bool isUp = Interaction.IsUp(Input.MouseLeft);
    bool isPressed = Interaction.IsPressed(Input.MouseLeft);
    bool isDown = Interaction.IsDown(Input.MouseLeft);
    bool isReleased = Interaction.IsReleased(Input.MouseLeft);
    
    KeyState state = Interact[Input.MouseLeft];
    
    float analog = Interaction.AnalogValue(Input.MouseLeft);
}

// gamepad
{
    bool isUp = Interaction.IsUp(Input.GamepadA);
    bool isPressed = Interaction.IsPressed(Input.GamepadA);
    bool isDown = Interaction.IsDown(Input.GamepadA);
    bool isReleased = Interaction.IsReleased(Input.GamepadA);
    
    KeyState state = Interact[Input.GamepadA];
    
    float analog = Interaction.AnalogValue(Input.GamepadA);
}

// general
{
    bool isAnyInputDown = Interaction.Any();
    bool isAnyKeyboardKeyDown = Interaction.AnyKeyboardKey();
    bool isAnyMouseButtonDown = Interaction.AnyMouseButton();
    bool isAnyGamepadButtonDown = Interaction.AnyGamepadButton();
    
    bool areModifiersDown = Interact[Modifier.Shift | Modifier.Control];
    bool areModifiersDownAlt = Interaction.AreDown(Modifier.Shift | Modifier.Control);
    Modifier modifiersDown = Interaction.Modifiers();
    
    List<Key> keysPressed = new List<Key>();
    Interaction.KeysPressed(keysPressed);
    
    List<Key> keysDown = new List<Key>();
    Interaction.KeysDown(keysDown);
    
    List<Key> keysReleased = new List<Key>();
    Interaction.KeysReleased(keysReleased);
    
    List<char> charactersTyped = new List<char>();
    Interaction.Characters(charactersTyped);
}

```

### Actions

```csharp

```

### Receiver

```csharp
{
    var receiver = Interaction.CreateReceiver("layer_name"));
    receiver.Bind("action_name", KeyState.Pressed, OnShortcutPressed);
}

public record struct ActionEvent(string Action, InputState State, float Value)
{
    public bool Consumed;
}

private void OnShortcutPressed(ref ActionEvent evt)
{
    
}

```