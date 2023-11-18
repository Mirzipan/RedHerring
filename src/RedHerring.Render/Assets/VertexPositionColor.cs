using System.Numerics;
using RedHerring.Numbers;

namespace RedHerring.Render.Assets;

public struct VertexPositionColor
{
    public const uint SizeInBytes = 28;
    
    public Vector3 Position;
    public Color4 Color;

    public VertexPositionColor(Vector3 position, Color4 color)
    {
        Position = position;
        Color = color;
    }
}