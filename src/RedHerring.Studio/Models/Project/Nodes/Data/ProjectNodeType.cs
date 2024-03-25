using RedHerring.Render.ImGui;

namespace RedHerring.Studio.Models.Project.FileSystem;

public enum ProjectNodeType
{
	Uninitialized = 0,
	
	AssetFlag              = 0x01,
	AssetFolder            = 1  << 8 | AssetFlag,
	AssetImage             = 2  << 8 | AssetFlag,
	AssetScene             = 3  << 8 | AssetFlag,
	AssetBinary            = 4  << 8 | AssetFlag,
	AssetMaterial          = 5  << 8 | AssetFlag,
	AssetVertexShader      = 6  << 8 | AssetFlag,
	AssetPixelShader       = 7  << 8 | AssetFlag,
	AssetTessControlShader = 8  << 8 | AssetFlag,
	AssetTessEvalShader    = 9  << 8 | AssetFlag,
	AssetGeometryShader    = 10 << 8 | AssetFlag,
	AssetComputeShader     = 11 << 8 | AssetFlag,
	
	ScriptFlag   = 0x02,
	ScriptFolder = 1 << 8 | ScriptFlag,
	ScriptFile   = 2 << 8 | ScriptFlag,
	
	FlagMask = 0xff
}

public static class ProjectNodeTypeExtensions
{
	public static bool IsAssetsRelated(this ProjectNodeType type)
	{
		return ((uint) type & (uint) ProjectNodeType.FlagMask) == (uint) ProjectNodeType.AssetFlag;
	}

	public static bool IsScriptsRelated(this ProjectNodeType type)
	{
		return ((uint) type & (uint) ProjectNodeType.FlagMask) == (uint) ProjectNodeType.ScriptFlag;
	}

	public static ProjectNodeType FromAssetExtension(string extension)
	{
		return extension switch
		{
			".png" => ProjectNodeType.AssetImage,
			".jpg" => ProjectNodeType.AssetImage,
			".fbx" => ProjectNodeType.AssetScene,
			".obj" => ProjectNodeType.AssetScene,
			".vsh.hlsl" => ProjectNodeType.AssetVertexShader,
			".vsh.glsl" => ProjectNodeType.AssetVertexShader,
			".psh.hlsl" => ProjectNodeType.AssetPixelShader,
			".psh.glsl" => ProjectNodeType.AssetPixelShader,
			".tesc.hlsl" => ProjectNodeType.AssetTessControlShader,
			".tesc.glsl" => ProjectNodeType.AssetTessControlShader,
			".tese.hlsl" => ProjectNodeType.AssetTessEvalShader,
			".tese.glsl" => ProjectNodeType.AssetTessEvalShader,
			".geom.hlsl" => ProjectNodeType.AssetGeometryShader,
			".geom.glsl" => ProjectNodeType.AssetGeometryShader,
			".comp.hlsl" => ProjectNodeType.AssetComputeShader,
			".comp.glsl" => ProjectNodeType.AssetComputeShader,
			".material" => ProjectNodeType.AssetMaterial,
			_ => ProjectNodeType.AssetBinary
		};
	}

	public static string ToIcon(this ProjectNodeType type)
	{
		return type switch
		{
			ProjectNodeType.Uninitialized => FontAwesome6.CircleQuestion,
			
			ProjectNodeType.AssetBinary => FontAwesome6.File,
			ProjectNodeType.AssetFolder => FontAwesome6.Folder,
			ProjectNodeType.AssetImage => FontAwesome6.FileImage,
			ProjectNodeType.AssetScene => FontAwesome6.Cube,
			ProjectNodeType.AssetVertexShader => FontAwesome6.Brush,
			ProjectNodeType.AssetPixelShader => FontAwesome6.Paintbrush,
			ProjectNodeType.AssetTessControlShader => FontAwesome6.Shapes,
			ProjectNodeType.AssetTessEvalShader => FontAwesome6.Shapes,
			ProjectNodeType.AssetGeometryShader => FontAwesome6.Shapes,
			ProjectNodeType.AssetComputeShader => FontAwesome6.SquareRootVariable,
			ProjectNodeType.AssetMaterial => FontAwesome6.Palette,
			
			ProjectNodeType.ScriptFile => FontAwesome6.FileCode,
			ProjectNodeType.ScriptFolder => FontAwesome6.Folder,

			_ => FontAwesome6.Ban
		};
	}
}