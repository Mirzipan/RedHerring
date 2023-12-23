using System;
using System.Collections.Generic;
using System.Reflection;

namespace Migration
{
	// Maps data classes to latest versions of migratable classes and to lists of them.
	// Used in transfer between application data and migration data.
	
	// For example, class without custom id:
	//   MyClass       <-> MyClass_003
	//   List<MyClass> <-> List<IMyClassMigratable>
	
	// And if class has custom id:
	//   MyClass       <-> custom-id_003
	//   List<MyClass> <-> List<custom-id>
	public sealed class DataSerializationBinder : BaseSerializationBinder
	{
		public DataSerializationBinder(Assembly assembly)
		{
			Dictionary<Type, Type> mapping           = new ();
			Type                   list_generic_type = typeof (List<>);

			// map interfaces
			{
				IEnumerable<Type> migratable_interfaces = MigrationManager.GetTypesWithAttribute(assembly, typeof(MigratableInterfaceAttribute));
				foreach (Type migratable_interface in migratable_interfaces)
				{
					Type migratable_interface_list = list_generic_type.MakeGenericType(migratable_interface);
					
					MigratableInterfaceAttribute migratable_interface_attribute = migratable_interface.GetCustomAttribute<MigratableInterfaceAttribute>();
					Type                         original_interface_list        = list_generic_type.MakeGenericType(migratable_interface_attribute.DataClass);
					
					if (mapping.ContainsKey(original_interface_list))
					{
						MigrationPlatform.LogError($"Multiple migratable interfaces mapped to the same model class {migratable_interface_attribute.DataClass.FullName}! Only the newest should be mapped!");
						continue;
					}

					mapping.Add(original_interface_list, migratable_interface_list);
				}
			}

			// map classes
			{
				IEnumerable<Type> migratable_classes = MigrationManager.GetTypesWithAttribute(assembly, typeof(LatestVersionAttribute));
				foreach (Type migratable_class in migratable_classes)
				{
					LatestVersionAttribute original_class = migratable_class.GetCustomAttribute<LatestVersionAttribute>();
					if (mapping.ContainsKey(original_class.DataClass))
					{
						MigrationPlatform.LogError($"Multiple migratable classes mapped to the same model class {original_class.DataClass.FullName}! Only the newest should be mapped!");
						continue;
					}

					mapping.Add(original_class.DataClass, migratable_class);
				}
			}
			
			// non migratable classes
			{
				IEnumerable<Type> non_migratable_classes = MigrationManager.GetTypesWithAttribute(assembly, typeof(NonMigratableIdAttribute));
				foreach (Type non_migratable_class in non_migratable_classes)
				{
					mapping.Add(non_migratable_class, non_migratable_class);

					Type non_migratable_class_list = list_generic_type.MakeGenericType(non_migratable_class);
					mapping.Add(non_migratable_class_list, non_migratable_class_list);
				}
			}

			// no obsolete classes

			Init(mapping);
		}
	}
}