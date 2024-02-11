using RedHerring.Alexandria;
using RedHerring.Fingerprint;
using Veldrid;
using static ImGuiNET.ImGui;

namespace RedHerring.Render.ImGui;

public static class ImGuiProxy
{
    private static ImGuiRenderer? _renderer;
    private static GraphicsDevice _device;
    private static ResourceFactory _factory;

    public static void SetDevice(GraphicsDevice device)
    {
        _device = device;
        _factory = device.ResourceFactory;
    }
    
    public static void Update(GameTime time, Input input)
    {
        _renderer?.Update((float)time.Elapsed.TotalSeconds, input);
    }

    public static IntPtr GetOrCreateImGuiBinding(TextureView textureView)
    {
        IntPtr? result = _renderer?.GetOrCreateImGuiBinding(_factory, textureView);
        return result ?? IntPtr.Zero;
    }

    public static void RemoveImGuiBinding(TextureView textureView)
    {
        _renderer?.RemoveImGuiBinding(textureView);
    }

    public static IntPtr GetOrCreateImGuiBinding(Texture texture)
    {
        IntPtr? result = _renderer?.GetOrCreateImGuiBinding(_factory, texture);
        return result ?? IntPtr.Zero;
    }

    public static void RemoveImGuiBinding(Texture texture)
    {
        _renderer?.RemoveImGuiBinding(texture);
    }
    
    internal static void ResetImGuiRenderer(ref ImGuiRenderer? renderer, GraphicsDevice device, int width, int height)
    {
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