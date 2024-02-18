using RedHerring.Render.Materials;
using Vortice.Mathematics;

namespace RedHerring.Render.Models;

public class Scene
{
    public Guid            AssetId;
    public BoundingBox     BoundingBox;
    public BoundingSphere  BoundingSphere;
    public List<SceneMesh>      Meshes = new();
    public List<Material>? Materials;

    public SceneNode? Root;
}