using RedHerring.Deduction;
using RedHerring.Studio.Models.Project.FileSystem;

namespace RedHerring.Studio.Models.Project.Importers;

[AttributeIndexer(typeof(ImporterAttribute))]
public sealed class ImporterRegistry : AttributeIndexer
{
	private readonly Dictionary<ProjectNodeType, Type> _types    = new();
	private readonly Type                              _fallback = typeof(CopyImporter);

	public Importer CreateImporter(ProjectNode node)
	{
		Type   importerType = _types.TryGetValue(node.Type, out Type? type) ? type : _fallback;
		object instance     = Activator.CreateInstance(importerType, node)!;
		return (Importer) instance;
	}
	
	public void Index(Attribute attribute, Type type)
	{
		if (attribute is ImporterAttribute importerAttribute)
		{
			_types.Add(importerAttribute.NodeType, type);
		}
	}
}