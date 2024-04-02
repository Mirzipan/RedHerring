using System.Numerics;
using RedHerring.Render.Features;
using Silk.NET.Maths;

namespace RedHerring.Render;

public class RenderContext : IDisposable
{
	private readonly RenderDevice _device;
	private Vector2D<int> _size;
	
	private RenderFeatureCollection _features;
	private RenderEnvironment _environment;
	private Shared _shared = new();
	
	public RenderFeatureCollection Features => _features;
	public RenderEnvironment Environment => _environment;
	public Shared Shared => _shared;

	#region Lifecycle

	internal RenderContext(RenderDevice device)
	{
		_device = device;
		_features = new RenderFeatureCollection();
		_environment = new RenderEnvironment();
	}

	public void Dispose()
	{
		_shared.Dispose();
        
		_device.ReloadShaders(_features);
		_features.Dispose();
	}

	#endregion Lifecycle

	#region Manipulation

	public void AddFeature(RenderFeature feature)
	{
		if (Features.Get(feature.GetType()) is not null)
		{
			return;
		}
        
		Features.Add(feature);
		_device.Init(feature);
		feature.Resize(_size);
	}

	public void SetCameraViewMatrix(Matrix4x4 world, Matrix4x4 view, Matrix4x4 projection, float fieldOfView, float clipPlaneNear,
		float clipPlaneFar)
	{
		_environment.Position = world.Translation;
		_environment.ViewMatrix = view;
		_environment.ProjectiomMatrix = projection;
		_environment.ViewProjectionMatrix = view * projection;
		_environment.FieldOfView = fieldOfView;
		_environment.ClipPlaneNear = clipPlaneNear;
		_environment.ClipPlaneFar = clipPlaneFar;
	}

	#endregion Manipulation

	#region Internal

	internal void InitFeatures()
	{
		_device.Init(_features);
	}

	internal void Resize(Vector2D<int> size)
	{
		_size = size;
		_features.Resize(size);
	}

	#endregion Internal
}