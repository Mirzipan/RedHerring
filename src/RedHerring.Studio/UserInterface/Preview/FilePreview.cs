using System.Numerics;
using ImGuiNET;
using RedHerring.Render.ImGui;
using RedHerring.Studio.Commands;
using RedHerring.Studio.Models.Project.FileSystem;
using RedHerring.Studio.Models.ViewModels;
using Gui = ImGuiNET.ImGui;

namespace RedHerring.Studio.UserInterface.Editor;

public class FilePreview
{
    private string? _filePath;
    
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
        Clear();

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
        Clear();

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

    public void Update()
    {
        if (_lines.Count == 0)
        {
            if (_textureBinding == IntPtr.Zero)
            {
                return;
            }

            DrawInfoBar();
            TextureFile.Draw(_textureBinding);
            return;
        }

        DrawInfoBar();

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

    #region Draw

    private void DrawInfoBar()
    {
        // var windowPos = Gui.GetWindowPos();
        // var windowSize = Gui.GetWindowSize();
        // var size = new Vector2(Gui.GetContentRegionAvail().X, 40);
        //
        // Gui.SetNextWindowPos(windowPos with { Y = windowPos.Y + windowSize.Y - 40 });
        // Gui.SetNextWindowSize(size);
        //
        // if (Gui.BeginChild("Status Bar", size, ImGuiChildFlags.None, ImGuiWindowFlags.HorizontalScrollbar))
        // {
        //     Gui.Text("Path:");
        //     Gui.SameLine();
        //     Gui.Text(_filePath);
        //     Gui.EndChild();
        // }
    }

    #endregion Draw
    
    #region Private
    
    private void Clear()
    {
        _filePath = null;
        _lines.Clear();
        
        ClearTextureBinding();
    }

    private void ClearTextureBinding()
    {
        if (_textureBinding != IntPtr.Zero)
        {
            ImGuiProxy.RemoveImGuiBinding(_textureBinding);
            _textureBinding = IntPtr.Zero;
        }
    }

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

        _filePath = node.RelativePath;
    }

    private void LoadAssetFile(ProjectAssetFileNode node)
    {
        if (!TextureFile.IsTexture(node.Extension))
        {
            return;
        }
        
        ClearTextureBinding();
        
        _textureBinding = ImGuiProxy.GetOrCreateImGuiBinding(node.AbsolutePath);
        
        _filePath = node.RelativePath;
    }

    private void Rebuild()
    {
    }

    #endregion
}