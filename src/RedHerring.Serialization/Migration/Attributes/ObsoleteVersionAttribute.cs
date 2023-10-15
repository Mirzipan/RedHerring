using System;

namespace Migration
{
	// Attribute to tell serialization binder which class from original data is mapped to obsolete version of migratable class.
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public sealed class ObsoleteVersionAttribute : BaseTargetClassAttribute
	{
		public ObsoleteVersionAttribute(Type data_class, int version = INVALID_VERSION) : base(data_class, version)
		{
		}
	}
}