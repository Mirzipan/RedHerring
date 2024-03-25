using System.Numerics;
using RedHerring.Numbers;

namespace RedHerring.Alexandria.Extensions;

// Alexandria should not be dependent on Silk.NET.Maths 
public static class BinaryWriterExtensions
{
	public static void Write(this BinaryWriter writer, Vector3 vector)
	{
		writer.Write(vector.X);
		writer.Write(vector.Y);
		writer.Write(vector.Z);
	}

	public static void Write(this BinaryWriter writer, Vector2 vector)
	{
		writer.Write(vector.X);
		writer.Write(vector.Y);
	}

	public static void Write(this BinaryWriter writer, Color4 color)
	{
		writer.Write(color.R);
		writer.Write(color.G);
		writer.Write(color.B);
		writer.Write(color.A);
	}
}