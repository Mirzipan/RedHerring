using Silk.NET.Windowing;
using Veldrid;

namespace RedHerring.Core;

public interface Platform
{
    Engine Engine { get; }
    GraphicsBackend GraphicsBackend { get; }
    IWindow? MainWindow { get; }
    IWindow CreateWindow();
    
    string ApplicationDataDirectory { get; }
    string ApplicationDirectory { get; }
    string ResourcesDirectory { get; }

    void Run();
}