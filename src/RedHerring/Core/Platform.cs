using Veldrid;

namespace RedHerring.Core;

public interface Platform
{
    GraphicsBackend GraphicsBackend { get; }
    
    string ApplicationDataDirectory { get; }
    string ApplicationDirectory { get; }
    string ResourcesDirectory { get; }
}