using RedHerring.Alexandria;
using RedHerring.Engines;
using RedHerring.Engines.Components;
using RedHerring.Fingerprint;
using RedHerring.Fingerprint.Layers;
using RedHerring.Fingerprint.Shortcuts;
using RedHerring.Studio.Commands;

namespace RedHerring.Studio;

// TODO: Add this to engine context when creating editor window.
public sealed class EditorComponent : AnEngineComponent, IUpdatable, IDrawable
{
    private InputComponent _inputComponent;
    private RenderComponent _renderComponent;

    private InputReceiver _inputReceiver;
    
    private readonly CommandHistory _history = new CommandHistory();

    public bool IsEnabled => true;
    public int UpdateOrder => int.MaxValue;

    public bool IsVisible => true;
    public int DrawOrder => int.MaxValue;

    #region Lifecycle

    protected override void Init()
    {
        _inputReceiver = new InputReceiver("editor");
        _inputReceiver.ConsumesAllInput = true;
        
        _inputReceiver.Bind("undo", Undo);
        _inputReceiver.Bind("redo", Redo);
    }

    protected override void Load()
    {
        // TODO: replace by injection when available
        _inputComponent = Container.Components.Get<InputComponent>()!;
        _renderComponent = Container.Components.Get<RenderComponent>()!;
        
        InitInput();
    }

    public void Update(GameTime gameTime)
    {
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
        _inputComponent.Input.Bindings!.Add(new ShortcutBinding("undo", new KeyboardShortcut(Key.U)));
        _inputComponent.Input.Bindings!.Add(new ShortcutBinding("redo", new KeyboardShortcut(Key.Z)));
        _inputComponent.Input.Layers.Push(_inputReceiver);
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