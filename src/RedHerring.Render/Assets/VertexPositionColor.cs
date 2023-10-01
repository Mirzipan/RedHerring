using System.Numerics;
using Veldrid;

namespace RedHerring.Render.Assets;

public struct VertexPositionColor
{
    public const uint SizeInBytes = 24;
    
    public Vector2 Position;
    public RgbaFloat Color;

    public VertexPositionColor(Vector2 position, RgbaFloat color)
    {
        Position = position;
        Color = color;
    }
}