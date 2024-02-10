namespace RedHerring.Studio.Definition;

public abstract partial class ThemeDefinition : Clues.Definition
{
    public ThemeDefinition(string name, bool isDefault) : base(Guid.NewGuid(), name, isDefault)
    {
    }

    public abstract void Apply();
}