using Vortice.Mathematics;

namespace RedHerring.Render.Models;

[Serializable]
public class Mesh
{
    public string Name;
    public int MaterialIndex;
    public BoundingBox BoundingBox;
    public BoundingSphere BoundingSphere;

    public int IndexStart;
    public int TriStart;
    public int TriCount;
}