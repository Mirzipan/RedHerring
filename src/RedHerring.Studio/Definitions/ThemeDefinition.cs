using RedHerring.Clues;

namespace RedHerring.Studio.Definitions;

public abstract partial class ThemeDefinition : Definition
{
    public ThemeDefinition(string name, bool isDefault) : base(Guid.NewGuid(), name, isDefault)
    {
    }

    public abstract void Apply();
}