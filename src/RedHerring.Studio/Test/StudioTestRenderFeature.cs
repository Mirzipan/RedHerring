using System.Numerics;
using RedHerring.Alexandria.Disposables;
using RedHerring.Assets;
using RedHerring.Numbers;
using RedHerring.Render;
using RedHerring.Render.Features;
using RedHerring.Render.Models;
using RedHerring.Render.Passes;
using Silk.NET.Maths;
using Veldrid;
using Veldrid.SPIRV;

namespace RedHerring.Studio.Debug;

public class StudioTestRenderFeature : RenderFeature, IDisposable
{
	public override int Priority => 1;

	// this will be in generated AssetDatabase class
	public static SceneReference Cube         = new(@"cube.fbx.scene");   
	public static AssetReference VertexShader = new(@"diffuse.vsh.spirv");
	public static AssetReference PixelShader  = new(@"diffuse.psh.spirv");

	// move to shared resources, materials, etc..
	private DeviceBuffer   _vertexShaderConstants      = null!;
	private ResourceLayout _vertexShaderResourceLayout = null!;
	private ResourceSet    _vertexShaderResourceSet    = null!;
	
	private DeviceBuffer   _pixelShaderConstants      = null!;
	private ResourceLayout _pixelShaderResourceLayout = null!;
	private ResourceSet    _pixelShaderResourceSet    = null!;

	private Shader[]    _shaders     = null!;
	private Pipeline    _pipeline    = null!;
	private CommandList _commandList = null!;
	
	// game data
	private float _time = 0;
	
	// camera
	private Vector3   _cameraPosition;
	private Vector3   _cameraDirection;
	private Vector3   _cameraUp;
	private float     _cameraAspectRatio;
	private Matrix4x4 _viewMatrix;
	private Matrix4x4 _projectionMatrix;

	// object
	private Vector3    _objectPosition;
	private Quaternion _objectRotation;
	private Color4     _objectColor;
	private Matrix4x4  _worldMatrix;
	private Matrix4x4  _worldViewProjectionMatrix;
	private SharedMesh _mesh = null!;
	
	// lights
	private Vector3 _lightDirection;
	private Color4  _lightColor;
	private Color4  _ambientColor;
	
	protected override void Init(GraphicsDevice device, CommandList commandList)
	{
		// TODO - this is called 2x .. why?
		
		InitMesh(device);
		InitShaders(device);
		InitVertexShaderResources(device);
		InitPixelShaderResources(device);
		InitPipeline(device);
		InitCommandList(device);

		_cameraPosition  = new Vector3(0, 0, -50);
		_cameraDirection = new Vector3(0, 0, 1);
		_cameraUp        = new Vector3(0, 1, 0);

		_objectPosition = Vector3.Zero;
		_objectRotation = Quaternion.Identity;
		_objectColor    = Color4.OrangeRed;

		_lightDirection = Vector3.Normalize(new Vector3(-1, -1, -1));
		_lightColor     = Color4.White;
		_ambientColor   = Color4.DarkGray;
	}

	public override void Update(GraphicsDevice device, CommandList commandList)
	{
		_time += 0.0001f;
		
		UpdateCamera();
		UpdateObject();
		UpdateVertexShaderConstantsBuffer(device);
		UpdatePixelShaderConstantsBuffer(device);
	}

	public override void Render(GraphicsDevice device, CommandList commandList, RenderEnvironment environment, RenderPass pass)
	{
		commandList.SetFramebuffer(device.SwapchainFramebuffer);
		commandList.SetVertexBuffer(0, _mesh.VertexBuffer);
		commandList.SetIndexBuffer(_mesh.IndexBuffer, _mesh.IndexFormat);

		commandList.SetPipeline(_pipeline);

		commandList.SetGraphicsResourceSet(0, _vertexShaderResourceSet);
		commandList.SetGraphicsResourceSet(1, _pixelShaderResourceSet);

		commandList.DrawIndexed(_mesh.IndexCount, 1, 0, 0, 0);
	}

	public override void Resize(Vector2D<int> size)
	{
		if (size.X == 0 || size.Y == 0)
		{
			return;
		}

		_cameraAspectRatio = (float)size.X / size.Y;
	}

	#region Init
	private void InitMesh(GraphicsDevice device)
	{
		Scene? scene = Cube.Value;
		if (scene is null)
		{
			throw new NullReferenceException($"{Cube.Path} is missing!");
		}

		_mesh = Renderer.CurrentContext()!.Shared.GetOrCreateMesh(device, Cube.Path, scene.Meshes[0]);
	}

	private void InitShaders(GraphicsDevice device)
	{
		byte[]? vertexShaderByteCode = VertexShader.Value;
		if (vertexShaderByteCode is null)
		{
			throw new NullReferenceException($"{VertexShader.Path} is missing!");
		}

		ShaderDescription vertexShaderDesc = new (ShaderStages.Vertex, vertexShaderByteCode, "main");

		byte[]? pixelShaderByteCode = PixelShader.Value;
		if (pixelShaderByteCode is null)
		{
			throw new NullReferenceException($"{PixelShader.Path} is missing!");
		}

		ShaderDescription pixelShaderDesc = new (ShaderStages.Fragment, pixelShaderByteCode, "main");

		_shaders = device.ResourceFactory.CreateFromSpirv(vertexShaderDesc, pixelShaderDesc);
		_shaders[0].DisposeWith(this);
		_shaders[1].DisposeWith(this);
	}
	
	private void InitVertexShaderResources(GraphicsDevice device)
	{
		BufferDescription cBufferDescription = new (sizeof(float) * (16 + 16), BufferUsage.UniformBuffer);
		_vertexShaderConstants = device.ResourceFactory.CreateBuffer(cBufferDescription);
		_vertexShaderConstants.DisposeWith(this);
		
		ResourceLayoutDescription resourceLayoutDescription = new (
			new ResourceLayoutElementDescription("VSHConstants", ResourceKind.UniformBuffer, ShaderStages.Vertex)
		);
		_vertexShaderResourceLayout = device.ResourceFactory.CreateResourceLayout(resourceLayoutDescription);
		_vertexShaderResourceLayout.DisposeWith(this);

		ResourceSetDescription resourceSetDescription = new (_vertexShaderResourceLayout,
			_vertexShaderConstants
		);
		_vertexShaderResourceSet = device.ResourceFactory.CreateResourceSet(resourceSetDescription);
		_vertexShaderResourceSet.DisposeWith(this);
	}

	private void InitPixelShaderResources(GraphicsDevice device)
	{
		BufferDescription cBufferDescription = new (sizeof(float) * (4 + 4 + 4 + 4), BufferUsage.UniformBuffer);
		_pixelShaderConstants = device.ResourceFactory.CreateBuffer(cBufferDescription);
		_pixelShaderConstants.DisposeWith(this);

		ResourceLayoutDescription resourceLayoutDescription = new (
			new ResourceLayoutElementDescription("PSHConstants", ResourceKind.UniformBuffer, ShaderStages.Fragment)
		);
		_pixelShaderResourceLayout = device.ResourceFactory.CreateResourceLayout(resourceLayoutDescription);
		_pixelShaderResourceLayout.DisposeWith(this);

		ResourceSetDescription resourceSetDescription = new (_pixelShaderResourceLayout,
			_pixelShaderConstants
		);
		_pixelShaderResourceSet = device.ResourceFactory.CreateResourceSet(resourceSetDescription);
		_pixelShaderResourceSet.DisposeWith(this);
	}

	private void InitPipeline(GraphicsDevice device)
	{
		GraphicsPipelineDescription pipelineDescription = new GraphicsPipelineDescription
		                                                  {
			                                                  BlendState = BlendStateDescription.SingleOverrideBlend,
			                                                  DepthStencilState = new
			                                                  (
				                                                  true,
				                                                  true,
				                                                  ComparisonKind.LessEqual
			                                                  ),
			                                                  RasterizerState = new
			                                                  (
				                                                  FaceCullMode.Back,
				                                                  PolygonFillMode.Solid,
				                                                  FrontFace.Clockwise,
				                                                  true,
				                                                  false
			                                                  ),
			                                                  PrimitiveTopology = PrimitiveTopology.TriangleList,
			                                                  ResourceLayouts   = new ResourceLayout[]
			                                                                      {
				                                                                      _vertexShaderResourceLayout,
				                                                                      _pixelShaderResourceLayout
			                                                                      },
			                                                  ShaderSet = new
			                                                  (
				                                                  new[] {_mesh.VertexLayoutDescription},
				                                                  _shaders
			                                                  ),
			                                                  Outputs = device.SwapchainFramebuffer.OutputDescription
		                                                  };

		_pipeline = device.ResourceFactory.CreateGraphicsPipeline(pipelineDescription);
		_pipeline.DisposeWith(this);
	}

	private void InitCommandList(GraphicsDevice device)
	{
		_commandList = device.ResourceFactory.CreateCommandList();
		_commandList.DisposeWith(this);
	}
	#endregion
	
	#region Update
	private void UpdateCamera()
	{
		_projectionMatrix = Matrix4x4.CreatePerspectiveFieldOfViewLeftHanded(MathF.PI * 0.5f, _cameraAspectRatio, 1f, 100f);
		_viewMatrix       = Matrix4x4.CreateLookAt(_cameraPosition, _cameraPosition + _cameraDirection, _cameraUp);
	}

	private void UpdateObject()
	{
		float t = _time;
		_objectRotation = Quaternion.CreateFromYawPitchRoll(t * 1f, t * 0.7f, t * 0.6f);
		_worldMatrix    = Matrix4x4.CreateFromQuaternion(_objectRotation) * Matrix4x4.CreateTranslation(_objectPosition);
		
		_worldViewProjectionMatrix = _worldMatrix * _viewMatrix * _projectionMatrix;
		//_worldViewProjectionMatrix = _projectionMatrix * _viewMatrix * _worldMatrix;
	}

	private void UpdateVertexShaderConstantsBuffer(GraphicsDevice device)
	{
		float[] tmp = new float[_vertexShaderConstants.SizeInBytes / sizeof(float)];
		int     w   = 0;

		for (int row = 0; row < 4; ++row)
		{
			for (int column = 0; column < 4; ++column)
			{
				tmp[w++] = _worldViewProjectionMatrix[row, column];
			}
		}

		for (int row = 0; row < 4; ++row)
		{
			for (int column = 0; column < 4; ++column)
			{
				tmp[w++] = _worldMatrix[row, column];
			}
		}

		device.UpdateBuffer(_vertexShaderConstants, 0, tmp);
	}

	private void UpdatePixelShaderConstantsBuffer(GraphicsDevice device)
	{
		float[] tmp = new float[_pixelShaderConstants.SizeInBytes / sizeof(float)];
		int     w   = 0;

		tmp[w++] = _objectColor.R;
		tmp[w++] = _objectColor.G;
		tmp[w++] = _objectColor.B;
		tmp[w++] = _objectColor.A;
		
		tmp[w++] = _lightDirection.X;
		tmp[w++] = _lightDirection.Y;
		tmp[w++] = _lightDirection.Z;
		tmp[w++] = 0;
		
		tmp[w++] = _lightColor.R;
		tmp[w++] = _lightColor.G;
		tmp[w++] = _lightColor.B;
		tmp[w++] = _lightColor.A;

		tmp[w++] = _ambientColor.R;
		tmp[w++] = _ambientColor.G;
		tmp[w++] = _ambientColor.B;
		tmp[w++] = _ambientColor.A;
		
		device.UpdateBuffer(_pixelShaderConstants, 0, tmp);
	}
	#endregion
}