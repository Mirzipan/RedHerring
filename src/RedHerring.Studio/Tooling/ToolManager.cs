using RedHerring.Deduction;
using RedHerring.Studio.Models;
using RedHerring.Studio.Systems;

namespace RedHerring.Studio.Tools;

[AttributeIndexer(typeof(ToolAttribute))]
public sealed class ToolManager : IIndexAttributes
{
	private readonly Dictionary<string, Type> _toolsByName = new();
	private readonly List<Tool>              _activeTools = new();

	private StudioModel  _studioModel;
	private StudioSystem _studioSystem;
	
	public void Init(StudioModel studioModel, StudioSystem studioSystem)
	{
		_studioModel  = studioModel;
		_studioSystem = studioSystem;
	}

	public Tool? Activate(string toolName, int uniqueId = -1)
	{
		Tool? tool = (Tool?) Activator.CreateInstance(_toolsByName[toolName], _studioModel, uniqueId); // if uniqueId == -1, new id is generated in constructor
		if (tool == null)
		{
			return null;
		}

		//_studioSystem.Resove(); TODO
		
		_activeTools.Add(tool);
		return tool;
	}

	public Tool? Get(string toolName) // TODO - generic if used more than once
	{
		Type toolType = _toolsByName[toolName];
		return _activeTools.FirstOrDefault(tool => tool.GetType() == toolType);
	}

	public void Index(Attribute attribute, Type type)
	{
		_toolsByName.Add(((ToolAttribute)attribute).Name, type);
	}

	public void Update()
	{
		for(int i=0;i <_activeTools.Count;++i)
		{
			_activeTools[i].Update(out bool finished);
			if (finished)
			{
				_activeTools.RemoveAt(i);
				--i;
			}
		}
	}

	#region Import/Export
	public List<ToolId> ExportActiveTools()
	{
		return _activeTools.Select(tool => tool.Id).ToList();
	}

	public void ImportActiveTools(List<ToolId>? toolIds)
	{
		_activeTools.Clear();

		if (toolIds == null)
		{
			return;
		}

		foreach (ToolId id in toolIds)
		{
			if (_toolsByName.ContainsKey(id.Name))
			{
				Activate(id.Name, id.UniqueId);
			}
		}
	}
	#endregion
}