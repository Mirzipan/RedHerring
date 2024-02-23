using RedHerring.Alexandria.Disposables;
using Silk.NET.Windowing;
using Veldrid;

namespace RedHerring.Render;

public static class Renderer
{
	private static RendererContext? _context;

	#region Lifecycle
    
	public static RendererContext CreateContext(IView? view, GraphicsBackend backend, bool useSeparateThread)
	{
		var previous = CurrentContext();
		RendererContext context = view is not null ? new UniversalRendererContext(view, backend, useSeparateThread) : new NullRendererContext();
		CurrentContext(previous ?? context);
        
		return context;
	}

	public static void DestroyContext(RendererContext? context = null)
	{
		var previous = CurrentContext();
		if (context is null)
		{
			context = previous;
		}

		CurrentContext(context != previous ? previous : null);
		context.TryDispose();
	}

	public static RendererContext? CurrentContext() => _context;

	public static void CurrentContext(RendererContext? context)
	{
		_context = context;
	}

	#endregion Lifecycle
}