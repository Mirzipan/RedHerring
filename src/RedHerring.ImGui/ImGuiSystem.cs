using RedHerring.Alexandria;
using RedHerring.Core;
using RedHerring.Core.Systems;
using RedHerring.Infusion.Attributes;
using Gui = ImGuiNET.ImGui;

namespace RedHerring.ImGui;

public class ImGuiSystem : AnEngineSystem, IUpdatable, IDrawable
{
    [Inject]
    private InputSystem _inputSystem = null!;
    [Inject]
    private GraphicsSystem _graphicsSystem = null!;

    private InputState _inputSnapshot = null!;
    private ImGuiRenderFeature _feature = null!;
    
    public bool IsVisible => true;
    public int DrawOrder => 10_000_000;

    public bool IsEnabled => true;
    public int UpdateOrder => -10_000_000;

    #region Lifecycle

    protected override void Init()
    {
    }

    protected override void Load()
    {
        _inputSnapshot = new InputState();
        _feature = new ImGuiRenderFeature();
        _graphicsSystem.RegisterFeature(_feature);
    }

    protected override void Unload()
    {
    }
    
    public void Update(GameTime gameTime)
    {
        _inputSnapshot.Update(_inputSystem.Input);
        _feature.Update(gameTime, _inputSnapshot);
    }

    #endregion Lifecycle

    #region Drawable

    public bool BeginDraw()
    {
        return true;
    }

    public void Draw(GameTime gameTime)
    {
    }

    public void EndDraw()
    {
    }

    #endregion Drawable
}