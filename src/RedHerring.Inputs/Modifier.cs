namespace RedHerring.Inputs;

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

    ControlAlt = Control | Alt,
    ControlShift = Control | Shift,
    ControlSuper = Control | Super,

    ShiftAlt = Shift | Alt,
    ShiftControl = Shift | Control,
    ShiftSuper = Shift | Super,

    SuperAlt = Super | Alt,
    SuperControl = Super | Control,
    SuperShift = Super | Shift,
    
    AltControlShift = Alt | Control | Shift,
    AltControlSuper = Alt | Control | Super,
    
    AltShiftControl = Alt | Shift | Control,
    AltShiftSuper = Alt | Shift | Super,
    
    AltSuperControl = Alt | Super | Control,
    AltSuperShift = Alt | Super | Shift,
    
    ControlAltShift = Control | Alt | Shift,
    ControlAltSuper = Control | Alt | Super,
    
    ControlShiftAlt = Control | Shift | Alt,
    ControlShiftSuper = Control | Shift | Super,
    
    ControlSuperAlt = Control | Super | Alt,
    ControlSuperShift = Control | Super | Shift,
    
    ShiftAltControl = Shift | Alt | Control,
    ShiftAltSuper = Shift | Alt | Super,
    
    ShiftControlAlt = Shift | Control | Alt,
    ShiftControlSuper = Shift | Control | Super,
    
    ShiftSuperAlt = Shift | Super | Alt,
    ShiftSuperControl = Shift | Super | Control,
    
    SuperAltControl = Super | Alt | Control,
    SuperAltShift = Super | Alt | Shift,
    
    SuperControlAlt = Super | Control | Alt,
    SuperControlShift = Super | Control | Shift,
    
    SuperShiftAlt = Super | Shift | Alt,
    SuperShiftControl = Super | Shift | Control,

    All = Alt | Control | Shift | Super,
}