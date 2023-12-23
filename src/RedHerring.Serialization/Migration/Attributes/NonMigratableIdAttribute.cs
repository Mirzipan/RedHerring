using System;

namespace Migration
{
	// attribute that assigns custom id for classes and structs that are serialized but not migrated
	[System.AttributeUsage(System.AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
	public class NonMigratableIdAttribute : Attribute
	{
		public readonly string CustomId;
		public NonMigratableIdAttribute(string customId)
		{
			CustomId = customId;
		}
	}
}