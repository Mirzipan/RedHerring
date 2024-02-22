using RedHerring.Assets;
using RedHerring.Render;
using RedHerring.Render.Features;
using RedHerring.Render.Models;
using RedHerring.Render.Passes;
using Silk.NET.Maths;
using Veldrid;

namespace RedHerring.Studio.Debug;

public class StudioTestRenderFeature : RenderFeature, IDisposable
{
	public override int Priority => 1;
	
	public static SceneReference Cube = new(@"Test/cube.fbx.scene"); // from asset database
	
	protected override void Init(GraphicsDevice device, CommandList commandList)
	{
		// mesh
		{
			Scene? scene = Cube.Value;
			//SharedMesh sharedMesh = 
		}
	}

	public override void Update(GraphicsDevice device, CommandList commandList)
	{
	}

	public override void Render(GraphicsDevice device, CommandList commandList, RenderEnvironment environment, RenderPass pass)
	{
		int d = 0;
	}

	public override void Resize(Vector2D<int> size)
	{
	}
}