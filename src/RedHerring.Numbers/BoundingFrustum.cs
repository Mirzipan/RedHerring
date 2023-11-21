using System.Numerics;
using System.Runtime.CompilerServices;

namespace RedHerring.Numbers;

public struct BoundingFrustum
{
    public Plane Near;
    public Plane Far;
    public Plane Left;
    public Plane Right;
    public Plane Top;
    public Plane Bottom;

    #region Lifecycle

    public BoundingFrustum(Matrix4x4 matrix)
    {
        Near = new Plane(
            matrix.M13,
            matrix.M23,
            matrix.M33,
            matrix.M43
            ).Normalize();
        
        Far = new Plane(
            matrix.M14 - matrix.M13, 
            matrix.M24 - matrix.M23, 
            matrix.M34 - matrix.M33, 
            matrix.M44 - matrix.M43
            ).Normalize();
        
        Left = new Plane(
            matrix.M14 + matrix.M11, 
            matrix.M24 + matrix.M21, 
            matrix.M34 + matrix.M31, 
            matrix.M44 + matrix.M41
            ).Normalize();
        
        Right = new Plane(
            matrix.M14 - matrix.M11, 
            matrix.M24 - matrix.M21, 
            matrix.M34 - matrix.M31, 
            matrix.M44 - matrix.M41
            ).Normalize();
        
        Top = new Plane(
            matrix.M14 - matrix.M12, 
            matrix.M24 - matrix.M22, 
            matrix.M34 - matrix.M32, 
            matrix.M44 - matrix.M42
            ).Normalize();
        
        Bottom = new Plane(
            matrix.M14 + matrix.M12, 
            matrix.M24 + matrix.M22, 
            matrix.M34 + matrix.M32, 
            matrix.M44 + matrix.M42
            ).Normalize();
    }

    #endregion Lifecycle
    
    #region Operations
    
    // TODO: intersections and such
    
    #endregion Operations
}