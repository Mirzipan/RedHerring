using RedHerring.Render.Materials;
using Silk.NET.Maths;
using Vortice.Mathematics;

namespace RedHerring.Render.Models;

public class Model
{
    public Guid AssetId;
    public BoundingBox BoundingBox;
    public BoundingSphere BoundingSphere;
    public List<Mesh> Meshes;
    public List<Material> Materials;

    public List<Vector3D<float>> Vertices;
    public List<Vector3D<float>> Normals;
    public List<int> Indices;
}