using ImGuiNET;
using Gui = ImGuiNET.ImGui;

namespace RedHerring.Studio.UserInterface;

public sealed class InspectorStringControl : AnInspectorSingleInputControl<string>
{
	private const int MaxLength = 1024; // TODO - maybe from some attribute?
	
	public InspectorStringControl(Inspector inspector, string id) : base(inspector, id)
	{
	}

	protected override void GuiInputControl()
	{
		string? localValue = Value;
		Gui.InputText(Id, ref localValue, MaxLength, _isReadOnly ? ImGuiInputTextFlags.ReadOnly : ImGuiInputTextFlags.None);
		Value = localValue;
	}
}