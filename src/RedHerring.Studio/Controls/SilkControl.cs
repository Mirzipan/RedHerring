using Avalonia.Controls;
using RedHerring.Studio.Engine;
using Silk.NET.Maths;
using Silk.NET.Windowing;
using Silk.NET.Windowing.Extensions.Veldrid;
using Window = Silk.NET.Windowing.Window;

namespace RedHerring.Studio.Controls;

public abstract class SilkControl : Control
{
    private IView _view = null!;

    public IView View => _view;

    public sealed override void BeginInit()
    {
        base.BeginInit();

        try
        {
            var options = new ViewOptions
            {
                API = EngineBootstrap.GetPreferredBackend().ToGraphicsAPI(),
                VSync = false,
                ShouldSwapAutomatically = false,
            };

            _view = Window.GetView(options); // this unfortunately creates a new window .... 
            _view.Load += OnLoad;
            _view.Update += OnUpdate;
            _view.Render += OnDraw;
            _view.Closing += OnClose;
            _view.Resize += OnResize;
        }
        catch
        {
            // ignored
        }
    }

    public override void EndInit()
    {
        base.EndInit();

        try
        {
            _view.Run();
        }
        catch
        {
            // ignored
        }
    }

    protected virtual void OnLoad()
    {
    }

    protected virtual void OnDraw(double delta)
    {
    }

    protected virtual void OnUpdate(double delta)
    {
    }

    protected virtual void OnResize(Vector2D<int> size)
    {
    }

    protected virtual void OnClose()
    {
    }
}