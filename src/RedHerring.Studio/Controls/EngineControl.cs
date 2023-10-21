using RedHerring.Studio.Engine;
using Silk.NET.Maths;

namespace RedHerring.Studio.Controls;

public class EngineControl : SilkControl
{
    private static Core.Engine _engine = null!;
    
    protected override void OnLoad()
    {
        try
        {
            _engine = EngineBootstrap.Start(View);
        }
        catch
        {
            // ignored
        }
    }

    protected override void OnDraw(double delta)
    {
        _engine.Draw();
    }

    protected override void OnUpdate(double delta)
    {
        _engine.Update();
    }

    protected override void OnResize(Vector2D<int> size)
    {
        _engine.Resize(size);
    }

    protected override void OnClose()
    {
        _engine.Exit();
    }
}