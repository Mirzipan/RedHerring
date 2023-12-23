using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace Migration
{
	internal static class GZipWrapper
	{
		internal static async Task<byte[]> CompressAsync(byte[] bytes)
		{
			MemoryStream stream = new MemoryStream();
			GZipStream   zip    = new GZipStream(stream, CompressionMode.Compress);
			Task         task   = zip.WriteAsync(bytes, 0, bytes.Length);
			await task;
			if (task.IsCanceled || task.IsFaulted)
			{
				throw task.Exception;
			}

			await zip.DisposeAsync();
			return stream.GetBuffer();
		}

		internal static byte[] Compress(byte[] bytes)
		{
			MemoryStream stream = new MemoryStream();
			GZipStream   zip    = new GZipStream(stream, CompressionMode.Compress);
			zip.Write(bytes, 0, bytes.Length);
			zip.Dispose();
			return stream.GetBuffer();
		}
		
		internal static async Task<byte[]> DecompressAsync(byte[] bytes)
		{
			MemoryStream stream    = new MemoryStream(bytes);
			MemoryStream outStream = new MemoryStream();
			GZipStream   zip       = new GZipStream(stream, CompressionMode.Decompress);
			Task         task      = zip.CopyToAsync(outStream);
			await task;
			if (task.IsCanceled || task.IsFaulted)
			{
				throw task.Exception;
			}

			await zip.DisposeAsync();
			return outStream.GetBuffer();
		}

		internal static byte[] Decompress(byte[] bytes)
		{
			MemoryStream stream    = new MemoryStream(bytes);
			MemoryStream outStream = new MemoryStream();
			GZipStream   zip       = new GZipStream(stream, CompressionMode.Decompress);
			zip.CopyTo(outStream);
			zip.Dispose();
			return outStream.GetBuffer();
		}
	}
}