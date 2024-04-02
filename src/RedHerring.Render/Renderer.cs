using Silk.NET.Maths;

namespace RedHerring.Render;

public static class Renderer
{
	private static RenderDevice _device = null!;
	private static RenderContext _context = null!;

	#region Lifecycle

	public static RenderContext Init(RenderDevice device)
	{
		_device = device;
		_context = CreateContext();
		
		_device.Init();
		
		return _context;
	}
    
	public static RenderContext CreateContext()
	{
		var previous = CurrentContext();
		// TODO(Mirzi): deactive context? unbind listeners?
		
		var context = new RenderContext(_device);
		CurrentContext(context);
		return context;
	}

	public static void DestroyContext(RenderContext? context = null)
	{
		var previous = CurrentContext();
		if (context is null)
		{
			context = previous;
		}

		CurrentContext(context != previous ? previous : CreateContext());
		context.Dispose();
	}

	public static RenderContext CurrentContext() => _context;

	public static void CurrentContext(RenderContext context)
	{
		_context = context;
	}

	#endregion Lifecycle

	#region Public

	public static void Resize(Vector2D<int> size)
	{
		_device.Resize(size);
		_context.Resize(size);
	}

	public static bool BeginDraw() => _device.BeginDraw();

	public static void NextFrame() => _device.Draw(_context);
	public static void EndDraw() => _device.EndDraw();

	public static void ReloadShaders()
	{
		// TODO(Mirzi): force reload shaders
	}

	#endregion Public
}