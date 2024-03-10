using System.Numerics;
using System.Reflection;
using ImGuiNET;
using RedHerring.Alexandria.Pooling;
using RedHerring.Fingerprint;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Veldrid;

namespace RedHerring.Render.ImGui;

// Taken from Veldrid and modified to work with newer Dear ImGui versions, without having to wait for updates to Veldrid itself.
// https://github.com/veldrid/veldrid/tree/master/src/Veldrid.ImGui

/// <summary>
/// Can render draw lists produced by ImGui.
/// Also provides functions for updating ImGui input.
/// </summary>
public sealed class ImGuiRenderer : IDisposable
{
    private GraphicsDevice _gd;
    private ResourceFactory _factory;
    private readonly Assembly _assembly;
    private ColorSpaceHandling _colorSpaceHandling;

    // Device objects
    private DeviceBuffer _vertexBuffer;
    private DeviceBuffer _indexBuffer;
    private DeviceBuffer _projMatrixBuffer;
    private Texture _fontTexture;
    private Shader _vertexShader;
    private Shader _fragmentShader;
    private ResourceLayout _layout;
    private ResourceLayout _textureLayout;
    private Pipeline _pipeline;
    private ResourceSet _mainResourceSet;
    private ResourceSet _fontTextureResourceSet;
    private IntPtr _fontAtlasId = (IntPtr)1;

    private int _windowWidth;
    private int _windowHeight;
    private Vector2 _scaleFactor = Vector2.One;

    // Image trackers
    private readonly Dictionary<TextureView, ResourceSetInfo> _setsByView = new();
    private readonly Dictionary<Texture, TextureView> _autoViewsByTexture = new();
    private readonly Dictionary<IntPtr, ResourceSetInfo> _viewsById = new();

    // Input
    private readonly List<InputEvent> _inputEvents = new(64);
    private readonly List<char> _inputChars = new(64);
    
    private readonly List<IDisposable> _ownedResources = new();
    private int _lastAssignedId = 100;
    private bool _frameBegun;

    /// <summary>
    /// Constructs a new ImGuiRenderer.
    /// </summary>
    /// <param name="gd">The GraphicsDevice used to create and update resources.</param>
    /// <param name="outputDescription">The output format.</param>
    /// <param name="width">The initial width of the rendering target. Can be resized.</param>
    /// <param name="height">The initial height of the rendering target. Can be resized.</param>
    internal ImGuiRenderer(GraphicsDevice gd, OutputDescription outputDescription, int width, int height)
        : this(gd, outputDescription, width, height, ColorSpaceHandling.Legacy)
    {
    }

    /// <summary>
    /// Constructs a new ImGuiRenderer.
    /// </summary>
    /// <param name="gd">The GraphicsDevice used to create and update resources.</param>
    /// <param name="outputDescription">The output format.</param>
    /// <param name="width">The initial width of the rendering target. Can be resized.</param>
    /// <param name="height">The initial height of the rendering target. Can be resized.</param>
    /// <param name="colorSpaceHandling">Identifies how the renderer should treat vertex colors.</param>
    internal ImGuiRenderer(GraphicsDevice gd, OutputDescription outputDescription, int width, int height,
        ColorSpaceHandling colorSpaceHandling)
    {
        _gd = gd;
        _factory = gd.ResourceFactory;
        _assembly = typeof(ImGuiRenderer).GetTypeInfo().Assembly;
        _colorSpaceHandling = colorSpaceHandling;
        _windowWidth = width;
        _windowHeight = height;

        IntPtr context = ImGuiNET.ImGui.CreateContext();
        ImGuiNET.ImGui.SetCurrentContext(context);

        ImGuiNET.ImGui.GetIO().Fonts.AddFontDefault();
        ImGuiNET.ImGui.GetIO().Fonts.Flags |= ImFontAtlasFlags.NoBakedLines;

        CreateDeviceResources(gd, outputDescription);

        SetPerFrameImGuiData(1f / 60f);

        ImGuiNET.ImGui.NewFrame();
        _frameBegun = true;
    }

    public void WindowResized(int width, int height)
    {
        _windowWidth = width;
        _windowHeight = height;
    }

    public void DestroyDeviceObjects()
    {
        Dispose();
    }

    public void CreateDeviceResources(GraphicsDevice gd, OutputDescription outputDescription)
        => CreateDeviceResources(gd, outputDescription, _colorSpaceHandling);

    public void CreateDeviceResources(GraphicsDevice gd, OutputDescription outputDescription,
        ColorSpaceHandling colorSpaceHandling)
    {
        _gd = gd;
        _colorSpaceHandling = colorSpaceHandling;
        ResourceFactory factory = gd.ResourceFactory;
        _vertexBuffer =
            factory.CreateBuffer(new BufferDescription(10000, BufferUsage.VertexBuffer | BufferUsage.Dynamic));
        _vertexBuffer.Name = "ImGui.NET Vertex Buffer";
        _indexBuffer = factory.CreateBuffer(new BufferDescription(2000, BufferUsage.IndexBuffer | BufferUsage.Dynamic));
        _indexBuffer.Name = "ImGui.NET Index Buffer";

        _projMatrixBuffer =
            factory.CreateBuffer(new BufferDescription(64, BufferUsage.UniformBuffer | BufferUsage.Dynamic));
        _projMatrixBuffer.Name = "ImGui.NET Projection Buffer";

        byte[] vertexShaderBytes =
            LoadEmbeddedShaderCode(gd.ResourceFactory, "imgui-vertex", ShaderStages.Vertex, _colorSpaceHandling);
        byte[] fragmentShaderBytes =
            LoadEmbeddedShaderCode(gd.ResourceFactory, "imgui-frag", ShaderStages.Fragment, _colorSpaceHandling);
        _vertexShader = factory.CreateShader(new ShaderDescription(ShaderStages.Vertex, vertexShaderBytes,
            _gd.BackendType == GraphicsBackend.Vulkan ? "main" : "VS"));
        _vertexShader.Name = "ImGui.NET Vertex Shader";
        _fragmentShader = factory.CreateShader(new ShaderDescription(ShaderStages.Fragment, fragmentShaderBytes,
            _gd.BackendType == GraphicsBackend.Vulkan ? "main" : "FS"));
        _fragmentShader.Name = "ImGui.NET Fragment Shader";

        VertexLayoutDescription[] vertexLayouts = new VertexLayoutDescription[]
        {
            new VertexLayoutDescription(
                new VertexElementDescription("in_position", VertexElementSemantic.Position, VertexElementFormat.Float2),
                new VertexElementDescription("in_texCoord", VertexElementSemantic.TextureCoordinate,
                    VertexElementFormat.Float2),
                new VertexElementDescription("in_color", VertexElementSemantic.Color, VertexElementFormat.Byte4_Norm))
        };

        _layout = factory.CreateResourceLayout(new ResourceLayoutDescription(
            new ResourceLayoutElementDescription("ProjectionMatrixBuffer", ResourceKind.UniformBuffer,
                ShaderStages.Vertex),
            new ResourceLayoutElementDescription("MainSampler", ResourceKind.Sampler, ShaderStages.Fragment)));
        _layout.Name = "ImGui.NET Resource Layout";
        _textureLayout = factory.CreateResourceLayout(new ResourceLayoutDescription(
            new ResourceLayoutElementDescription("MainTexture", ResourceKind.TextureReadOnly, ShaderStages.Fragment)));
        _textureLayout.Name = "ImGui.NET Texture Layout";

        GraphicsPipelineDescription pd = new GraphicsPipelineDescription(
            BlendStateDescription.SingleAlphaBlend,
            new DepthStencilStateDescription(false, false, ComparisonKind.Always),
            new RasterizerStateDescription(FaceCullMode.None, PolygonFillMode.Solid, FrontFace.Clockwise, true, true),
            PrimitiveTopology.TriangleList,
            new ShaderSetDescription(
                vertexLayouts,
                new[] { _vertexShader, _fragmentShader },
                new[]
                {
                    new SpecializationConstant(0, gd.IsClipSpaceYInverted),
                    new SpecializationConstant(1, _colorSpaceHandling == ColorSpaceHandling.Legacy),
                }),
            new ResourceLayout[] { _layout, _textureLayout },
            outputDescription,
            ResourceBindingModel.Default);
        _pipeline = factory.CreateGraphicsPipeline(ref pd);
        _pipeline.Name = "ImGui.NET Pipeline";

        _mainResourceSet = factory.CreateResourceSet(new ResourceSetDescription(_layout,
            _projMatrixBuffer,
            gd.PointSampler));
        _mainResourceSet.Name = "ImGui.NET Main Resource Set";

        RecreateFontDeviceTexture(gd);
    }

    /// <summary>
    /// Gets or creates a handle for a texture to be drawn with ImGui.
    /// Pass the returned handle to Image() or ImageButton().
    /// </summary>
    public IntPtr GetOrCreateImGuiBinding(TextureView textureView)
    {
        if (!_setsByView.TryGetValue(textureView, out ResourceSetInfo rsi))
        {
            ResourceSet resourceSet =
                _factory.CreateResourceSet(new ResourceSetDescription(_textureLayout, textureView));
            resourceSet.Name = $"ImGui.NET {textureView.Name} Resource Set";
            rsi = new ResourceSetInfo(GetNextImGuiBindingID(), resourceSet);

            _setsByView.Add(textureView, rsi);
            _viewsById.Add(rsi.ImGuiBinding, rsi);
            _ownedResources.Add(resourceSet);
        }

        return rsi.ImGuiBinding;
    }

    public void RemoveImGuiBinding(TextureView textureView)
    {
        if (_setsByView.TryGetValue(textureView, out ResourceSetInfo rsi))
        {
            _setsByView.Remove(textureView);
            _viewsById.Remove(rsi.ImGuiBinding);
            _ownedResources.Remove(rsi.ResourceSet);
            rsi.ResourceSet.Dispose();
        }
    }

    private IntPtr GetNextImGuiBindingID()
    {
        int newId = _lastAssignedId++;
        return (IntPtr)newId;
    }

    public Texture LoadTextureFromFile(string filePath)
    {
        using (var image = Image.Load<Rgba32>(filePath))
        {
            uint width = (uint)image.Width;
            uint height = (uint)image.Height;
            
            var description = TextureDescription.Texture2D(width, height, 1, 1, 
                PixelFormat.R8_G8_B8_A8_UNorm,
                TextureUsage.Sampled);
            
            Texture texture = _factory.CreateTexture(description);

            byte[] pixelData = new byte[width * height * 4];
            image.CopyPixelDataTo(pixelData);
            _gd.UpdateTexture(texture, pixelData, 0, 0, 0, width, height, 1, 0, 0);
            return texture;
        }
    }

    /// <summary>
    /// Gets or creates a handle for a texture to be drawn with ImGui.
    /// Pass the returned handle to Image() or ImageButton().
    /// </summary>
    public IntPtr GetOrCreateImGuiBinding(Texture texture)
    {
        if (!_autoViewsByTexture.TryGetValue(texture, out TextureView textureView))
        {
            textureView = _factory.CreateTextureView(texture);
            textureView.Name = $"ImGui.NET {texture.Name} View";
            _autoViewsByTexture.Add(texture, textureView);
            _ownedResources.Add(textureView);
        }

        return GetOrCreateImGuiBinding(textureView);
    }

    public void RemoveImGuiBinding(Texture texture)
    {
        if (_autoViewsByTexture.TryGetValue(texture, out TextureView textureView))
        {
            _autoViewsByTexture.Remove(texture);
            _ownedResources.Remove(textureView);
            textureView.Dispose();
            RemoveImGuiBinding(textureView);
        }
    }

    /// <summary>
    /// Retrieves the shader texture binding for the given helper handle.
    /// </summary>
    public ResourceSet GetImageResourceSet(IntPtr imGuiBinding)
    {
        if (!_viewsById.TryGetValue(imGuiBinding, out ResourceSetInfo rsi))
        {
            throw new InvalidOperationException("No registered ImGui binding with id " + imGuiBinding);
        }

        return rsi.ResourceSet;
    }

    public void ClearCachedImageResources()
    {
        foreach (IDisposable resource in _ownedResources)
        {
            resource.Dispose();
        }

        _ownedResources.Clear();
        _setsByView.Clear();
        _viewsById.Clear();
        _autoViewsByTexture.Clear();
        _lastAssignedId = 100;
    }

    private byte[] LoadEmbeddedShaderCode(
        ResourceFactory factory,
        string name,
        ShaderStages stage,
        ColorSpaceHandling colorSpaceHandling)
    {
        switch (factory.BackendType)
        {
            case GraphicsBackend.Direct3D11:
            {
                if (stage == ShaderStages.Vertex && colorSpaceHandling == ColorSpaceHandling.Legacy)
                {
                    name += "-legacy";
                }

                string resourceName = name + ".hlsl.bytes";
                return GetEmbeddedResourceBytes(resourceName);
            }
            case GraphicsBackend.OpenGL:
            {
                if (stage == ShaderStages.Vertex && colorSpaceHandling == ColorSpaceHandling.Legacy)
                {
                    name += "-legacy";
                }

                string resourceName = name + ".glsl";
                return GetEmbeddedResourceBytes(resourceName);
            }
            case GraphicsBackend.OpenGLES:
            {
                if (stage == ShaderStages.Vertex && colorSpaceHandling == ColorSpaceHandling.Legacy)
                {
                    name += "-legacy";
                }

                string resourceName = name + ".glsles";
                return GetEmbeddedResourceBytes(resourceName);
            }
            case GraphicsBackend.Vulkan:
            {
                string resourceName = name + ".spv";
                return GetEmbeddedResourceBytes(resourceName);
            }
            case GraphicsBackend.Metal:
            {
                string resourceName = name + ".metallib";
                return GetEmbeddedResourceBytes(resourceName);
            }
            default:
                throw new NotImplementedException();
        }
    }

    private string GetEmbeddedResourceText(string resourceName)
    {
        using (StreamReader sr = new StreamReader(_assembly.GetManifestResourceStream(resourceName)))
        {
            return sr.ReadToEnd();
        }
    }

    private byte[] GetEmbeddedResourceBytes(string resourceName)
    {
        using (Stream s = _assembly.GetManifestResourceStream(resourceName))
        {
            byte[] ret = new byte[s.Length];
            s.Read(ret, 0, (int)s.Length);
            return ret;
        }
    }

    /// <summary>
    /// Recreates the device texture used to render text.
    /// </summary>
    public unsafe void RecreateFontDeviceTexture() => RecreateFontDeviceTexture(_gd);

    /// <summary>
    /// Recreates the device texture used to render text.
    /// </summary>
    public unsafe void RecreateFontDeviceTexture(GraphicsDevice gd)
    {
        ImGuiIOPtr io = ImGuiNET.ImGui.GetIO();
        // Build
        io.Fonts.GetTexDataAsRGBA32(out byte* pixels, out int width, out int height, out int bytesPerPixel);

        // Store our identifier
        io.Fonts.SetTexID(_fontAtlasId);

        _fontTexture?.Dispose();
        _fontTexture = gd.ResourceFactory.CreateTexture(TextureDescription.Texture2D(
            (uint)width,
            (uint)height,
            1,
            1,
            PixelFormat.R8_G8_B8_A8_UNorm,
            TextureUsage.Sampled));
        _fontTexture.Name = "ImGui.NET Font Texture";
        gd.UpdateTexture(
            _fontTexture,
            (IntPtr)pixels,
            (uint)(bytesPerPixel * width * height),
            0,
            0,
            0,
            (uint)width,
            (uint)height,
            1,
            0,
            0);

        _fontTextureResourceSet?.Dispose();
        _fontTextureResourceSet =
            gd.ResourceFactory.CreateResourceSet(new ResourceSetDescription(_textureLayout, _fontTexture));
        _fontTextureResourceSet.Name = "ImGui.NET Font Texture Resource Set";

        io.Fonts.ClearTexData();
    }

    /// <summary>
    /// Renders the ImGui draw list data.
    /// </summary>
    public unsafe void Render(GraphicsDevice gd, CommandList cl)
    {
        if (_frameBegun)
        {
            _frameBegun = false;
            ImGuiNET.ImGui.Render();
            RenderImDrawData(ImGuiNET.ImGui.GetDrawData(), gd, cl);
        }
    }

    /// <summary>
    /// Updates ImGui input and IO configuration state.
    /// </summary>
    public void Update(float deltaSeconds, InteractionContext interactionContext)
    {
        BeginUpdate(deltaSeconds);
        UpdateImGuiInput(interactionContext);
        EndUpdate();
    }

    /// <summary>
    /// Called before we handle the input in <see cref="Update(float, InputSnapshot)"/>.
    /// This render ImGui and update the state.
    /// </summary>
    protected void BeginUpdate(float deltaSeconds)
    {
        if (_frameBegun)
        {
            ImGuiNET.ImGui.Render();
        }

        SetPerFrameImGuiData(deltaSeconds);
    }

    /// <summary>
    /// Called at the end of <see cref="Update(float, InputSnapshot)"/>.
    /// This tells ImGui that we are on the next frame.
    /// </summary>
    protected void EndUpdate()
    {
        _frameBegun = true;
        ImGuiNET.ImGui.NewFrame();
    }

    /// <summary>
    /// Sets per-frame data based on the associated window.
    /// This is called by Update(float).
    /// </summary>
    private unsafe void SetPerFrameImGuiData(float deltaSeconds)
    {
        ImGuiIOPtr io = ImGuiNET.ImGui.GetIO();
        io.DisplaySize = new Vector2(
            _windowWidth / _scaleFactor.X,
            _windowHeight / _scaleFactor.Y);
        io.DisplayFramebufferScale = _scaleFactor;
        io.DeltaTime = deltaSeconds; // DeltaTime is in seconds.
    }

    private void UpdateImGuiInput(InteractionContext context)
    {
        ImGuiIOPtr io = ImGuiNET.ImGui.GetIO();
        
        io.AddMousePosEvent(context.AnalogValue(Input.MouseX), context.AnalogValue(Input.MouseY));
        io.AddMouseWheelEvent(context.AnalogValue(Input.MouseWheelX), context.AnalogValue(Input.MouseWheelY));

        _inputEvents.Clear();
        context.PumpEvents(_inputEvents);
        for (int i = 0; i < _inputEvents.Count; i++)
        {
            var change = _inputEvents[i];
            var source = change.Input.ToSource();
            switch (source)
            {
                case Source.Keyboard:
                    io.AddKeyEvent(Convert.ToImGuiKey(change.Input), change.IsDown);
                    break;
                case Source.MouseButton:
                    io.AddMouseButtonEvent(Convert.ToImGuiButton(change.Input), change.IsDown);
                    break;
            }
        }

        _inputChars.Clear();
        context.Characters(_inputChars);
        for (int i = 0; i < _inputChars.Count; i++)
        {
            io.AddInputCharacter(_inputChars[i]);
        }

        // var mouse = interactionContext.Mouse;
        // if (mouse is not null)
        // {
        //     io.AddMousePosEvent(mouse.Position.X, mouse.Position.Y);
        //     io.AddMouseWheelEvent(0f, mouse.ScrollWheel.Y);
        //
        //     for (int i = 0; i < mouse.ButtonsChanged.Count; i++)
        //     {
        //         var change = mouse.ButtonsChanged[i];
        //         io.AddMouseButtonEvent((int)change.Button, change.IsDown);
        //     }
        // }
        //
        // var keyboard = interactionContext.Keyboard;
        // if (keyboard is not null)
        // {
        //     _inputChars.Clear();
        //     keyboard.Chars(_inputChars);
        //
        //     for (int i = 0; i < _inputChars.Count; i++)
        //     {
        //         io.AddInputCharacter(_inputChars[i]);
        //     }
        //
        //     for (int i = 0; i < keyboard.KeysChanged.Count; i++)
        //     {
        //         var change = keyboard.KeysChanged[i];
        //         io.AddKeyEvent(Convert.ToImGuiKey(change.Key), change.IsDown);
        //     }
        // }
    }

    private unsafe void RenderImDrawData(ImDrawDataPtr drawData, GraphicsDevice gd, CommandList cl)
    {
        uint vertexOffsetInVertices = 0;
        uint indexOffsetInElements = 0;

        if (drawData.CmdListsCount == 0)
        {
            return;
        }

        uint totalVBSize = (uint)(drawData.TotalVtxCount * sizeof(ImDrawVert));
        if (totalVBSize > _vertexBuffer.SizeInBytes)
        {
            _vertexBuffer.Dispose();
            _vertexBuffer = gd.ResourceFactory.CreateBuffer(new BufferDescription((uint)(totalVBSize * 1.5f),
                BufferUsage.VertexBuffer | BufferUsage.Dynamic));
            _vertexBuffer.Name = $"ImGui.NET Vertex Buffer";
        }

        uint totalIBSize = (uint)(drawData.TotalIdxCount * sizeof(ushort));
        if (totalIBSize > _indexBuffer.SizeInBytes)
        {
            _indexBuffer.Dispose();
            _indexBuffer = gd.ResourceFactory.CreateBuffer(new BufferDescription((uint)(totalIBSize * 1.5f),
                BufferUsage.IndexBuffer | BufferUsage.Dynamic));
            _indexBuffer.Name = $"ImGui.NET Index Buffer";
        }

        for (int i = 0; i < drawData.CmdListsCount; i++)
        {
            ImDrawListPtr cmdList = drawData.CmdLists[i];

            cl.UpdateBuffer(
                _vertexBuffer,
                vertexOffsetInVertices * (uint)sizeof(ImDrawVert),
                cmdList.VtxBuffer.Data,
                (uint)(cmdList.VtxBuffer.Size * sizeof(ImDrawVert)));

            cl.UpdateBuffer(
                _indexBuffer,
                indexOffsetInElements * sizeof(ushort),
                cmdList.IdxBuffer.Data,
                (uint)(cmdList.IdxBuffer.Size * sizeof(ushort)));

            vertexOffsetInVertices += (uint)cmdList.VtxBuffer.Size;
            indexOffsetInElements += (uint)cmdList.IdxBuffer.Size;
        }

        // Setup orthographic projection matrix into our constant buffer
        {
            var io = ImGuiNET.ImGui.GetIO();

            Matrix4x4 mvp = Matrix4x4.CreateOrthographicOffCenter(
                0f,
                io.DisplaySize.X,
                io.DisplaySize.Y,
                0.0f,
                -1.0f,
                1.0f);

            _gd.UpdateBuffer(_projMatrixBuffer, 0, ref mvp);
        }

        cl.SetVertexBuffer(0, _vertexBuffer);
        cl.SetIndexBuffer(_indexBuffer, IndexFormat.UInt16);
        cl.SetPipeline(_pipeline);
        cl.SetGraphicsResourceSet(0, _mainResourceSet);

        drawData.ScaleClipRects(ImGuiNET.ImGui.GetIO().DisplayFramebufferScale);

        // Render command lists
        int vtxOffset = 0;
        int idxOffset = 0;
        for (int n = 0; n < drawData.CmdListsCount; n++)
        {
            ImDrawListPtr cmdList = drawData.CmdLists[n];
            for (int cmd = 0; cmd < cmdList.CmdBuffer.Size; cmd++)
            {
                ImDrawCmdPtr pcmd = cmdList.CmdBuffer[cmd];
                if (pcmd.UserCallback != IntPtr.Zero)
                {
                    throw new NotImplementedException();
                }

                if (pcmd.TextureId != IntPtr.Zero)
                {
                    if (pcmd.TextureId == _fontAtlasId)
                    {
                        cl.SetGraphicsResourceSet(1, _fontTextureResourceSet);
                    }
                    else
                    {
                        cl.SetGraphicsResourceSet(1, GetImageResourceSet(pcmd.TextureId));
                    }
                }

                cl.SetScissorRect(
                    0,
                    (uint)pcmd.ClipRect.X,
                    (uint)pcmd.ClipRect.Y,
                    (uint)(pcmd.ClipRect.Z - pcmd.ClipRect.X),
                    (uint)(pcmd.ClipRect.W - pcmd.ClipRect.Y));

                cl.DrawIndexed(pcmd.ElemCount, 1, pcmd.IdxOffset + (uint)idxOffset,
                    (int)(pcmd.VtxOffset + vtxOffset), 0);
            }

            idxOffset += cmdList.IdxBuffer.Size;
            vtxOffset += cmdList.VtxBuffer.Size;
        }
    }

    /// <summary>
    /// Frees all graphics resources used by the renderer.
    /// </summary>
    public void Dispose()
    {
        _vertexBuffer.Dispose();
        _indexBuffer.Dispose();
        _projMatrixBuffer.Dispose();
        _fontTexture.Dispose();
        _vertexShader.Dispose();
        _fragmentShader.Dispose();
        _layout.Dispose();
        _textureLayout.Dispose();
        _pipeline.Dispose();
        _mainResourceSet.Dispose();
        _fontTextureResourceSet.Dispose();

        foreach (IDisposable resource in _ownedResources)
        {
            resource.Dispose();
        }
    }

    private struct ResourceSetInfo
    {
        public readonly IntPtr ImGuiBinding;
        public readonly ResourceSet ResourceSet;

        public ResourceSetInfo(IntPtr imGuiBinding, ResourceSet resourceSet)
        {
            ImGuiBinding = imGuiBinding;
            ResourceSet = resourceSet;
        }
    }
}