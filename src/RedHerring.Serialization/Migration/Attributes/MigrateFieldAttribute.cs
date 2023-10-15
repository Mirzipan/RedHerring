namespace Migration
{
	// Attribute to force any field for migration check. 
	// 	- useful for types that are not migratable themselves but contain migratable class
	// 	- Lists are not migratable, but can contain migratable classes and needs to be marked as migratable as well
	// 	- for example:  [SerializeReference, MigratableField] List<ISomethingMigratable> 
	[System.AttributeUsage(System.AttributeTargets.Field)]
	public sealed class MigrateFieldAttribute : System.Attribute
	{
		
	}
}