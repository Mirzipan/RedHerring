using System.Numerics;
using Vortice.Mathematics;

namespace RedHerring.Render.ImGui;

public static class ColorExtensions
{
    public static Vector4 ToVector4(this Color3 @this) => new(@this.R, @this.G, @this.B, 1f);
}