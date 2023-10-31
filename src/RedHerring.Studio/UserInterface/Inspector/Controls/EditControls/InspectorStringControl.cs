using ImGuiNET;
using Gui = ImGuiNET.ImGui;

namespace RedHerring.Studio.UserInterface;

public sealed class InspectorStringControl : AnInspectorSingleInputControl<string>
{
	private const int MaxLength = 1024; // TODO - maybe from some attribute?
	
	public InspectorStringControl(Inspector inspector, string id) : base(inspector, id)
	{
	}

	protected override bool InputControl()
	{
		Gui.InputText(Id, ref Value, MaxLength, _isReadOnly ? ImGuiInputTextFlags.ReadOnly : ImGuiInputTextFlags.None);
		return Gui.IsItemDeactivatedAfterEdit();
	}
}