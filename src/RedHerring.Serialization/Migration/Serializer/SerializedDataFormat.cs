using OdinSerializer;

namespace Migration
{
	public enum SerializedDataFormat
	{
		Invalid = -1,
		
		JSON        = 0,
		Binary      = 1,
		JSON_GZip   = 2,
		Binary_GZip = 3,
		
		Count
	}

	public static class SerializedDataFormatExtensions
	{
		public static DataFormat ToOdinDataFormat(this SerializedDataFormat data_format)
		{
			if (data_format == SerializedDataFormat.JSON || data_format == SerializedDataFormat.JSON_GZip)
			{
				return DataFormat.JSON;
			}
			return DataFormat.Binary;
		}

		public static bool IsValid(this SerializedDataFormat data_format)
		{
			return (int)data_format >= 0 && (int)data_format < (int)SerializedDataFormat.Count;
		}

		public static bool IsGZip(this SerializedDataFormat data_format)
		{
			return data_format == SerializedDataFormat.JSON_GZip || data_format == SerializedDataFormat.Binary_GZip;
		}
	}
}