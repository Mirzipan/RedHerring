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
    private FileKind _kind;

    private IntPtr _textureBinding;
    private SceneDescription? _sceneDescription;

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

        switch (source)
        {
            case ProjectScriptFileNode scriptFile:
                LoadScriptFile(scriptFile);
                break;
            case ProjectAssetFileNode assetFile:
                LoadAssetFile(assetFile);
                break;
        }

        Rebuild();
    }

    public void Init(IReadOnlyList<ISelectable> sources)
    {
        Clear();

        for (int i = 0; i < sources.Count; i++)
        {
            var source = sources[i];
            switch (source)
            {
                case ProjectScriptFileNode scriptFile:
                    LoadScriptFile(scriptFile);
                    goto rebuild;
                case ProjectAssetFileNode assetFile:
                    LoadAssetFile(assetFile);
                    goto rebuild;
            }
        }

        rebuild:
        Rebuild();
    }

    public void Update()
    {
        switch (_kind)
        {
            case FileKind.Unknown:
                PlaintextFile.Draw(_lines);
                break;
            case FileKind.PlainText:
                PlaintextFile.Draw(_lines);
                break;
            case FileKind.Markdown:
                MarkdownFile.Draw(_lines);
                break;
            case FileKind.CSharp:
                CSharpFile.Draw(_lines);
                break;
            case FileKind.Texture:
                if (_textureBinding != IntPtr.Zero)
                {
                    TextureFile.Draw(_textureBinding);
                }
                break;
            case FileKind.Scene:
                if (_sceneDescription is not null)
                {
                    SceneFile.Draw(_sceneDescription);
                }
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
        ClearSceneDescription();
    }

    private void ClearTextureBinding()
    {
        if (_textureBinding != IntPtr.Zero)
        {
            ImGuiProxy.RemoveImGuiBinding(_textureBinding);
            _textureBinding = IntPtr.Zero;
        }
    }

    private void ClearSceneDescription()
    {
        if (_sceneDescription is not null)
        {
            _sceneDescription = null;
        }
    }

    private void LoadScriptFile(ProjectScriptFileNode node)
    {
        var result = node.LoadFile();
        if (result.Code == ProjectScriptFileNode.LoadingResultCode.Ok)
        {
            _lines.AddRange(result.Lines!);
            if (CSharpFile.HasExtension(node.Extension))
            {
                _kind = FileKind.CSharp;
            }
            else if (MarkdownFile.HasExtension(node.Extension))
            {
                _kind = FileKind.Markdown;
            }
            else
            {
                _kind = FileKind.PlainText;
            }
        }
        else
        {
            _kind = FileKind.Unknown;
        }

        _filePath = node.RelativePath;
    }

    private void LoadAssetFile(ProjectAssetFileNode node)
    {
        if (TextureFile.HasExtension(node.Extension))
        {
            ClearTextureBinding();
        
            _textureBinding = ImGuiProxy.GetOrCreateImGuiBinding(node.AbsolutePath);
            
            _kind = FileKind.Texture;
            _filePath = node.RelativePath;
            return;
        }

        if (SceneFile.HasExtension(node.Extension))
        {
            if (node.Meta?.ImporterSettings is not SceneImporterSettings)
            {
                return;
            }

            _sceneDescription = new SceneDescription((node.Meta.ImporterSettings as SceneImporterSettings)!);
            _kind = FileKind.Scene;
            _filePath = node.RelativePath;
            return;
        }
    }

    private void Rebuild()
    {
    }

    #endregion
}