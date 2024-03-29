using System.Numerics;
using System.Runtime.CompilerServices;

namespace RedHerring.Numbers;

public static class QuaternionExtensions
{
    public static Vector3 Up(this Quaternion @this)
    {
        float twoX = @this.X + @this.X;
        float twoY = @this.Y + @this.Y;
        float twoZ = @this.Z + @this.Z;

        float xTwoX = @this.X * twoX;
        float xTwoY = @this.X * twoY;
        float yTwoZ = @this.Y * twoZ;
        float zTwoZ = @this.Z * twoZ;
        float wTwoX = @this.W * twoX;
        float wTwoZ = @this.W * twoZ;
        
        return new Vector3(xTwoY - wTwoZ, 1.0f - xTwoX - zTwoZ, yTwoZ + wTwoX);
    }
    
    public static Vector3 Right(this Quaternion @this)
    {
        float twoY = @this.Y + @this.Y;
        float twoZ = @this.Z + @this.Z;

        float xTwoY = @this.X * twoY;
        float xTwoZ = @this.X * twoZ;
        float yTwoY = @this.Y * twoY;
        float yTwoZ = @this.Y * twoZ;
        float wTwoY = @this.W * twoY;
        float wTwoZ = @this.W * twoZ;
        
        return new Vector3(1.0f - yTwoY - yTwoZ, xTwoY + wTwoZ, xTwoZ - wTwoY);
    }
    
    public static Vector3 Forward(this Quaternion @this)
    {
        float twoX = @this.X + @this.X;
        float twoY = @this.Y + @this.Y;
        float twoZ = @this.Z + @this.Z;

        float xTwoX = @this.X * twoX;
        float xTwoZ = @this.X * twoZ;
        float yTwoY = @this.Y * twoY;
        float yTwoZ = @this.Y * twoZ;
        float wTwoX = @this.W * twoX;
        float wTwoY = @this.W * twoY;
        
        return new Vector3(0.0f - xTwoZ - wTwoY, wTwoX - yTwoZ, xTwoX + yTwoY - 1f);
    }

    #region Equality

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Approximately(this Quaternion @this, Quaternion other, float tolerance = float.Epsilon)
    {
        var delta = @this - other;
        return delta.LengthSquared() <= tolerance * tolerance;
    }

    #endregion Equality
}