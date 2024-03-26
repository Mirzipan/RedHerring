using RedHerring.Studio.Models.Project.FileSystem;

namespace RedHerring.Studio;

[Importer(ProjectNodeType.AssetGeometryShader)]
public sealed class GeometryShaderImporter : ShaderImporter
{
	public GeometryShaderImporter(ProjectNode owner) : base(owner)
	{
	}

	public override ImporterSettings CreateImportSettings()
	{
		return new ShaderImporterSettings{ShaderStage = ShaderImporterStage.geometry};
	}
}