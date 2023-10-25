using RedHerring.Core.Systems;
using RedHerring.Infusion;

namespace RedHerring.Core;

public static class CoreInstaller
{
    public static ContainerDescription AddGraphics(this ContainerDescription @this)
    {
        return @this.AddSystem(new GraphicsSystem());
    }
    
    public static ContainerDescription AddInput(this ContainerDescription @this)
    {
        return @this.AddSystem(new InputSystem());
    }
    
    public static ContainerDescription AddAssets(this ContainerDescription @this)
    {
        return @this.AddSystem(new AssetsSystem());
    }
}