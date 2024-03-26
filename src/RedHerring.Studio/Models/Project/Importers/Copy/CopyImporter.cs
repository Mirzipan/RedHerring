using RedHerring.Assets;
using RedHerring.Studio.Models.Project.FileSystem;
using RedHerring.Studio.Models.ViewModels.Console;

namespace RedHerring.Studio;

[Importer]
public sealed class CopyImporter : Importer<byte[]>
{
	public override string ReferenceType => nameof(AssetReference);
	
	public CopyImporter(ProjectNode owner) : base(owner)
	{
	}

	public override void UpdateCache()
	{
	}

	public override byte[]? Load()
	{
		try
		{
			return File.ReadAllBytes(Owner.AbsolutePath);
		}
		catch (Exception e)
		{
			ConsoleViewModel.LogException(e.ToString());
		}

		return null;
	}

	public override void Save(byte[] data)
	{
		throw new InvalidOperationException();
	}

	public override void ClearCache()
	{
	}

	public override void Import(string resourcesRootPath, out string? relativeResourcePath)
	{
		byte[]? data = Load();
		
		if (data == null)
		{
			ConsoleViewModel.LogError($"Cannot import file {Owner.RelativePath}, file was not loaded!");
			relativeResourcePath = null;
			return;
		}

		string path = Path.Join(resourcesRootPath, Owner.RelativePath);
		
		try
		{
			Directory.CreateDirectory(Path.GetDirectoryName(path)!);
			File.WriteAllBytes(path, data);
		}
		catch (Exception e)
		{
			ConsoleViewModel.LogException(e.ToString());
		}

		relativeResourcePath = Owner.RelativePath;
	}

	public override ImporterSettings CreateImportSettings()
	{
		return new CopyImporterSettings();
	}

	public override bool UpdateImportSettings(ImporterSettings settings)
	{
		return false;
	}
}