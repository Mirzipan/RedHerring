using RedHerring.Core.Systems;
using RedHerring.Infusion;

namespace RedHerring.Core;

public static class CoreInstaller
{
    public static ContainerDescription AddCoreSystems(this ContainerDescription @this)
    {
        @this.AddSystem(new PathsSystem());
        @this.AddSystem(new WindowConfigurationSystem());
        return @this;
    }
    
    public static ContainerDescription AddGraphics(this ContainerDescription @this)
    {
        return @this.AddSystem(new GraphicsSystem());
    }
    
    public static ContainerDescription AddAssets(this ContainerDescription @this)
    {
        return @this.AddSystem(new AssetsSystem());
    }
}