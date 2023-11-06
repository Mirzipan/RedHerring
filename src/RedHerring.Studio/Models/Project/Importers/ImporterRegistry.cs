using RedHerring.Deduction;

namespace RedHerring.Studio.Models.Project.Importers;

[AttributeIndexer(typeof(ImporterAttribute))]
public sealed class ImporterRegistry : IIndexAttributes
{
	private Dictionary<string, Importer> _importers        = new();
	private Importer                     _fallbackImporter = new CopyImporter();

	public Importer GetImporter(string extension)
	{
		if (_importers.TryGetValue(extension, out Importer? importer))
		{
			return importer;
		}

		return _fallbackImporter;
	}

	public void Index(Attribute attribute, Type type)
	{
		ImporterAttribute importerAttribute = (ImporterAttribute) attribute;
		Importer          importer          = (Importer) Activator.CreateInstance(type)!;
		foreach (var extension in importerAttribute.Extensions)
		{
			_importers.Add(extension, importer);
		}
	}
}