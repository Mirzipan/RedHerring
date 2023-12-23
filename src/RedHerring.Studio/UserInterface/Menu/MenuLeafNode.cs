using Gui = ImGuiNET.ImGui;

namespace RedHerring.Studio.UserInterface;

public sealed class MenuLeafNode : MenuNode
{
	private readonly string      _shortcut; // TODO - this should be generated from input shortcut
	private readonly Action      _onClick;
	private readonly Func<bool>? _isEnabled;
	private readonly Func<bool>? _isChecked;

	public override void Update()
	{
		if (Gui.MenuItem(Name, _shortcut, _isChecked?.Invoke() ?? false, _isEnabled?.Invoke() ?? true))
		{
			_onClick.Invoke();
		}
	}

	public MenuLeafNode(
		string name,
		string shortcut,
		Action onClick,
		Func<bool>? isEnabled,
		Func<bool>? isChecked
	)
		: base(name)
	{
		_shortcut  = shortcut;
		_onClick   = onClick;
		_isEnabled = isEnabled;
		_isChecked = isChecked;
	}
}