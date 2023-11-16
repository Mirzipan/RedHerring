using RedHerring.Alexandria.Collections;
using RedHerring.Deduction;

namespace RedHerring.Studio.Models.Project.Importers;

[AttributeIndexer(typeof(ImporterAttribute))]
[AttributeIndexer(typeof(ImporterProcessorAttribute))]
public sealed class ImporterRegistry : IIndexAttributes
{
	private Dictionary<string, Importer> _importers        = new();
	private Importer                     _fallbackImporter = new CopyImporter();

	private ListDictionary<Type, ImporterProcessor> _processors = new();
	private List<ImporterProcessor> 			   _fallbackProcessors = new() {new CopyImporterProcessor()};

	public Importer GetImporter(string extension)
	{
		if (_importers.TryGetValue(extension, out Importer? importer))
		{
			return importer;
		}

		return _fallbackImporter;
	}
	
	public List<ImporterProcessor> GetProcessors(Type type)
	{
		if(_processors.TryGet(type, out List<ImporterProcessor>? processors))
		{
			return processors;
		}
		
		return _fallbackProcessors;
	}

	public void Index(Attribute attribute, Type type)
	{
		if (attribute is ImporterAttribute importerAttribute)
		{
			Importer importer = (Importer) Activator.CreateInstance(type)!;
			foreach (string extension in importerAttribute.Extensions)
			{
				_importers.Add(extension, importer);
			}

			return;
		}

		if (attribute is ImporterProcessorAttribute importerProcessorAttribute)
		{
			ImporterProcessor importerProcessor = (ImporterProcessor) Activator.CreateInstance(type)!;
			_processors.Add(importerProcessorAttribute.ProcessedType, importerProcessor);
		}
	}
}