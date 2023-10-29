namespace RedHerring.Studio.Models.Project.Importers;

public sealed class ImporterSelector
{
	private Dictionary<string, List<AnImporter>> _importers         = new();
	private List<AnImporter>                     _fallbackImporters = new() {new CopyImporter()};
	
	public ImporterSelector()
	{
		
		// TODO DEBUG - this will be replaced by attribute scan
		SceneImporter sceneImporter = new();
		_importers.Add("fbx", new List<AnImporter>{sceneImporter});
		_importers.Add("obj", new List<AnImporter>{sceneImporter});
	}
	
	public List<AnImporter> GetImporters(string extension)
	{
		if (_importers.TryGetValue(extension, out List<AnImporter>? importers))
		{
			return importers;
		}

		return _fallbackImporters;
	}
}