using RedHerring.Studio.Import;

namespace RedHerring.Studio.UserInterface.Editor;

internal class SceneDescription
{
    private readonly string[] _meshNames;
    private readonly string[] _materialNames;
    private readonly string[] _animationNames;
    private readonly string _rootName;

    public string[] MeshNames => _meshNames;
    public string[] MaterialNames => _materialNames;
    public string[] AnimationNames => _animationNames;
    
    public int MeshCount => _meshNames.Length;
    public int MaterialCount => _materialNames.Length;
    public int AnimationCount => _animationNames.Length;
    public string RootName => _rootName;
    
    public SceneDescription(SceneImporterSettings settings)
    {
        _meshNames = settings.Meshes.Select(e => e.Name).ToArray();
        _materialNames = settings.Materials.Select(e => e.Name).ToArray();
        _animationNames = settings.Animations.Select(e => e.Name).ToArray();
        _rootName = settings.Root.Name;
    }

    public SceneDescription(AssimpSceneImporterSettings settings)
    {
        _meshNames = settings.Meshes.Select(e => e.Name).ToArray();
        _materialNames = settings.Materials.Select(e => e.Name).ToArray();
        _animationNames = settings.Animations.Select(e => e.Name).ToArray();
        _rootName = settings.Root.Name;
    }
}