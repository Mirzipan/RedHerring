﻿using RedHerring.Clues;

namespace RedHerring.Studio.Definition;

public abstract partial class ThemeDefinition : Clues.Definition
{
    public ThemeDefinition(string name, bool isDefault) : base(new DefinitionId(name), name, isDefault)
    {
    }

    public abstract void Apply();
}