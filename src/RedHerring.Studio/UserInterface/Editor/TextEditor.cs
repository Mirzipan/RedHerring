using RedHerring.Studio.Commands;
using RedHerring.Studio.Models.Project.FileSystem;
using RedHerring.Studio.Models.ViewModels;

namespace RedHerring.Studio.UserInterface.Editor;

public class TextEditor
{
    private readonly List<string> _lines = new();
    private TextFileKind _kind;

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

        switch (_kind)
        {
            case TextFileKind.Unknown:
                PlaintextFile.Draw(_lines);
                break;
            case TextFileKind.PlainText:
                PlaintextFile.Draw(_lines);
                break;
            case TextFileKind.Markdown:
                MarkdownFile.Draw(_lines);
                break;
            case TextFileKind.CSharp:
                CSharpFile.Draw(_lines);
                break;
            default:
                PlaintextFile.Draw(_lines);
                break;
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
            if (node.Extension == ".cs")
            {
                _kind = TextFileKind.CSharp;
            }
            else if (node.Extension == ".md" || node.Extension == ".markdown")
            {
                _kind = TextFileKind.Markdown;
            }
            else
            {
                _kind = TextFileKind.PlainText;
            }
        }
        else
        {
            _kind = TextFileKind.Unknown;
        }
    }

    private void Rebuild()
    {
    }

    #endregion
}