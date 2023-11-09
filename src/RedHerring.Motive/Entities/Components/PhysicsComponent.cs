using System.Numerics;
using RedHerring.Alexandria;

namespace RedHerring.Motive.Entities.Components;

public abstract class PhysicsComponent : EntityComponent, Activatable
{
    public bool IsActive { get; set; }
    
    public abstract bool IsKinematic { get; set; }
    
    public abstract float Mass { get; set; }
    public abstract float Speed { get; set; }
    public abstract float Friction { get; set; }
    public abstract float RollingFriction { get; set; }
    public abstract float Restitution { get; set; }
    public abstract float Damping { get; set; }
    public abstract float AngularDamping { get; set; }
    
    public abstract Vector3 LinearVelocity { get; set; }
    public abstract Vector3 AngularVelocity { get; set; }
    public abstract Vector3 Gravity { get; set; }
    
}