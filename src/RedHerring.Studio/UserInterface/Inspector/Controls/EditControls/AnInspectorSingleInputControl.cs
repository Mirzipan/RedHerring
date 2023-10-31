using Gui = ImGuiNET.ImGui;

namespace RedHerring.Studio.UserInterface;

public abstract class AnInspectorSingleInputControl<T> : AnInspectorEditControl<T>
{
	protected AnInspectorSingleInputControl(Inspector inspector, string id) : base(inspector, id)
	{
	}

	public sealed override void Update()
	{
		Gui.AlignTextToFramePadding();
		Gui.TextUnformatted(Label);
		Gui.SameLine();

		bool isItemActive = false;
		if(_multipleValues)
		{
			if (!GuiMultiEditButton())
			{
				return;
			}

			_multipleValues = false;
			isItemActive    = true;
			Gui.SetKeyboardFocusHere(); // focus next control
		}

		BeginReadOnlyStyle();
		InputControl();
		EndReadOnlyStyle();

		SubmitOrUpdateValue(Gui.IsItemDeactivatedAfterEdit(), isItemActive || Gui.IsItemActive());
	}

	protected abstract void InputControl();
}