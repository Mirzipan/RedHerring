﻿namespace RedHerring.Studio.Models.Project.Importers;

public sealed class ImporterRegistry
{
	private static readonly Type GenericImporterType = typeof(AssetImporter<>);

	private readonly record struct Entry(Importer Importer, Type OutputType);
	
	private Dictionary<string, List<Importer>> _importers         = new();
	private List<Importer>                     _fallbackImporters = new() {new CopyImporter()};

	private Dictionary<string, Entry> _byExtension = new();

	public ImporterRegistry()
	{
		
		// TODO DEBUG - this will be replaced by attribute scan
		SceneImporter sceneImporter = new();
		_importers.Add("fbx", new List<Importer>{sceneImporter});
		_importers.Add("obj", new List<Importer>{sceneImporter});
	}
	
	public List<Importer> GetImporters(string extension)
	{
		if (_importers.TryGetValue(extension, out List<Importer>? importers))
		{
			return importers;
		}

		return _fallbackImporters;
	}

	public void Register<T>(T importer, params string[] extensions) where T : Importer
	{
		var type = typeof(T);
		var baseType = GetGenericImporterType(type);
		if (baseType is null)
		{
			return;
		}

		foreach (string extension in extensions)
		{
			if (_importers.ContainsKey(extension))
			{
				// TODO: log?
			}

			_byExtension[extension] = new Entry(importer, baseType!.GetGenericArguments()[0]);
		}
	}

	public void Unregister(string extension) => _byExtension.Remove(extension);

	public Importer Find(string extension)
	{
		return Find(extension, out _);
	}

	public Importer Find(string extension, out Type? outputType)
	{
		outputType = _byExtension.TryGetValue(extension, out var entry) ? entry.OutputType : null;
		return entry.Importer;
	}

	private static Type? GetGenericImporterType(Type type)
	{
		Type? current = type;
		while (current is not null)
		{
			if (current.IsGenericType && current.GetGenericTypeDefinition() == GenericImporterType)
			{
				return current;
			}

			current = current.BaseType;
		}

		return null;
	}
}