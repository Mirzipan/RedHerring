using System;
using System.Collections.Generic;
using System.Reflection;

namespace Migration
{
	// Maps migratable classes to themselves and lists of migratable interfaces to the same lists, just for my custom class naming and custom ids.
	// Includes all versions of all classes.
	
	// For example, class without custom id:
	//   MyClass_001       <-> MyClass_001
	//   MyClass_002       <-> MyClass_002
	//   List<MyClass_001> <-> List<MyClass_001>
	//   List<MyClass_002> <-> List<MyClass_002>

	// And class with custom id:
	//   MyClass_001       <-> custom-id_001
	//   MyClass_002       <-> custom-id_002
	//   List<MyClass_001> <-> List<custom-id_001>
	//   List<MyClass_002> <-> List<custom-id_002>
	public sealed class MigrationSerializationBinder : BaseSerializationBinder
	{
		public MigrationSerializationBinder(Assembly assembly)
		{
			Dictionary<Type, Type> mapping           = new Dictionary<Type, Type>();
			Type                   list_generic_type = typeof (List<>);

			// list types from all interfaces
			{
				IEnumerable<Type> migratable_interfaces = MigrationManager.GetTypesWithAttribute(assembly, typeof(MigratableInterfaceAttribute));
				foreach (Type migratable_interface in migratable_interfaces)
				{
					Type migratable_interface_list = list_generic_type.MakeGenericType(migratable_interface);
					
					mapping.Add(migratable_interface_list, migratable_interface_list);
				}
			}

			// types from all classes
			{
				IEnumerable<Type> migratable_classes = MigrationManager.GetTypesWithAttribute(assembly, typeof(LatestVersionAttribute));
				foreach (Type migratable_class in migratable_classes)
				{
					mapping.Add(migratable_class, migratable_class);
				}
			}

			// types from all obsolete classes
			{
				IEnumerable<Type> migratable_classes = MigrationManager.GetTypesWithAttribute(assembly, typeof(ObsoleteVersionAttribute));
				foreach (Type migratable_class in migratable_classes)
				{
					mapping.Add(migratable_class, migratable_class);
				}
			}
			
			Init(mapping);
		}
	}
}