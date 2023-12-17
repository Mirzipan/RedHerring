﻿using RedHerring.Alexandria;
using Veldrid;

namespace RedHerring.Render.ImGui;

public static class ImGuiProxy
{
    private static ImGuiRenderer? _renderer;
    
    public static void Update(GameTime time, InputSnapshot snapshot)
    {
        _renderer?.Update((float)time.Elapsed.TotalSeconds, snapshot);
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
        Theme.CrimsonRivers();

        _renderer = renderer;
    }

    internal static void Dispose()
    {
        _renderer = null;
    }
}