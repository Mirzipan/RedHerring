using System.Globalization;
using ImGuiNET;
using RedHerring.Render.ImGui;
using RedHerring.Studio.Models;
using RedHerring.Studio.Models.ViewModels.Console;
using RedHerring.Studio.UserInterface;
using Gui = ImGuiNET.ImGui;

namespace RedHerring.Studio.Tools;

[Tool(ToolName)]
public sealed class ToolConsole : Tool
{
    public const string ToolName = FontAwesome6.Terminal + " Console";
    
    protected override string Name => ToolName;

    private Menu _contextMenu = new(MenuStyle.ContextMenu);
    private string _selectedLine = string.Empty;

    private bool _showTimestamps = true;
    private bool _showInfo = true;
    private bool _showSuccess = true;
    private bool _showWarning = true;
    private bool _showError = true;
    private bool _showException = true;

    public ToolConsole(StudioModel studioModel, int uniqueId) : base(studioModel, uniqueId)
    {
        PopulateContextMenu();
    }

    public override void Update(out bool finished)
    {
        finished = Draw();
        _contextMenu.InvokeClickActions();
    }

    private bool Draw()
    {
        bool isOpen = true;
        
        if (Gui.Begin(NameId, ref isOpen, ImGuiWindowFlags.HorizontalScrollbar))
        {
            DrawSettings();

            Gui.SeparatorText("Messages");
            
            for (int i = 0; i < StudioModel.Console.Count; ++i)
            {
                ConsoleItem item = StudioModel.Console[i];
                switch (item.Kind)
                {
                    case ConsoleItemKind.Info:
                        if (!_showInfo)
                        {
                            continue;
                        }
                        break;
                    case ConsoleItemKind.Success:
                        if (!_showSuccess)
                        {
                            continue;
                        }
                        break;
                    case ConsoleItemKind.Warning:
                        if (!_showWarning)
                        {
                            continue;
                        }
                        break;
                    case ConsoleItemKind.Error:
                        if (!_showError)
                        {
                            continue;
                        }
                        break;
                    case ConsoleItemKind.Exception:
                        if (!_showException)
                        {
                            continue;
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                var color = item.Kind.ToColor();

                if (_showTimestamps)
                {
                    Gui.TextColored(color, item.TimeStamp.ToLongTimeString());
                    Gui.SameLine();
                }

                string id = item.TimeStamp.ToString(CultureInfo.InvariantCulture);
                Gui.PushID(id);
                Gui.TextColored(color, item.Message);
                Gui.PopID();

                if (Gui.BeginPopupContextItem(id))
                {
                    _selectedLine = item.Message;
                    _contextMenu.Update();
                    Gui.EndPopup();
                }
            }

            Gui.End();
        }

        return !isOpen;
    }

    private void DrawSettings()
    {
        Gui.BeginGroup();
        Gui.Checkbox("Timestamps", ref _showTimestamps);
        Gui.SameLine();
        Gui.Checkbox("Info", ref _showInfo);
        Gui.SameLine();
        Gui.Checkbox("Success", ref _showSuccess);
        Gui.SameLine();
        Gui.Checkbox("Warning", ref _showWarning);
        Gui.SameLine();
        Gui.Checkbox("Error", ref _showError);
        Gui.SameLine();
        Gui.Checkbox("Exception", ref _showException);
        Gui.EndGroup();
    }

    private void PopulateContextMenu()
    {
        _contextMenu.AddItem("Copy", CopySelectedLineToClipboard);
    }

    private void CopySelectedLineToClipboard()
    {
        Gui.SetClipboardText(_selectedLine);
    }
}