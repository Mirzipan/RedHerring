using System.Numerics;
using RedHerring.Motive.Entities.Components;

namespace RedHerring.Incident;

public class RigidbodyComponent : APhysicsComponent
{
    // TODO: we kinda need a physics library at this point
    
    public override bool IsKinematic { get; set; }
    public override float Mass { get; set; }
    public override float Speed { get; set; }
    public override float Friction { get; set; }
    public override float RollingFriction { get; set; }
    public override float Restitution { get; set; }
    public override float Damping { get; set; }
    public override float AngularDamping { get; set; }
    public override Vector3 LinearVelocity { get; set; }
    public override Vector3 AngularVelocity { get; set; }
    public override Vector3 Gravity { get; set; }
}