using RedHerring.Alexandria;
using RedHerring.Inputs;
using Veldrid;
using static ImGuiNET.ImGui;

namespace RedHerring.Render.ImGui;

public static class ImGuiProxy
{
    private static ImGuiRenderer? _renderer;
    private static Dictionary<IntPtr, Texture> _texturesById = new();
    
    public static void Update(GameTime time, InteractionContext interactionContext)
    {
        _renderer?.Update((float)time.Elapsed.TotalSeconds, interactionContext);
    }

    public static IntPtr GetOrCreateImGuiBinding(string filePath)
    {
        if (_renderer is null)
        {
            return IntPtr.Zero;
        }
        
        var texture = _renderer.LoadTextureFromFile(filePath);
        IntPtr ptr = _renderer.GetOrCreateImGuiBinding(texture);
        _texturesById[ptr] = texture;
        return ptr;
    }

    public static void RemoveImGuiBinding(IntPtr binding)
    {
        if (_texturesById.TryGetValue(binding, out var texture))
        {
            RemoveImGuiBinding(texture);
            _texturesById.Remove(binding);
        }
    }

    public static IntPtr GetOrCreateImGuiBinding(TextureView textureView)
    {
        IntPtr? result = _renderer?.GetOrCreateImGuiBinding(textureView);
        return result ?? IntPtr.Zero;
    }

    public static void RemoveImGuiBinding(TextureView textureView)
    {
        _renderer?.RemoveImGuiBinding(textureView);
    }

    public static IntPtr GetOrCreateImGuiBinding(Texture texture)
    {
        IntPtr? result = _renderer?.GetOrCreateImGuiBinding(texture);
        return result ?? IntPtr.Zero;
    }

    public static void RemoveImGuiBinding(Texture texture)
    {
        _renderer?.RemoveImGuiBinding(texture);
    }
    
    internal static void ResetImGuiRenderer(ref ImGuiRenderer? renderer, GraphicsDevice device, int width, int height)
    {
        _texturesById.Clear();
        
        if (renderer is not null)
        {
            renderer.ClearCachedImageResources();
            renderer.DestroyDeviceObjects();
            renderer.Dispose();
        }

        renderer = new ImGuiRenderer(
            device,
            device.MainSwapchain.Framebuffer.OutputDescription,
            width,
            height);

        FontLoader.Load(renderer!);
        StyleColorsDark();

        _renderer = renderer;
    }

    internal static void Dispose()
    {
        _renderer = null;
    }
}