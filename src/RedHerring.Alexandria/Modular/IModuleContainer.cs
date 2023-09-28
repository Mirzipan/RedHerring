namespace RedHerring.Alexandria.Modular;

public interface IModuleContainer
{
    IModule Get(Type moduleType);
}