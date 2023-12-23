//#define MIGRATION_SERIALIZER_LOG
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using OdinSerializer;

namespace Migration
{
	public static class MigrationSerializer
	{
		private static Assembly ThisAssembly => typeof(MigrationSerializer).Assembly;
		
		// serialize data with given format
		public static async Task<byte[]> SerializeAsync<TData>(TData data, SerializedDataFormat data_format, Assembly assembly = null)
		{
			byte[] serialized_data = SerializeData(data, data_format, assembly);
			
			if (data_format.IsGZip())
			{
				serialized_data = await GZipWrapper.CompressAsync(serialized_data);
			}

			return serialized_data;
		}

		public static byte[] Serialize<TData>(TData data, SerializedDataFormat data_format, Assembly assembly = null)
		{
			byte[] serialized_data = SerializeData(data, data_format, assembly);
			
			if (data_format.IsGZip())
			{
				serialized_data = GZipWrapper.Compress(serialized_data);
			}

			return serialized_data;
		}
		
		// deserialize data from input, types_hash must be stored separately and it's the same hash the MigrationManager provides
		public static async Task<TData> DeserializeAsync<TData, TDataMigratable>(
			byte[]               types_hash,
			byte[]               input,
			SerializedDataFormat data_format,
			MigrationManager     migration_manager,
			bool                 force_migration = false,
			Assembly             assembly        = null
		)
		{
			if (data_format.IsGZip())
			{
				input = await GZipWrapper.DecompressAsync(input);
			}

			return DeserializeData<TData, TDataMigratable>(types_hash, input, data_format, migration_manager, force_migration, assembly);
		}

		public static TData Deserialize<TData, TDataMigratable>(
			byte[]               types_hash,
			byte[]               input,
			SerializedDataFormat data_format,
			MigrationManager     migration_manager,
			bool                 force_migration = false,
			Assembly             assembly        = null
		)
		{
			if (data_format.IsGZip())
			{
				input = GZipWrapper.Decompress(input);
			}

			return DeserializeData<TData, TDataMigratable>(types_hash, input, data_format, migration_manager, force_migration, assembly);
		}
		
		//-------------------------------------------------------------------------------------------------------------------
		private static byte[] SerializeData<TData>(TData data, SerializedDataFormat data_format, Assembly assembly = null)
		{
			BaseSerializationBinder model_serialization_binder = new DataSerializationBinder(assembly ?? ThisAssembly);
			SerializationContext model_serialization_context = new SerializationContext
			                                                   {
				                                                   Binder = model_serialization_binder
			                                                   };

			byte[] serialized_data = SerializationUtility.SerializeValue(data, data_format.ToOdinDataFormat(), model_serialization_context);
			return serialized_data;
		}

		//-------------------------------------------------------------------------------------------------------------------
		private static TData DeserializeData<TData, TDataMigratable>(
			byte[]               types_hash,
			byte[]               input,
			SerializedDataFormat data_format,
			MigrationManager     migration_manager,
			bool                 force_migration = false,
			Assembly             assembly        = null
		)
		{
			if (force_migration || !migration_manager.TypesHash.SequenceEqual(types_hash))
			{
				#if MIGRATION_SERIALIZER_LOG
				Debug.Log("Migrating data");
				#endif

				BaseSerializationBinder migration_serialization_binder = new MigrationSerializationBinder(assembly ?? ThisAssembly);
				#if MIGRATION_SERIALIZER_LOG
				migration_serialization_binder.DebugDump();
				#endif
				
				DeserializationContext migration_deserialization_context = new DeserializationContext
				                                                           {
					                                                           Binder = migration_serialization_binder
				                                                           };

				TDataMigratable migratable_data = SerializationUtility.DeserializeValue<TDataMigratable>(input, data_format.ToOdinDataFormat(), migration_deserialization_context);
				if (migratable_data == null)
				{
					return default;
				}

				migratable_data = migration_manager.MigrateRoot(migratable_data);
					
				SerializationContext migration_serialization_context = new SerializationContext
				                                                       {
					                                                       Binder = migration_serialization_binder
				                                                       };

				data_format = SerializedDataFormat.Binary;
				input       = SerializationUtility.SerializeValue(migratable_data, data_format.ToOdinDataFormat(), migration_serialization_context);

				#if MIGRATION_SERIALIZER_LOG
				Debug.Log("Migration complete");
				#endif
			}

			// final deserialization
			#if MIGRATION_SERIALIZER_LOG
			Debug.Log("Deserializing data");
			#endif
			
			BaseSerializationBinder model_serialization_binder = new DataSerializationBinder(assembly ?? ThisAssembly);
			DeserializationContext model_deserialization_context = new DeserializationContext
			                                                       {
				                                                       Binder = model_serialization_binder
			                                                       };

			TData deserialized_data = SerializationUtility.DeserializeValue<TData>(input, data_format.ToOdinDataFormat(), model_deserialization_context);
			return deserialized_data;
		}
	}
}