using Gui = ImGuiNET.ImGui;

namespace RedHerring.Studio.UserInterface;

public abstract class InspectorSingleInputControl<T> : InspectorEditControl<T>
{
	protected InspectorSingleInputControl(Inspector inspector, string id) : base(inspector, id)
	{
	}

	public sealed override void Update()
	{
		bool isItemActive = false;
		if(_multipleValues)
		{
			if (!GuiMultiEditButton())
			{
				return;
			}

			_multipleValues = false;
			isItemActive    = true;
		}

		BeginReadOnlyStyle();
		bool submit = InputControl(isItemActive);
		EndReadOnlyStyle();

		SubmitOrUpdateValue(submit, isItemActive || Gui.IsItemActive());
	}

	// create single input control, returns true if value was submitted (not after every change!)
	protected abstract bool InputControl(bool makeItemActive);
}