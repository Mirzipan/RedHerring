using System.Reflection;
using System.Runtime.InteropServices;
using RedHerring.Alexandria.Extensions.Collections;
using RedHerring.Core;
using Silk.NET.Windowing;
using Veldrid;

namespace RedHerring.Platforms;

public sealed class DesktopPlatform : Platform
{
    // TODO: platform should manage the window and update the engine, so that the Program will only need to create a platform
    private static IWindow? _window;
    
    private string _applicationDataDirectory = null!;
    private string _applicationDirectory = null!;
    private string _resourcesDirectory = null!;
    
    public GraphicsBackend GraphicsBackend { get; init; }
    public string ApplicationDataDirectory => _applicationDataDirectory;
    public string ApplicationDirectory => _applicationDirectory;
    public string ResourcesDirectory => _resourcesDirectory;

    #region Lifecycle

    internal DesktopPlatform(string name)
    {
        GraphicsBackend = PreferredGraphicsBackend();
        InitDirectories(name);
    }

    public static DesktopPlatform Create(string name)
    {
        return new DesktopPlatform(name);
    }

    #endregion Lifecycle

    #region Private

    private void InitDirectories(string name)
    {
        var uri = new Uri(Assembly.GetEntryAssembly()!.Location);
        _applicationDirectory = uri.LocalPath;
        _resourcesDirectory = Path.Combine(_applicationDirectory, "Resources");
        
        string homePath = HomePath();
        _applicationDataDirectory = Path.Combine(homePath, name);
    }

    #endregion Private

    #region Queries

    private static GraphicsBackend PreferredGraphicsBackend()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return GraphicsDevice.IsBackendSupported(GraphicsBackend.Vulkan)
                ? GraphicsBackend.Vulkan
                : GraphicsBackend.Direct3D11;
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return GraphicsDevice.IsBackendSupported(GraphicsBackend.Metal)
                ? GraphicsBackend.Metal
                : GraphicsBackend.OpenGL;
        }

        return GraphicsDevice.IsBackendSupported(GraphicsBackend.Vulkan)
            ? GraphicsBackend.Vulkan
            : GraphicsBackend.OpenGL;
    }

    private static string HomePath()
    {
        string? path = null;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            path = Environment.ExpandEnvironmentVariables("%APPDATA%");
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            path = Environment.ExpandEnvironmentVariables("XDG_CONFIG_HOME");

            if (path.IsNullOrEmpty())
            {
                path = Environment.ExpandEnvironmentVariables("~/Library/Application Support");
            }
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            path = Environment.ExpandEnvironmentVariables("XDG_CONFIG_HOME");
        }

        if (path.IsNullOrEmpty())
        {
            path = "./";
        }

        return path!;
    }

    #endregion Queries
}