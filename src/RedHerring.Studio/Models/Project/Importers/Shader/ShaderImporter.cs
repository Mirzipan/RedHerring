using RedHerring.Core.Systems;
using RedHerring.Studio.Models.Project.FileSystem;
using RedHerring.Studio.Models.ViewModels.Console;

namespace RedHerring.Studio;

public abstract class ShaderImporter : Importer
{
	private static readonly string _compilerAbsolutePath = Path.Join(PathsSystem.ShaderCompilerPath());
	public override         string ReferenceType => nameof(RedHerring.Assets.AssetReference);

	public ShaderImporter(ProjectNode owner) : base(owner)
	{
	}

	public override void UpdateCache()
	{
	}

	public override void ClearCache()
	{
	}

	public override void Import(string resourcesRootPath, out string? relativeResourcePath)
	{
		relativeResourcePath = Owner.RelativePath.Replace(".hlsl", ".spirv").Replace(".glsl", ".spirv");
		string targetPath         = Path.Join(resourcesRootPath, relativeResourcePath);

		ShaderImporterSettings? settings = Owner.Meta?.ImporterSettings as ShaderImporterSettings;
		if (settings is null)
		{
			ConsoleViewModel.LogError($"Cannot import '{Owner.RelativePath}' - settings are missing or invalid!");
			relativeResourcePath = null;
			return;
		}

		string arguments = $"-fentry-point={settings.EntryPoint} -fshader-stage={settings.ShaderStage} -o \"{targetPath}\" \"{Owner.AbsolutePath}\"";
		ConsoleViewModel.LogInfo($"Executing: {_compilerAbsolutePath} {arguments}");
		string outputLog = FileExecutionUtility.ExecuteFile(_compilerAbsolutePath, arguments);
		ConsoleViewModel.LogInfo(outputLog);
	}

	public override bool UpdateImportSettings(ImporterSettings settings)
	{
		return false;
	}
}
