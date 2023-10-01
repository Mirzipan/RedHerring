using System.Numerics;
using Vortice.Mathematics;

namespace RedHerring.Render.Lights;

public class Light
{
    public Matrix4x4 WorldMatrix;
    public float Intensity;
    public Vector3 Position;
    public Vector3 Direction;
    public Color3 Color;
    public BoundingBox BoundingBox;
}