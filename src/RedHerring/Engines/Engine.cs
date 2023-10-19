using RedHerring.Alexandria;
using RedHerring.Alexandria.Components;
using RedHerring.Engines.Components;
using RedHerring.Exceptions;
using RedHerring.Games;
using RedHerring.Infusion;
using Silk.NET.Maths;

namespace RedHerring.Engines;

public sealed class Engine : AThingamabob, IComponentContainer
{
    public InjectionContainer InjectionContainer { get; private set; } = null!;
    public EngineComponentCollection Components { get; }
    public AnEngineContext Context { get; private set; } = null!;
    public GraphicsComponent? Renderer { get; private set; } = null!;
    public Game? Game { get; private set; }
    public bool IsRunning { get; private set; }
    public bool IsExiting { get; private set; }

    public GameTime UpdateTime { get; }
    public GameTime DrawTime { get; }
    
    private Cronos _cronos;
    private int _frameCount;
    
    #region Lifecycle

    public Engine()
    {
        Components = new EngineComponentCollection(this);
        
        IsRunning = false;
        IsExiting = false;

        UpdateTime = new GameTime();
        DrawTime = new GameTime();

        _cronos = new Cronos();
    }

    public void Run(Game game)
    {
        if (!IsRunning)
        {
            throw new EngineNotRunningException();
        }
        
        Game = game;
        Game.Initialize(this);
    }

    public void Run(AnEngineContext context)
    {
        if (IsRunning)
        {
            throw new EngineAlreadyRunningException();
        }
        
        Context = context;
        _cronos.Reset();
        _frameCount = 0;

        InitFromContext();
        
        IsRunning = true;
    }

    public void Tick()
    {
        if (IsExiting)
        {
            return;
        }

        _cronos.Tick();
        
        ++_frameCount;

        TickInternal();
    }

    public void Exit()
    {
        IsExiting = true;
        Game?.Close();
        
        Components.Unload();
    }

    #endregion Lifecycle

    #region Public

    public void Resize(Vector2D<int> size)
    {
        Renderer?.Resize(size);
    }

    IComponent? IComponentContainer.Get(Type type) => ((IComponentContainer)Components).Get(type);

    #endregion Public

    #region Private

    private void InitFromContext()
    {
        Components.Init();
        
        var description = new ContainerDescription("Engine");
        Components.InstallBindings(description);
        InjectionContainer = description.Build();
        
        Components.Load();

        Renderer = Components.Get<GraphicsComponent>();
    }

    private void TickInternal()
    {
        bool isDrawing = Renderer?.BeginDraw() ?? false;

        // TODO: make draw and update independent
        UpdateTime.Update(_cronos.TotalTime, _cronos.ElapsedTime, _frameCount);
        Update(UpdateTime);        
        
        DrawTime.Update(_cronos.TotalTime, _cronos.ElapsedTime, _frameCount);
        Draw(DrawTime);

        if (isDrawing)
        {
            Renderer!.EndDraw();
        }
    }

    private void Draw(GameTime time)
    {
        Components.Draw(time);
        Game?.Draw(time);
        
        Renderer!.Draw();
    }

    private void Update(GameTime time)
    {
        Components.Update(time);
        Game?.Update(time);
    }

    #endregion Private
}