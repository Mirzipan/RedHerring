using Assimp;

namespace RedHerring.Studio.Models.Project.Importers;

public class SceneIntermediate
{
    public readonly Assimp.Scene Scene;

    public SceneIntermediate(Scene scene)
    {
        Scene = scene;
    }
}