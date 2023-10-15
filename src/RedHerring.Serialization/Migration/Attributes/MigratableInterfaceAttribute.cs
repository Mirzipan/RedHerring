using System;

namespace Migration
{
	// Attribute that marks root of migratable hierarchy.
	// Migration manager scans for all assignable types to migratable interfaces.
	// Attribute to tell serialization binder which class from original data is mapped to migratable interface.
	[AttributeUsage(AttributeTargets.Interface, Inherited = false)]
	public sealed class MigratableInterfaceAttribute : BaseTargetClassAttribute
	{
		public MigratableInterfaceAttribute(Type data_class) : base(data_class, INVALID_VERSION)
		{
		}
	}
}