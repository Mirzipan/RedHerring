using RedHerring.Clues;

namespace RedHerring.Studio.Definitions;

public partial class ThemeDefinition
{
    public static readonly ThemeDefinition EmbraceTheDarkness = new EmbraceTheDarknessTheme();
    public static readonly ThemeDefinition CrimsonRivers = new CrimsonRiversTheme();
    public static readonly ThemeDefinition Bloodsucker = new BloodsuckerTheme();

    public static void AddToSet(DefinitionSet set)
    {
        set.Add(EmbraceTheDarkness);
        set.Add(CrimsonRivers);
        set.Add(Bloodsucker);
    }
}