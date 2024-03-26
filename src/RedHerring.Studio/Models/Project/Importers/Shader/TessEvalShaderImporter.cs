using RedHerring.Studio.Models.Project.FileSystem;

namespace RedHerring.Studio;

[Importer(ProjectNodeType.AssetTessEvalShader)]
public sealed class TessEvalShaderImporter : ShaderImporter
{
	public TessEvalShaderImporter(ProjectNode owner) : base(owner)
	{
	}

	public override ImporterSettings CreateImportSettings()
	{
		return new ShaderImporterSettings{ShaderStage = ShaderImporterStage.tesseval};
	}
}