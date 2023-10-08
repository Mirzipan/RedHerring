using RedHerring.Alexandria;
using Vortice.Mathematics;

namespace RedHerring.Motive.Entities.Components;

public class LightComponent : AnEntityComponent, IActivatable
{
    public bool IsActive { get; set; }
    public Color Color { get; set; }
    public float Intensity { get; set; }
    public float FallOff { get; set; }
    public float Range { get; set; }
}