using ImGuiNET;
using RedHerring.Alexandria;
using RedHerring.Core;
using RedHerring.Core.Systems;
using RedHerring.Fingerprint;
using RedHerring.Fingerprint.Layers;
using RedHerring.Fingerprint.Shortcuts;
using RedHerring.Infusion.Attributes;
using RedHerring.Studio.Commands;
using RedHerring.Studio.UserInterface;
using RedHerring.Studio.UserInterface.Dialogs;
using NativeFileDialogSharp;
using RedHerring.ImGui;
using RedHerring.Studio.Models.Project;
using RedHerring.Studio.Models.Tests;
using RedHerring.Studio.TaskProcessor;
using RedHerring.Studio.Tools;
using Gui = ImGuiNET.ImGui;

namespace RedHerring.Studio.Systems;

// TODO: Add this to engine context when creating editor window.
public sealed class EditorSystem : AnEngineSystem, IUpdatable, IDrawable
{
    [Inject] private InputSystem    _inputSystem    = null!;
    [Inject] private GraphicsSystem _graphicsSystem = null!;

    public bool IsEnabled => true;
    public int UpdateOrder => int.MaxValue;

    public bool IsVisible => true;
    public int DrawOrder => int.MaxValue;

    private InputReceiver _inputReceiver;

    private          ProjectModel   _projectModel = new();
    private readonly CommandHistory _history      = new CommandHistory();

    private const    int                         _threadsCount  = 4;
    private readonly TaskProcessor.TaskProcessor _taskProcessor = new(_threadsCount);
    public           TaskProcessor.TaskProcessor TaskProcessor => _taskProcessor;

    private readonly List<ATool> _activeTools = new();
    
    #region User Interface
    private readonly DockSpace  _dockSpace  = new();
    private readonly Menu       _menu       = new();
    private readonly StatusBar  _statusBar  = new();
    private readonly MessageBox _messageBox = new();
    #endregion
    
    #region Lifecycle

    protected override void Init()
    {
        _inputReceiver = new InputReceiver("editor");
        _inputReceiver.ConsumesAllInput = false;
        
        _inputReceiver.Bind("undo", Undo);
        _inputReceiver.Bind("redo", Redo);
    }

    protected override void Load()
    {
        ImGuiNET.ImGui.GetIO().ConfigFlags |= ImGuiConfigFlags.DockingEnable;

        InitInput();

        InitMenu();

        // debug
        _activeTools.Add(new ToolProjectView(_projectModel));
    }

    protected override void Unload()
    {
        _taskProcessor.Cancel();
    }

    public void Update(GameTime gameTime)
    {
        _dockSpace.Update();
        
        _menu.Update();
        
        UpdateStatusBarMessage();
        _statusBar.Update();

        _messageBox.Update();

        for(int i=0;i<_activeTools.Count;++i)
        {
            _activeTools[i].Update(out bool finished);
            if (finished)
            {
                _activeTools.RemoveAt(i);
                --i;
            }
        }

        //Gui.ShowDemoWindow();
    }

    private void UpdateStatusBarMessage()
    {
        int workerThreadsCount = _taskProcessor.WorkerThreadsCount;
        int remainingTasks     = _taskProcessor.GetRemainingTasks();
        int availableThreads   = _taskProcessor.AvailableWorkerThreads;
		
        if (remainingTasks > 0)
        {
            _statusBar.Message = $"Processing {remainingTasks} tasks on {workerThreadsCount} threads.";
        }
        else
        {
            _statusBar.Message = $"Ready. {availableThreads} of {workerThreadsCount} threads available.";
        }
		
        _statusBar.MessageColor = remainingTasks == 0 && availableThreads == workerThreadsCount ? StatusBar.Color.Info : StatusBar.Color.Warning;
    }
    
    public bool BeginDraw() => true;

    public void Draw(GameTime gameTime)
    {
    }

    public void EndDraw()
    {
    }

    #endregion Lifecycle

    #region Private

    private void InitInput()
    {
        _inputSystem.Input.Bindings!.Add(new ShortcutBinding("undo", new KeyboardShortcut(Key.U)));
        _inputSystem.Input.Bindings!.Add(new ShortcutBinding("redo", new KeyboardShortcut(Key.Z)));
        _inputSystem.Input.Layers.Push(_inputReceiver);
    }

    private void InitMenu()
    {
        _menu.AddItem("File/Open project..", OnOpenProjectClicked);
        _menu.AddItem("File/Settings/Theme/Embrace the Darkness", UITheme.EmbraceTheDarkness);
        _menu.AddItem("File/Settings/Theme/Crimson Rivers", UITheme.CrimsonRivers);
        _menu.AddItem("File/Settings/Theme/Bloodsucker", UITheme.Bloodsucker);
        _menu.AddItem("File/Exit", OnExitClicked);

        _menu.AddItem("Edit/Undo", _history.Undo);
        _menu.AddItem("Edit/Redo", _history.Redo);

        _menu.AddItem("Debug/Modal window", () => Gui.OpenPopup("MessageBox"));
        _menu.AddItem("Debug/Task processor test", OnDebugTaskProcessorTestClicked);
        _menu.AddItem("Debug/Serialization test", OnDebugSerializationTestClicked);
        _menu.AddItem("Debug/Importer test", OnDebugImporterTestClicked);
    }

    #endregion Private

    #region Menu
    private async void OnOpenProjectClicked()
    {
        DialogResult result = Dialog.FolderPicker();
        if(!result.IsOk)
        {
            return;
        }

        try
        {
            await _projectModel.Open(result.Path);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception: {e}");
        }

        //Console.WriteLine($"Picked folder: {result.Path}");
    }

    private void OnExitClicked()
    {
        Context.Engine?.Exit();
    }

    public void OnDebugTaskProcessorTestClicked()
    {
        for(int i=0;i <20;++i)
        {
            _taskProcessor.EnqueueTask(new TestTask(i), 0);
        }
    }

    public void OnDebugSerializationTestClicked()
    {
        SerializationTests.Test();
    }

    public void OnDebugImporterTestClicked()
    {
        ImporterTests.Test();
    }
    #endregion
    
    #region Input

    private void Undo(ref ActionEvent evt)
    {
        evt.Consumed = true;
        
        _history.Undo();
    }

    private void Redo(ref ActionEvent evt)
    {
        evt.Consumed = true;
        
        _history.Redo();
    }

    #endregion Input
}