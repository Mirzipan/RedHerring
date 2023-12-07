using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing.Processors.Quantization;

namespace RedHerring.Studio.Models.Project.Importers;

[Importer(".png")]
public sealed class PngImporter : AssetImporter<PngImporterSettings>
{
    protected override PngImporterSettings CreateImporterSettings() => new();

    protected override ImporterResult Import(Stream stream, PngImporterSettings settings, string resourcePath, CancellationToken cancellationToken)
    {
        using Image image = Image.Load(stream);
        
        Directory.CreateDirectory(Path.GetDirectoryName(resourcePath)!);

        var encoder = new PngEncoder
                      {
                          SkipMetadata             = false,
                          Quantizer                = new OctreeQuantizer(),
                          PixelSamplingStrategy    = new ExtensivePixelSamplingStrategy(),
                          BitDepth                 = settings.BitDepth,
                          ColorType                = settings.ColorType,
                          FilterMethod             = PngFilterMethod.Adaptive,
                          CompressionLevel         = settings.Compression,
                          TextCompressionThreshold = 0,
                          Gamma                    = settings.UseGamma ? 2.2f : 0f,
                          Threshold                = 0,
                          InterlaceMethod          = null,
                          ChunkFilter              = null,
                          TransparentColorMode     = settings.PreserveTransparentColors ? PngTransparentColorMode.Preserve : PngTransparentColorMode.Clear,
                      };
        
        image.SaveAsPng(resourcePath, encoder);
        return ImporterResult.Finished;
    }
}