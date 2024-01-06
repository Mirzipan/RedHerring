using ImGuiNET;
using Gui = ImGuiNET.ImGui;

namespace RedHerring.Studio.UserInterface;

public sealed class InspectorStringControl : InspectorSingleInputControl<string>
{
	private const int MaxLength = 1024; // TODO - maybe from some attribute?
	
	public InspectorStringControl(IInspectorCommandTarget commandTarget, string id) : base(commandTarget, id)
	{
	}

	protected override bool InputControl(bool makeItemActive)
	{
		if (makeItemActive)
		{
			Gui.SetKeyboardFocusHere();
		}

		Value ??= "";

		Gui.InputText(LabelId, ref Value, MaxLength, _isReadOnly ? ImGuiInputTextFlags.ReadOnly : ImGuiInputTextFlags.None);
		return Gui.IsItemDeactivatedAfterEdit();
	}
}