using System;
using System.Collections.Generic;
using OdinSerializer;
using OdinSerializer.Utilities;

namespace Migration
{
	// Base universal binder that from dictionary of types mapped to each other creates two way mappings for both serialization and deserialization.
	public abstract class BaseSerializationBinder : TwoWaySerializationBinder
	{
		private Dictionary<Type, string> m_BindTypeToName;
		private Dictionary<string, Type> m_BindNameToType;

		private static Dictionary<Type, string> m_TypeToNameCache;
		
		protected BaseSerializationBinder()
		{
		}

		protected BaseSerializationBinder(Dictionary<Type,Type> type_to_type)
		{
			Init(type_to_type);
		}

		public void Init(Dictionary<Type, Type> type_to_type)
		{
			m_BindTypeToName = new Dictionary<Type, string>();
			m_BindNameToType = new Dictionary<string, Type>();
			
			foreach (KeyValuePair<Type, Type> pair in type_to_type)
			{
				string name = TypeToName(pair.Value);
				m_BindTypeToName.Add(pair.Key, name);
				m_BindNameToType.Add(name, pair.Key);
			}

			//DebugDump();
		}

		private string TypeToName(Type type) 
		{
			if (m_TypeToNameCache == null)
			{
				m_TypeToNameCache = new Dictionary<Type, string>();
			}

			if (m_TypeToNameCache.TryGetValue(type, out string cached_name))
			{
				return cached_name;
			}

			string name = GetTypeNameOrCustomId(type);
			
			if (type.GenericTypeArguments.Length > 0)
			{
				name += "<";
				for (int i = 0; i < type.GenericTypeArguments.Length; ++i)
				{
					if (i > 0)
					{
						name += ",";
					}

					name += TypeToName(type.GenericTypeArguments[i]);
				}

				name += ">";
			}

			m_TypeToNameCache.Add(type, name);
			return name;
		}

		private string GetTypeNameOrCustomId(Type type)
		{
			{
				LatestVersionAttribute latest_version = type.GetAttribute<LatestVersionAttribute>(false);
				if (latest_version?.CustomId != null)
				{
					if (latest_version.IsVersionValid)
					{
						return $"{latest_version.CustomId}_{latest_version.Version}";
					}

					MigrationManager.SplitTypeName(type.Name, out _, out int version);
					return $"{latest_version.CustomId}_{version}";
				}
			}

			{
				ObsoleteVersionAttribute obsolete_version = type.GetAttribute<ObsoleteVersionAttribute>(false);
				if (obsolete_version?.CustomId != null)
				{
					if (obsolete_version.IsVersionValid)
					{
						return $"{obsolete_version.CustomId}_{obsolete_version.Version}";
					}

					MigrationManager.SplitTypeName(type.Name, out _, out int version);
					return $"{obsolete_version.CustomId}_{version}";
				}
			}

			{
				MigratableInterfaceAttribute migratable_interface = type.GetAttribute<MigratableInterfaceAttribute>(false);
				if (migratable_interface?.CustomId != null)
				{
					return migratable_interface.CustomId;
				}
			}

			return type.Namespace + "." + type.Name;
		}

		public override string BindToName(Type type, DebugContext debugContext = null)
		{
			if (m_BindTypeToName.TryGetValue(type, out string name))
			{
				return name;
			}

			return Default.BindToName(type, debugContext);
		}

		public override Type BindToType(string typeName, DebugContext debugContext = null)
		{
			if (m_BindNameToType.TryGetValue(typeName, out Type type))
			{
				return type;
			}

			return Default.BindToType(typeName, debugContext);
		}

		public override bool ContainsType(string typeName)
		{
			return m_BindNameToType.ContainsKey(typeName) || Default.ContainsType(typeName); 
		}

		public void DebugDump()
		{
			foreach (var pair in m_BindNameToType)
			{
				MigrationPlatform.Log($"Mapping: {pair.Key} -> {pair.Value.FullName}");
			}
		}
	}
}