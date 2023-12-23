using System.Text.Json;

namespace RedHerring.Studio.Models.Project.FileSystem;

public sealed class ProjectScriptFileHeader
{
	private const string _scriptHeader = "//Meta"; 
	
	[Serializable]
	public class FileId
	{
		public string Guid    { get; set; }
		public string Type    { get; set; }
		public int    Version { get; set; }

		public FileId(string guid, string type, int version)
		{
			Guid    = guid;
			Type    = type;
			Version = version;
		}
	}

	public static FileId? ReadFromFile(string path)
	{
		using FileStream file = new(path, FileMode.Open);

		if (file.Length <= _scriptHeader.Length + 1)
		{
			return null;
		}

		byte[] header = new byte[_scriptHeader.Length];
		file.ReadExactly(header, 0, _scriptHeader.Length);

		bool equals = true;
		for (int i = 0; i < _scriptHeader.Length; ++i)
		{
			if (header[i] != _scriptHeader[i])
			{
				equals = false;
				break;
			}
		}

		if (!equals)
		{
			return null;
		}

		byte[] content = new byte[file.Length - _scriptHeader.Length];
		if (file.ReadAtLeast(content, 1, false) != content.Length)
		{
			return null;
		}

		int endOfLine = Array.FindIndex(content, x => x == '\n' || x == '\r');
		if (endOfLine != -1)
		{
			Array.Resize(ref content, endOfLine);
		}

		return JsonSerializer.Deserialize<FileId>(content);
	}

	public static string CreateHeaderString(FileId fileId)
	{
		return $"{_scriptHeader}{JsonSerializer.Serialize(fileId)}";
	}

	public static FileId? ReadFromStringLine(string line)
	{
		if (!line.StartsWith(_scriptHeader))
		{
			return null;
		}

		string json = line.Substring(_scriptHeader.Length);
		return JsonSerializer.Deserialize<FileId>(json);
	}
}