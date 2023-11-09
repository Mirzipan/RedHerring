namespace RedHerring.Core.Systems;

public sealed class TimeSystem : EngineSystem
{
    public float DefaultTimeScale { get; set; } = 1f;
    public float TimeScale { get; set; } = 1f;
    
    protected override void Init()
    {
    }
}