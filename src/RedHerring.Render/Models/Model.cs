using RedHerring.Render.Materials;
using Vortice.Mathematics;

namespace RedHerring.Render.Models;

public class Model
{
    public Guid            AssetId;
    public BoundingBox     BoundingBox;
    public BoundingSphere  BoundingSphere;
    public List<Mesh>      Meshes = new();
    public List<Material>? Materials;

    public ModelNode? Root;
}