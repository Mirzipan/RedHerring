using System.Collections;
using ImGuiNET;
using Gui = ImGuiNET.ImGui;

using System.Reflection;
using RedHerring.Studio.UserInterface.Attributes;

namespace RedHerring.Studio.UserInterface;

public sealed class InspectorListControl : AnInspectorControl
{
	private bool                     _isReadOnly = false;
	private List<AnInspectorControl> _controls   = new(); // control per item

	public InspectorListControl(Inspector inspector, string id) : base(inspector, id)
	{
		
	}

	public override void InitFromSource(object? sourceOwner, object source, FieldInfo? sourceField = null)
	{
		base.InitFromSource(sourceOwner, source, sourceField);
		
		_isReadOnly = sourceField != null && (sourceField.IsInitOnly || sourceField.GetCustomAttribute<ReadOnlyInInspectorAttribute>() != null);
	}

	public override void AdaptToSource(object? sourceOwner, object source, FieldInfo? sourceField = null)
	{
		base.AdaptToSource(sourceOwner, source, sourceField);

		_isReadOnly |= sourceField != null && (sourceField.IsInitOnly || sourceField.GetCustomAttribute<ReadOnlyInInspectorAttribute>() != null);
	}
	
	public override void Update()
	{
		InspectorValueBinding valueBinding = ValueBindings.First();
		
		object? value = valueBinding.SourceField?.GetValue(valueBinding.Source);
		if (value == null)
		{
			return;
		}

		if (value is IList list)
		{
			if (Gui.CollapsingHeader(LabelId, ImGuiTreeNodeFlags.DefaultOpen))
			{
				Gui.Indent();
			
				//foreach (AnInspectorControl control in _controls)
				for(int i = 0; i < list.Count; ++i)
				{
					
					
					// if (Gui.TreeNode($"Item {i}"))
					// {
					// 	Gui.TreePop();
					// }

					
				}
				Gui.Unindent();
			}

			return;
		}

		if (value is Array array)
		{
			if (Gui.CollapsingHeader(LabelId, ImGuiTreeNodeFlags.DefaultOpen))
			{
				Gui.Indent();

				//foreach (AnInspectorControl control in _controls)
				for (int i = 0; i < array.Length; ++i)
				{
					// if (Gui.TreeNode($"Item {i}"))
					// {
					// 	Gui.TreePop();
					// }


				}

				Gui.Unindent();
			}
		}
	}
}