using System;
using OdinSerializer.Utilities;

namespace Migration
{
	public abstract class BaseTargetClassAttribute : Attribute
	{
		public const    int  INVALID_VERSION = -1;

		public readonly int  Version;
		public          bool IsVersionValid => Version != INVALID_VERSION;
		
		public readonly Type DataClass;
		
		protected BaseTargetClassAttribute(Type data_class, int version)
		{
			Version   = version;
			DataClass = data_class;
		}
		
		public string CustomId => DataClass.GetAttribute<SerializedClassIdAttribute>(false)?.Id;
	}
}