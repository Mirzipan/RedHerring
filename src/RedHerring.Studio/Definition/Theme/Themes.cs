﻿using RedHerring.Clues;

namespace RedHerring.Studio.Definition;

public abstract partial class ThemeDefinition
{
    public static readonly ThemeDefinition EmbraceTheDarkness = new EmbraceTheDarknessTheme();
    public static readonly ThemeDefinition CrimsonRivers = new CrimsonRiversTheme();
    public static readonly ThemeDefinition Bloodsucker = new BloodsuckerTheme();

    public static void AddToContext(DefinitionsContext context)
    {
        context.Add(EmbraceTheDarkness);
        context.Add(CrimsonRivers);
        context.Add(Bloodsucker);
    }
}