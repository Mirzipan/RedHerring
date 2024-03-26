using RedHerring.Studio.Models;

namespace RedHerring.Studio;

public sealed class StudioAssetDatabase
{
	private readonly Dictionary<string, StudioAssetDatabaseItem> _items = new();
	private          bool                                        _dirty = false;
	public           bool                                        IsDirty => _dirty;

	public StudioAssetDatabaseItem? this[string key]
	{
		get => _items.TryGetValue(key, out StudioAssetDatabaseItem? path) ? path : null;
		set
		{
			if (value is null)
			{
				_items.Remove(key);
			}
			else
			{
				_items[key] = value;
			}

			_dirty = true;
		}
	}

	#region IO Manipulation
	public void Save(ProjectSettings projectSettings)
	{
		string path = Path.Join(projectSettings.AbsoluteScriptsPath, projectSettings.AssetDatabaseSourcePath);

		{
			using FileStream   stream = File.Open(path, FileMode.Create);
			using StreamWriter writer = new(stream);

			writer.WriteLine("// this file is generated in RedHerring Studio");
			writer.WriteLine("using RedHerring.Assets;");
			writer.WriteLine($"namespace {projectSettings.AssetDatabaseNamespace};");
			writer.WriteLine($"public static class {projectSettings.AssetDatabaseClass}");
			writer.WriteLine("{");
			
			foreach (StudioAssetDatabaseItem item in _items.Values)
			{
				if (item.Field == null)
				{
					continue;
				}

				writer.WriteLine($"	public static {item.ReferenceType} {item.Field} = new(@\"{item.Path}\");");
			}

			writer.WriteLine();

			writer.WriteLine("	public static Dictionary<Guid, Reference> Assets = new() {");
			foreach (StudioAssetDatabaseItem item in _items.Values)
			{
				writer.WriteLine($"		{{new Guid(\"{item.Guid}\"),new {item.ReferenceType}(@\"{item.Path}\")}},");
			}
			writer.WriteLine("	};");
			
			writer.WriteLine("}");

			writer.Flush();
			stream.Flush();
		}
		
		_dirty = false;
	}
	#endregion
}