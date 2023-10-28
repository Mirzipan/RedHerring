//#define MIGRATION_LOG

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Migration
{
	public sealed class MigrationManager
	{
		private sealed class ClassVersion
		{
			public readonly string ClassName;
			public readonly int    IntVersion;
			public readonly bool   IsLatestByAttribute;

			public ClassVersion(string class_name, int int_version, bool is_latest_by_attribute)
			{
				ClassName           = class_name;
				IntVersion          = int_version;
				IsLatestByAttribute = is_latest_by_attribute;
			}
		}

		// marker for end of chain - latest version
		private struct LatestVersionType
		{
		}

		// marker for end of chain - obsolete version
		private struct ObsoleteVersionType
		{
		}

		private Dictionary<string,Type> m_Types;
		private Dictionary<Type, Type>  m_NextTypeMapping; // maps any type to it's next version, end of chain is mapped to LatestVersionType or ObsoleteVersionType

		private byte[] m_TypesHash;
		public  byte[] TypesHash => m_TypesHash;
		
		public MigrationManager(Assembly assembly = null)
		{
			Init(assembly);
			CreateTypesHash();
		}

		public T MigrateRoot<T>(T root)
		{
			Migrate(root);
			return (T)MigrateValueToLatestVersion(root, root.GetType());
		}

		public void Migrate(object data)
		{
			Type data_type = data.GetType();

			if (data_type.IsGenericType && data_type.GetGenericTypeDefinition() == typeof(List<>))
			{
				MigrateList(data);
				return;
			}

			MigrateAnyType(data);
		}

		private void MigrateList(object data)
		{
			Type data_type = data.GetType();

			PropertyInfo count_property   = data_type.GetProperty("Count");
			PropertyInfo indexer_property = data_type.GetProperty("Item"); // default name of Indexer property

			int      count = (int)count_property.GetValue(data);
			object[] index = new object[] {0};
			for (int i = 0; i < count; ++i)
			{
				index[0] = i;
				object value = indexer_property.GetValue(data, index);

				if (value == null)
				{
					continue;
				}

				Type value_type = value.GetType();

				if (m_NextTypeMapping.ContainsKey(value_type))
				{
					Migrate(value);
				}

				value = MigrateValueToLatestVersion(value, value_type);
				if (value != null)
				{
					indexer_property.SetValue(data, value, index);
				}
			}
		}

		private void MigrateAnyType(object data)
		{
			Type data_type = data.GetType();

			FieldInfo[] fields = data_type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
			foreach (FieldInfo field in fields)
			{
				object field_object = field.GetValue(data);
				if (field_object == null)
				{
					continue;
				}

				Type field_object_type = field_object.GetType();

				if (
					field.GetCustomAttribute<MigrateFieldAttribute>(false) != null
					||
					m_NextTypeMapping.ContainsKey(field_object_type)
				)
				{
					Migrate(field_object);
				}

				field_object = MigrateValueToLatestVersion(field_object, field_object_type);
				if (field_object != null)
				{
					field.SetValue(data, field_object);
				}
			}
		}

		private object MigrateValueToLatestVersion(object value, Type value_type)
		{
			if (m_NextTypeMapping.TryGetValue(value_type, out Type next_type))
			{
				if (next_type == typeof(LatestVersionType))
				{
					// already latest version
					return null;
				}
					
				// migrate
				while (m_NextTypeMapping.TryGetValue(value_type, out next_type) && next_type != typeof(LatestVersionType))
				{
#if MIGRATION_LOG
					Debug.Log($"Migrating {value.GetType().Name} -> {next_type.Name}");
#endif
					
					// in any case - get allocate next function
					MethodInfo allocate_method = value.GetType().GetMethod("AllocateNext");

					object     next_value;
					if (next_type == typeof(ObsoleteVersionType) || allocate_method != null)
					{
						// obsolete but with undefined next type or obsolete with AllocateNext method -> call AllocateNext and determine next type
						if (allocate_method == null)
						{
							MigrationPlatform.LogError($"Cannot migrate class {value_type.Name}, public method 'object AllocateNext()' is missing!");
							break;
						}

						next_value = allocate_method.Invoke(value, null);
						next_type  = next_value.GetType();
					}
					else
					{
						// obsolete with defined next type and without AllocateNext method -> allocate next and call Migrate
						next_value     = Activator.CreateInstance(next_type);
						MethodInfo migrate_method = next_value.GetType().GetMethod("Migrate", new[] {value_type});
						if (migrate_method == null)
						{
							MigrationPlatform.LogError($"Cannot migrate class {value_type.Name} to {next_type.Name}, public method 'void Migrate({value_type.Name})' is missing!");
							break;
						}

						migrate_method.Invoke(next_value, new[] {value});
					}

					// move to next
					value      = next_value;
					value_type = next_type;
				}

				return value;
			}
		
			return null;
		}

		private void Init(Assembly assembly = null)
		{
			assembly ??= GetType().Assembly;

			ScanMigratableTypes(assembly);
			BuildNextTypeMapping();

#if MIGRATION_LOG
			foreach (KeyValuePair<Type, Type> pair in m_NextTypeMapping)
			{
				Debug.Log($"Migratable type {pair.Key.Name} -> {(pair.Value == null ? "null" : pair.Value.Name)}");
			}
#endif
		}
		
		private void CreateTypesHash()
		{
			using (SHA1Managed sha1 = new SHA1Managed())
			{
				foreach (KeyValuePair<string, Type> pair in m_Types)
				{
					byte[] key_bytes = Encoding.ASCII.GetBytes(pair.Key);
					sha1.TransformBlock(key_bytes, 0, key_bytes.Length, null, 0);
				}

				sha1.TransformFinalBlock(new byte[] {0}, 0, 1);
				m_TypesHash = sha1.Hash;
			}
		}
		
		private void ScanMigratableTypes(Assembly assembly)
		{
			m_Types = new Dictionary<string,Type>();
			
			IEnumerable<Type> types = GetTypesWithAttribute(assembly, typeof(MigratableInterfaceAttribute));
			foreach (Type type in types)
			{
				ObtainAllAssignableTypes(assembly, type);
			}
		}

		private void ObtainAllAssignableTypes(Assembly assembly, Type migratableType)
		{
			IEnumerable<Type> migratable_classes = GetClassesAssignableTo(assembly, migratableType);
			foreach (Type migratable_class in migratable_classes)
			{
				if (migratable_class.GetCustomAttribute<NonMigratableClassAttribute>(false) != null)
				{
					continue;
				}

				if (!m_Types.ContainsKey(migratable_class.Name))
				{
					m_Types.Add(migratable_class.Name, migratable_class);
#if MIGRATION_LOG
					Debug.Log("Migratable class found: " + migratable_class.Name);
#endif
				}
			}
		}

		private void BuildNextTypeMapping()
		{
			// parse all type names and store max version for each of them
			Dictionary<string, List<ClassVersion>> class_versions = new (); // all versions for each class
			foreach(Type type in m_Types.Values)
			{
				if (!ExtractVersionFromType(type, out string name, out int int_version, out bool is_latest_by_attribute))
				{
					MigrationPlatform.LogWarning($"Class {type.Name} has invalid migratable class name format. Name should be CLASSNAME_VERSION.");
					continue;
				}

				if (class_versions.TryGetValue(name, out List<ClassVersion> versions))
				{
					versions.Add(new ClassVersion (type.Name, int_version, is_latest_by_attribute));
				}
				else
				{
					class_versions.Add(name, new List<ClassVersion>{new (type.Name, int_version, is_latest_by_attribute)});
				}
			}
			
			// sort versions
			foreach (List<ClassVersion> versions in class_versions.Values)
			{
				versions.Sort((x, y) => x.IntVersion - y.IntVersion);
			}

			// create mapping from each type to it's next version
			m_NextTypeMapping = new Dictionary<Type, Type>();
			foreach (Type type in m_Types.Values)
			{
				if (!ExtractVersionFromType(type, out string name, out int int_version, out bool is_latest_by_attribute))
				{
					continue;
				}

				// next version
				List<ClassVersion> this_class_versions      = class_versions[name];
				int                next_class_version_index = this_class_versions.FindIndex(x => x.IntVersion == int_version) + 1;

				if (next_class_version_index >= this_class_versions.Count)
				{
					// there is no next type found, so there are two options:
					//  - class is latest
					//  - class is obsolete and needs AllocateNext function
					MigrationPlatform.AssertIsTrue(is_latest_by_attribute || type.GetMethod("AllocateNext") != null, $"{type} : There is no type found for next version, but class is not marked as latest and does not have AllocateNext function!");
					m_NextTypeMapping.Add(type, is_latest_by_attribute ? typeof(LatestVersionType) : typeof(ObsoleteVersionType));
				}
				else
				{
					MigrationPlatform.AssertIsFalse(is_latest_by_attribute, $"{type} : Class that is not latest cannot be marked as latest with attribute!");
					m_NextTypeMapping.Add(type, m_Types[this_class_versions[next_class_version_index].ClassName]);
				}
			}
		}

		//------------ utils ------------------
		public static bool ExtractVersionFromType(Type type, out string name, out int version, out bool is_latest_by_attribute)
		{
			is_latest_by_attribute = false;
			
			{
				// try latest version attribute
				LatestVersionAttribute latest_version = type.GetCustomAttribute<LatestVersionAttribute>();
				if (latest_version != null)
				{
					is_latest_by_attribute = true;
					if (latest_version.IsVersionValid)
					{
						name    = latest_version.DataClass.Name; // cannot use type.Name because we didn't strip any version information from it
						version = latest_version.Version;
						return true;
					}
				}
			}

			{
				// try obsolete version attribute
				ObsoleteVersionAttribute obsolete_version = type.GetCustomAttribute<ObsoleteVersionAttribute>();
				if (obsolete_version != null && obsolete_version.IsVersionValid)
				{
					name           = obsolete_version.DataClass.Name; // cannot use type.Name because we didn't strip any version information from it
					version        = obsolete_version.Version;
					return true;
				}
			}
			
			// no valid attribute found - extract version from the class name
			return SplitTypeName(type.Name, out name, out version);
		}

		public static bool SplitTypeName(string type_name, out string name, out int version)
		{
			name           = null;
			version        = -1;

			Regex regex = new Regex(@"(.*)_([0-9]+)$");
			Match match = regex.Match(type_name);
			if (!match.Success)
			{
				return false;
			}

			if (match.Groups.Count != 3)
			{
				return false;
			}

			if (!int.TryParse(match.Groups[2].Value, out int version_int))
			{
				return false;
			}

			name           = match.Groups[1].Value;
			//string_version = match.Groups[2].Value;
			version        = version_int;
			return true;
		}

		public static IEnumerable<Type> GetTypesWithAttribute(Assembly assembly, Type attribute_type)
		{
			return assembly.GetTypes().Where(type => Attribute.IsDefined(type, attribute_type));
		}

		public static IEnumerable<Type> GetClassesAssignableTo(Assembly assembly, Type type)
		{
			return assembly.GetTypes()
				.Where(assemblyType =>
					assemblyType.IsClass && !assemblyType.IsAbstract && type.IsAssignableFrom(assemblyType));
		}
	}
}