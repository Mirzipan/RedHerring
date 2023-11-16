using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing.Processors.Quantization;

namespace RedHerring.Studio.Models.Project.Importers;

[ImporterProcessor(typeof(PngIntermediate))]
public class PngImporterProcessor : AssetImporterProcessor<PngIntermediate, PngImporterSettings>
{
    protected override void Process(PngIntermediate? intermediate, PngImporterSettings settings, string resourcePath)
    {
        if (intermediate is null)
        {
            return;
        }
		
        if(intermediate.Image is null)
        {
            return;
        }
		
        Directory.CreateDirectory(Path.GetDirectoryName(resourcePath)!);

        var encoder = new PngEncoder
        {
            SkipMetadata = false,
            Quantizer = new OctreeQuantizer(),
            PixelSamplingStrategy = new ExtensivePixelSamplingStrategy(),
            BitDepth = settings.BitDepth,
            ColorType = settings.ColorType,
            FilterMethod = PngFilterMethod.Adaptive,
            CompressionLevel = settings.Compression,
            TextCompressionThreshold = 0,
            Gamma = settings.UseGamma ? 2.2f : 0f,
            Threshold = 0,
            InterlaceMethod = null,
            ChunkFilter = null,
            TransparentColorMode = settings.PreserveTransparentColors ? PngTransparentColorMode.Preserve : PngTransparentColorMode.Clear,
        };
        intermediate.Image.SaveAsPng(resourcePath, encoder);
    }
}