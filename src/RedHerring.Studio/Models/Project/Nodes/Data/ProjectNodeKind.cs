using RedHerring.Render.ImGui;

namespace RedHerring.Studio.Models.Project.FileSystem;

public enum ProjectNodeKind
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

public static class ProjectNodeKindExtensions
{
	public static bool IsAssetsRelated(this ProjectNodeKind kind)
	{
		return ((uint) kind & (uint) ProjectNodeKind.FlagMask) == (uint) ProjectNodeKind.AssetFlag;
	}

	public static bool IsScriptsRelated(this ProjectNodeKind kind)
	{
		return ((uint) kind & (uint) ProjectNodeKind.FlagMask) == (uint) ProjectNodeKind.ScriptFlag;
	}

	public static ProjectNodeKind FromAssetExtension(string extension)
	{
		return extension switch
		{
			".cs" => ProjectNodeKind.ScriptFile,
			".png" => ProjectNodeKind.AssetImage,
			".jpg" => ProjectNodeKind.AssetImage,
			".jpeg" => ProjectNodeKind.AssetImage,
			".tga" => ProjectNodeKind.AssetImage,
			".fbx" => ProjectNodeKind.AssetScene,
			".obj" => ProjectNodeKind.AssetScene,
			".vsh.hlsl" => ProjectNodeKind.AssetVertexShader,
			".vsh.glsl" => ProjectNodeKind.AssetVertexShader,
			".psh.hlsl" => ProjectNodeKind.AssetPixelShader,
			".psh.glsl" => ProjectNodeKind.AssetPixelShader,
			".tesc.hlsl" => ProjectNodeKind.AssetTessControlShader,
			".tesc.glsl" => ProjectNodeKind.AssetTessControlShader,
			".tese.hlsl" => ProjectNodeKind.AssetTessEvalShader,
			".tese.glsl" => ProjectNodeKind.AssetTessEvalShader,
			".geom.hlsl" => ProjectNodeKind.AssetGeometryShader,
			".geom.glsl" => ProjectNodeKind.AssetGeometryShader,
			".comp.hlsl" => ProjectNodeKind.AssetComputeShader,
			".comp.glsl" => ProjectNodeKind.AssetComputeShader,
			".material" => ProjectNodeKind.AssetMaterial,
			_ => ProjectNodeKind.AssetBinary
		};
	}

	public static string ToIcon(this ProjectNodeKind kind)
	{
		return kind switch
		{
			ProjectNodeKind.Uninitialized => FontAwesome6.CircleQuestion,
			
			ProjectNodeKind.AssetBinary => FontAwesome6.File,
			ProjectNodeKind.AssetFolder => FontAwesome6.Folder,
			ProjectNodeKind.AssetImage => FontAwesome6.FileImage,
			ProjectNodeKind.AssetScene => FontAwesome6.Cube,
			ProjectNodeKind.AssetVertexShader => FontAwesome6.Brush,
			ProjectNodeKind.AssetPixelShader => FontAwesome6.Paintbrush,
			ProjectNodeKind.AssetTessControlShader => FontAwesome6.Shapes,
			ProjectNodeKind.AssetTessEvalShader => FontAwesome6.Shapes,
			ProjectNodeKind.AssetGeometryShader => FontAwesome6.Shapes,
			ProjectNodeKind.AssetComputeShader => FontAwesome6.SquareRootVariable,
			ProjectNodeKind.AssetMaterial => FontAwesome6.Palette,
			
			ProjectNodeKind.ScriptFile => FontAwesome6.FileCode,
			ProjectNodeKind.ScriptFolder => FontAwesome6.Folder,

			_ => FontAwesome6.Ban
		};
	}
}