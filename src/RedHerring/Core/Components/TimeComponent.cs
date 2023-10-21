namespace RedHerring.Core.Components;

public sealed class TimeComponent : AnEngineComponent
{
    public float DefaultTimeScale { get; set; } = 1f;
    public float TimeScale { get; set; } = 1f;
    
    protected override void Init()
    {
    }
}