using ImGuiNET;
using RedHerring.Alexandria;
using RedHerring.Core;
using RedHerring.Core.Systems;
using RedHerring.Fingerprint;
using RedHerring.Fingerprint.Layers;
using RedHerring.Fingerprint.Shortcuts;
using RedHerring.Infusion.Attributes;
using RedHerring.Studio.Commands;
using RedHerring.Studio.UserInterface;
using RedHerring.Studio.UserInterface.Dialogs;
using Gui = ImGuiNET.ImGui;

namespace RedHerring.Studio.Systems;

// TODO: Add this to engine context when creating editor window.
public sealed class EditorSystem : AnEngineSystem, IUpdatable, IDrawable
{
    [Inject] private InputSystem    _inputSystem    = null!;
    [Inject] private GraphicsSystem _graphicsSystem = null!;

    private InputReceiver _inputReceiver;
    
    private readonly CommandHistory _history = new CommandHistory();

    private readonly Menu        _menu        = new();
    private readonly StatusBar   _statusBar   = new();
    private readonly MessageBox  _messageBox  = new();
    private readonly TestWindows _testWindows = new();

    public bool IsEnabled => true;
    public int UpdateOrder => int.MaxValue;

    public bool IsVisible => true;
    public int DrawOrder => int.MaxValue;

    #region Lifecycle

    protected override void Init()
    {
        _inputReceiver = new InputReceiver("editor");
        _inputReceiver.ConsumesAllInput = false;
        
        _inputReceiver.Bind("undo", Undo);
        _inputReceiver.Bind("redo", Redo);
    }

    protected override void Load()
    {
        ImGuiNET.ImGui.GetIO().ConfigFlags |= ImGuiConfigFlags.DockingEnable;

        InitInput();

        _menu.AddItem("File/Exit",          OnExitClicked);
        
        _menu.AddItem("Edit/Undo",          _history.Undo);
        _menu.AddItem("Edit/Redo",          _history.Redo);

        _menu.AddItem("Debug/Modal window", () => Gui.OpenPopup("MessageBox"));
    }

    public void Update(GameTime gameTime)
    {
        //Gui.DockSpaceOverViewport(Gui.GetMainViewport());
        _testWindows.Update();
        
        //Gui.ShowDemoWindow();
        _menu.Update();
        _statusBar.Update();
        _messageBox.Update();
    }

    public bool BeginDraw() => true;

    public void Draw(GameTime gameTime)
    {
    }

    public void EndDraw()
    {
    }

    #endregion Lifecycle

    #region Private

    private void InitInput()
    {
        _inputSystem.Input.Bindings!.Add(new ShortcutBinding("undo", new KeyboardShortcut(Key.U)));
        _inputSystem.Input.Bindings!.Add(new ShortcutBinding("redo", new KeyboardShortcut(Key.Z)));
        _inputSystem.Input.Layers.Push(_inputReceiver);
    }

    private void OnExitClicked()
    {
        Container.Engine.Exit();
    }

    #endregion Private

    #region Input

    private void Undo(ref ActionEvent evt)
    {
        evt.Consumed = true;
        
        _history.Undo();
    }

    private void Redo(ref ActionEvent evt)
    {
        evt.Consumed = true;
        
        _history.Redo();
    }

    #endregion Input
}