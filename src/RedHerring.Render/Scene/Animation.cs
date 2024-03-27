namespace RedHerring.Render.Models;

[Serializable]
public sealed class Animation
{
    public string Name;
    public double DurationInTicks;
    public double TicksPerSecond;
    
    // TODO(Mirzi): add arrays of keys + some info about how to interpolate them
}