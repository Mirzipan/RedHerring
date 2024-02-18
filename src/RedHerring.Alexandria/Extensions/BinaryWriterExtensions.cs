using RedHerring.Numbers;
using Silk.NET.Maths;

namespace RedHerring.Alexandria.Extensions;

// Alexandria should not be dependent on Silk.NET.Maths 
public static class BinaryWriterExtensions
{
	public static void Write(this BinaryWriter writer, Vector3D<float> vector)
	{
		writer.Write(vector.X);
		writer.Write(vector.Y);
		writer.Write(vector.Z);
	}

	public static void Write(this BinaryWriter writer, Vector2D<float> vector)
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