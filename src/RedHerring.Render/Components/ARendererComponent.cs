using RedHerring.Core.Components;
using Veldrid;

namespace RedHerring.Render.Components;

public abstract class ARendererComponent : AComponent<RendererComponentCollection>
{
    private RendererComponentCollection? _container;
    public override RendererComponentCollection? Container => _container;
    public abstract int Priority { get; set; }
    
    public abstract void Draw(CommandList commandList);

    internal void SetContainer(RendererComponentCollection container)
    {
        _container = container;
    }
}