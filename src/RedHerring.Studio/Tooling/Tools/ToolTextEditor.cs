using RedHerring.Render.ImGui;
using RedHerring.Studio.Models;
using RedHerring.Studio.Models.ViewModels;
using RedHerring.Studio.UserInterface.Editor;
using Gui = ImGuiNET.ImGui;

namespace RedHerring.Studio.Tools;

[Tool(ToolName)]
public class ToolTextEditor : Tool
{
    public const string ToolName = FontAwesome6.Pencil + " Text Editor";

    private readonly StudioModel _studioModel;
    private readonly TextEditor _editor;
    private bool _subscribedToChange = false;

    protected override string Name => ToolName;

    public ToolTextEditor(StudioModel studioModel, int uniqueId) : base(studioModel, uniqueId)
    {
        _studioModel = studioModel;
        _editor = new TextEditor(studioModel.CommandHistory);
    }

    public override void Update(out bool finished)
    {
        finished = UpdateUI();
    }

    private bool UpdateUI()
    {
        bool isOpen = true;
        if (Gui.Begin(NameId, ref isOpen))
        {
            SubscribeToChange();
            _editor.Update();
            ApplyChanges();
            Gui.End();
        }
        else
        {
            UnsubscribeFromChange();
        }

        return !isOpen;
    }

    private void ApplyChanges()
    {
        if (!_studioModel.CommandHistory.WasChange)
        {
            return;
        }

        IReadOnlyList<ISelectable> selection = StudioModel.Selection.GetAllSelectedTargets();
        foreach (ISelectable selectable in selection)
        {
            selectable.ApplyChanges();
        }

        _studioModel.CommandHistory.ResetChange();
    }

    private void SubscribeToChange()
    {
        if (_subscribedToChange)
        {
            return;
        }

        StudioModel.Selection.SelectionChanged += OnSelectionChanged;
        _subscribedToChange = true;
        OnSelectionChanged();
    }

    private void UnsubscribeFromChange()
    {
        if (!_subscribedToChange)
        {
            return;
        }

        StudioModel.Selection.SelectionChanged -= OnSelectionChanged;
        _subscribedToChange = false;
    }

    private void OnSelectionChanged()
    {
        _editor.Init(StudioModel.Selection.GetAllSelectedTargets());
    }
}