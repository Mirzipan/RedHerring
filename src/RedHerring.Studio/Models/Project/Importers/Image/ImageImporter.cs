using ImageMagick;
using OdinSerializer;
using RedHerring.Render;
using RedHerring.Studio.Models.Project.FileSystem;
using RedHerring.Studio.Models.ViewModels.Console;

namespace RedHerring.Studio;

[Importer(ProjectNodeKind.AssetImage)]
public sealed class ImageImporter : Importer<MagickImage>
{
	public override string ReferenceType => nameof(ImageReference);

	public ImageImporter(ProjectNode owner) : base(owner)
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
		ImageImporterSettings? settings = Owner.Meta?.ImporterSettings as ImageImporterSettings;
		if (settings is null)
		{
			ConsoleViewModel.LogError($"Cannot import '{Owner.RelativePath}'. Settings are missing or invalid!");
			relativeResourcePath = null;
			return;
		}

		try
		{
			if (settings.Format == ImagePixelFormat.KeepOriginal)
			{
				ImportOriginalImage(resourcesRootPath, out relativeResourcePath);
			}
			else
			{
				ImportAndConvertImage(resourcesRootPath, out relativeResourcePath, settings);
			}
		}
		catch (Exception e)
		{
			ConsoleViewModel.LogException($"Image '{Owner.RelativePath}' import failed: {e}");
			relativeResourcePath = null;
		}
	}

	public override ImporterSettings CreateImportSettings()
	{
		return new ImageImporterSettings();
	}

	public override bool UpdateImportSettings(ImporterSettings settings)
	{
		return false;
	}

	public override MagickImage? Load()
	{
		throw new NotImplementedException();
	}

	public override void Save(MagickImage data)
	{
		throw new InvalidOperationException();
	}

	private void ImportOriginalImage(string resourcesRootPath, out string? relativeResourcePath)
	{
		byte[] data = File.ReadAllBytes(Owner.AbsolutePath);
		string path = Path.Join(resourcesRootPath, Owner.RelativePath);
		
		Directory.CreateDirectory(Path.GetDirectoryName(path)!);
		File.WriteAllBytes(path, data);

		relativeResourcePath = Owner.RelativePath;
	}

	private void ImportAndConvertImage(string resourcesRootPath, out string? relativeResourcePath, ImageImporterSettings settings)
	{
		using MagickImage image = new (Owner.AbsolutePath);
		if (image is null)
		{
			ConsoleViewModel.LogException($"Image '{Owner.RelativePath}' couldn't be imported.");
			relativeResourcePath = null;
			return;
		}

		Image engineImage = Image.CreateFromMagicImage2D(image, settings.Format, settings.GenerateMipMaps ? Image.AllMipmaps : 1);
			
		byte[] json = SerializationUtility.SerializeValue(engineImage, DataFormat.Binary);
		relativeResourcePath = $"{Owner.RelativePath}.image";
		string absolutePath = Path.Join(resourcesRootPath, relativeResourcePath);
		Directory.CreateDirectory(Path.GetDirectoryName(absolutePath)!);
		File.WriteAllBytes(absolutePath, json);
	}
}