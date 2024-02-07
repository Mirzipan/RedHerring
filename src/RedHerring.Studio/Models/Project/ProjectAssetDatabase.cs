using RedHerring.Alexandria.Extensions.Collections;

namespace RedHerring.Studio.Models.Project;

public sealed class ProjectAssetDatabase
{
	private class Item
	{
		public string Id;
		public string Field;
		public string RelativeResourcePath;
	}

	private List<Item> _items = new();

	public static readonly Dictionary<string, string> Assets = new()
	                                                           {
		                                                           {"asdf1", "asdf"},
		                                                           {"asdf2", "asdf"},
		                                                           {"asdf3", "asdf"},
	                                                           };

	public void AddItem(string id, string field, string relativeResourcePath)
	{
		_items.Add(new Item {Id = id, Field = field, RelativeResourcePath = relativeResourcePath});
	}

	public void Write(string path)
	{
		using StreamWriter stream = File.CreateText(path);
		stream.WriteLine("// this file is generated");
		stream.WriteLine("public sealed class AssetDatabase");
		stream.WriteLine("{");

		// fields
		foreach (Item item in _items)
		{
			if (!item.Field.IsNullOrEmpty())
			{
				stream.WriteLine($"	public const string {item.Field}=\"{item.RelativeResourcePath}\";");
			}
		}
		
		// dictionary
		stream.WriteLine("	public static readonly Dictionary<string, string> Assets = new()");
		stream.WriteLine("		{");
		foreach (Item item in _items)
		{
			stream.WriteLine($"			{{@\"{item.Id}\",@\"{item.RelativeResourcePath}\"}},");
		}
		stream.WriteLine("		};");

		stream.WriteLine("}");
	}
}