using RedHerring.Render.ImGui;
using RedHerring.Studio.Commands;
using RedHerring.Studio.Models.Project.FileSystem;
using RedHerring.Studio.Models.ViewModels;

namespace RedHerring.Studio.UserInterface.Editor;

public class FilePreview
{
    private readonly List<string> _lines = new();
    private TextFileKind _kind;

    private IntPtr _textureBinding;

    private static int _uniqueIdGenerator = 0;
    private int _uniqueId = _uniqueIdGenerator++;

    private CommandHistory _commandHistory;

    public FilePreview(CommandHistory commandHistory)
    {
        _commandHistory = commandHistory;
    }

    public void Init(object? source)
    {
        _lines.Clear();
        ClearTextureBinding();
        
        if (source is ProjectScriptFileNode scriptFile)
        {
            LoadScriptFile(scriptFile);
        }

        if (source is ProjectAssetFileNode assetFile)
        {
            LoadAssetFile(assetFile);
        }

        Rebuild();
    }

    public void Init(IReadOnlyList<ISelectable> sources)
    {
        _lines.Clear();
        ClearTextureBinding();

        for (int i = 0; i < sources.Count; i++)
        {
            var source = sources[i];
            if (source is ProjectScriptFileNode scriptFile)
            {
                LoadScriptFile(scriptFile);
                goto rebuild;
            }

            if (source is ProjectAssetFileNode assetFile)
            {
                LoadAssetFile(assetFile);
                goto rebuild;
            }
        }

        rebuild:
        Rebuild();
    }

    private void ClearTextureBinding()
    {
        if (_textureBinding != IntPtr.Zero)
        {
            ImGuiProxy.RemoveImGuiBinding(_textureBinding);
            _textureBinding = IntPtr.Zero;
        }
    }

    public void Update()
    {
        if (_lines.Count == 0)
        {
            if (_textureBinding == IntPtr.Zero)
            {
                return;
            }
            
            TextureFile.Draw(_textureBinding);
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

    private void LoadAssetFile(ProjectAssetFileNode node)
    {
        if (!TextureFile.IsTexture(node.Extension))
        {
            return;
        }
        
        ClearTextureBinding();
        
        _textureBinding = ImGuiProxy.GetOrCreateImGuiBinding(node.AbsolutePath);
    }

    private void Rebuild()
    {
    }

    #endregion
}