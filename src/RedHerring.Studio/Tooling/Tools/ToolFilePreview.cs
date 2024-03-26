using RedHerring.Render.ImGui;
using RedHerring.Studio.Models;
using RedHerring.Studio.Models.ViewModels;
using RedHerring.Studio.UserInterface.Editor;
using Gui = ImGuiNET.ImGui;

namespace RedHerring.Studio.Tools;

[Tool(ToolName)]
public class ToolFilePreview : Tool
{
    public const string ToolName = FontAwesome6.Eye + " Preview";

    private readonly StudioModel _studioModel;
    private readonly FilePreview _preview;
    private bool _subscribedToChange = false;

    protected override string Name => ToolName;

    public ToolFilePreview(StudioModel studioModel, int uniqueId) : base(studioModel, uniqueId)
    {
        _studioModel = studioModel;
        _preview = new FilePreview(studioModel.CommandHistory);
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
            _preview.Update();
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
        _preview.Init(StudioModel.Selection.GetAllSelectedTargets());
    }
}