using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace Migration
{
	internal static class GZipWrapper
	{
		internal static async Task<byte[]> CompressAsync(byte[] bytes)
		{
			using MemoryStream     stream = new MemoryStream();
			{
				await using GZipStream zip  = new GZipStream(stream, CompressionMode.Compress);
				Task                   task = zip.WriteAsync(bytes, 0, bytes.Length);
				await task;
				if (task.IsCanceled || task.IsFaulted)
				{
					throw task.Exception;
				}

				await zip.FlushAsync();
			}
			return stream.GetBuffer();
		}

		internal static byte[] Compress(byte[] bytes)
		{
			using MemoryStream stream = new MemoryStream();
			{
				using GZipStream zip = new GZipStream(stream, CompressionMode.Compress);
				zip.Write(bytes, 0, bytes.Length);
				zip.Flush();
			}
			return stream.GetBuffer();
		}
		
		internal static async Task<byte[]> DecompressAsync(byte[] bytes)
		{
			using MemoryStream     stream    = new MemoryStream(bytes);
			using MemoryStream     outStream = new MemoryStream();
			{
				await using GZipStream zip  = new GZipStream(stream, CompressionMode.Decompress);
				Task                   task = zip.CopyToAsync(outStream);
				await task;
				if (task.IsCanceled || task.IsFaulted)
				{
					throw task.Exception;
				}

				await zip.FlushAsync();
			}
			return outStream.GetBuffer();
		}

		internal static byte[] Decompress(byte[] bytes)
		{
			using MemoryStream stream    = new MemoryStream(bytes);
			using MemoryStream outStream = new MemoryStream();
			{
				using GZipStream zip = new GZipStream(stream, CompressionMode.Decompress);
				zip.CopyTo(outStream);
				zip.Flush();
			}
			return outStream.GetBuffer();
		}
	}
}