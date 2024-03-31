using RedHerring.Alexandria.Identifiers;
using RedHerring.Clues;

namespace RedHerring.Studio.Definitions;

public abstract partial class ThemeDefinition : Definition
{
    public ThemeDefinition(string name, bool isDefault) : base(new StringId(name), name, isDefault)
    {
    }

    public abstract void Apply();
}