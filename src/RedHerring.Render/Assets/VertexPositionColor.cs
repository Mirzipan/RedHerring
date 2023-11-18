using System.Numerics;
using Veldrid;

namespace RedHerring.Render.Assets;

public struct VertexPositionColor
{
    public const uint SizeInBytes = 28;
    
    public Vector3 Position;
    public RgbaFloat Color;

    public VertexPositionColor(Vector3 position, RgbaFloat color)
    {
        Position = position;
        Color = color;
    }
}