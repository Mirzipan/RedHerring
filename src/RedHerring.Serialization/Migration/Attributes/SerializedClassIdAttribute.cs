namespace Migration
{
	// Custom id for serializable class.
	// Class can be renamed or moved to other namespace without breaking serialization.
	[System.AttributeUsage(System.AttributeTargets.Class, Inherited = false)]
	public sealed class SerializedClassIdAttribute : System.Attribute
	{
		public readonly string Id;
		public SerializedClassIdAttribute(string id)
		{
			Id = id;
		}
	}
}