# Input API

*Disclaimer: this is just a proposal for the API, it is in no way considered final or even implemented.*

## Unified Codes

All buttons (keyboard, mouse, gamepad) exist within a single enum, to greatly simplify access and avoid the need for having essentially having to define the same functions multiple times.
The division of keys into ones with an actual character representing them and ones that are just keycodes, is inspired by the approach used by SDL3.
Names of keys are chosen so that the common part is always in the front, so `ShiftLeft` and `ShiftRight` instead of `LeftShift` or `LShift`.
In order to avoid confusion, there are no alternate names for any values.

Open questions:
* Do we want to include axes as a part of key input? Their values could be returned via `AnalogValue()`.
* How do we call the `Key` enum? At the point where we include just mouse and gamepad buttons, `Key` can still work as a good name, but if we included axes, it may be quite confusing to keep calling it that.
* Make axes into a separate `Axis` enum?

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
}

// characters use their own codes
// keys without characters are shifted by 0x40000000 (1 << 30)
// mouse buttons are shifted by 0x04000000 (1 << 26)
// gamepad buttons are shifted by 0x00400000 (1 << 22)

public enum Key
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
    GamepadStickLeftLeft = 0x00400041, // gamepad axes start (optional)
    GamepadStickLeftRight = 0x00400042,
    GamepadStickLeftUp = 0x00400043,
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
    MouseY = 0x04000042,
    MouseDeltaX = 0x04000043,
    MouseDeltaY = 0x04000044,
    MouseWheelX = 0x04000045,
    MouseWheelY = 0x04000046, // mouse axes end (optional)
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
    bool isUp = Input.IsUp(Key.Enter);
    bool isPressed = Input.IsPressed(Key.Enter);
    bool isDown = Input.IsDown(Key.Enter);
    bool isReleased = Input.IsReleased(Key.Enter);
    
    KeyState state = Input[Key.Enter];
    
    float analog = Input.AnalogValue(Key.Enter);
}
    
// mouse
{
    bool isUp = Input.IsUp(Key.MouseLeft);
    bool isPressed = Input.IsPressed(Key.MouseLeft);
    bool isDown = Input.IsDown(Key.MouseLeft);
    bool isReleased = Input.IsReleased(Key.MouseLeft);
    
    KeyState state = Input[Key.MouseLeft];
    
    float analog = Input.AnalogValue(Key.MouseLeft);
}

// gamepad
{
    bool isUp = Input.IsUp(Key.GamepadA);
    bool isPressed = Input.IsPressed(Key.GamepadA);
    bool isDown = Input.IsDown(Key.GamepadA);
    bool isReleased = Input.IsReleased(Key.GamepadA);
    
    KeyState state = Input[Key.GamepadA];
    
    float analog = Input.AnalogValue(Key.GamepadA);
}

// general
{
    bool isAnyInputDown = Input.Any();
    bool isAnyKeyboardKeyDown = Input.AnyKeyboardKey();
    bool isAnyMouseButtonDown = Input.AnyMouseButton();
    bool isAnyGamepadButtonDown = Input.AnyGamepadButton();
    
    bool areModifiersDown = Input[Modifier.Shift | Modifier.Control];
    bool areModifiersDownAlt = Input.AreDown(Modifier.Shift | Modifier.Control);
    Modifier modifiersDown = Input.Modifiers();
    
    List<Key> keysPressed = new List<Key>();
    Input.KeysPressed(keysPressed);
    
    List<Key> keysDown = new List<Key>();
    Input.KeysDown(keysDown);
    
    List<Key> keysReleased = new List<Key>();
    Input.KeysReleased(keysReleased);
    
    List<char> charactersTyped = new List<char>();
    Input.Characters(charactersTyped);
}

```

### Actions

```csharp

```

### Receiver

```csharp
{
    var receiver = Input.CreateReceiver("layer_name"));
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