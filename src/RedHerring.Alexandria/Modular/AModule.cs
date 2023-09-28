namespace RedHerring.Alexandria.Modular;

public abstract class AModule: IModule
{
    protected abstract IModuleContainer Container { get; }

    IModuleContainer IModule.Container => Container;
}