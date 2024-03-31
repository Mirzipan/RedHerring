namespace RedHerring.Studio.UserInterface.Editor;

internal class SceneDescription
{
    private readonly SceneImporterSettings _settings;

    private readonly string[] _meshNames;
    private readonly string[] _materialNames;
    private readonly string[] _animationNames;

    public string[] MeshNames => _meshNames;
    public string[] MaterialNames => _materialNames;
    public string[] AnimationNames => _animationNames;
    
    public int MeshCount => _settings.Meshes.Count;
    public int MaterialCount => _settings.Materials.Count;
    public int AnimationCount => _settings.Animations.Count;
    public string RootName => _settings.Root.Name;
    
    public SceneDescription(SceneImporterSettings settings)
    {
        _settings = settings;

        _meshNames = settings.Meshes.Select(e => e.Name).ToArray();
        _materialNames = settings.Materials.Select(e => e.Name).ToArray();
        _animationNames = settings.Animations.Select(e => e.Name).ToArray();
    }
}