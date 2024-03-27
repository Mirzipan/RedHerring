namespace RedHerring.Studio.UserInterface.Editor;

internal class SceneDescription
{
    private readonly SceneImporterSettings _settings;

    public int MeshCount => _settings.Meshes.Count;
    public int MaterialCount => _settings.Materials.Count;
    public string RootName => _settings.Root.Name;
    
    public SceneDescription(SceneImporterSettings settings)
    {
        _settings = settings;
    }
}