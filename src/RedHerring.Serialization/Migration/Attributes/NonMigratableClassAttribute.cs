namespace Migration
{
	// Attribute to remove class that is assignable to migratable interface from migration.
	[System.AttributeUsage(System.AttributeTargets.Class, Inherited = false)]
	public sealed class NonMigratableClassAttribute : System.Attribute
	{
		
	}
}