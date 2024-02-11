using RedHerring.Studio.Commands;
using RedHerring.Studio.Models.Project.FileSystem;
using RedHerring.Studio.Models.ViewModels;
using Gui = ImGuiNET.ImGui;

namespace RedHerring.Studio.UserInterface.Editor;

public class TextEditor
{
    private readonly List<string> _lines = new();

    private static int _uniqueIdGenerator = 0;
    private int _uniqueId = _uniqueIdGenerator++;

    private CommandHistory _commandHistory;

    public TextEditor(CommandHistory commandHistory)
    {
        _commandHistory = commandHistory;
    }

    public void Init(object? source)
    {
        _lines.Clear();
        
        if (source is ProjectScriptFileNode scriptFile)
        {
            LoadScriptFile(scriptFile);
        }

        Rebuild();
    }

    public void Init(IReadOnlyList<ISelectable> sources)
    {
        _lines.Clear();

        for (int i = 0; i < sources.Count; i++)
        {
            var source = sources[i];
            if (source is ProjectScriptFileNode scriptFile)
            {
                LoadScriptFile(scriptFile);
                goto rebuild;
            }
        }

        rebuild:
        Rebuild();
    }
    
    public void Update()
    {
        if (_lines.Count == 0)
        {
            return;
        }

        for (int i = 1; i < _lines.Count; i++)
        {
            string line = _lines[i];
            Gui.Text(line);
        }
    }

    public void Commit(Command command)
    {
        _commandHistory.Commit(command);
    }

    #region Private

    private void LoadScriptFile(ProjectScriptFileNode node)
    {
        var result = node.LoadFile();
        if (result.Code == ProjectScriptFileNode.LoadingResultCode.Ok)
        {
            _lines.AddRange(result.Lines!);
        }
    }

    private void Rebuild()
    {
    }

    #endregion
}