using System.Numerics;
using System.Runtime.CompilerServices;

namespace RedHerring.Numbers;

public static class Matrix4x4Extensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Up(this Matrix4x4 @this) => new(@this.M21, @this.M22, @this.M23);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Down(this Matrix4x4 @this) => new(-@this.M21, -@this.M22, -@this.M23);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Left(this Matrix4x4 @this) => new(-@this.M11, -@this.M12, -@this.M13);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Right(this Matrix4x4 @this) => new(@this.M11, @this.M12, @this.M13);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Forward(this Matrix4x4 @this) => new(-@this.M31, -@this.M32, -@this.M33);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Backward(this Matrix4x4 @this) => new(@this.M31, @this.M32, @this.M33);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 Row1(this Matrix4x4 @this) => new(@this.M11, @this.M12, @this.M13, @this.M14);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 Row2(this Matrix4x4 @this) => new(@this.M21, @this.M22, @this.M23, @this.M24);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 Row3(this Matrix4x4 @this) => new(@this.M31, @this.M32, @this.M33, @this.M34);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 Row4(this Matrix4x4 @this) => new(@this.M41, @this.M42, @this.M43, @this.M44);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 Column1(this Matrix4x4 @this) => new(@this.M11, @this.M21, @this.M31, @this.M41);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 Column2(this Matrix4x4 @this) => new(@this.M12, @this.M22, @this.M32, @this.M42);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 Column3(this Matrix4x4 @this) => new(@this.M13, @this.M23, @this.M33, @this.M43);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 Column4(this Matrix4x4 @this) => new(@this.M14, @this.M24, @this.M34, @this.M44);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Scale(this Matrix4x4 @this) => new(@this.M11, @this.M22, @this.M33);
    
    
}