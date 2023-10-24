using Assimp;

namespace RedHerring.Studio.Models.Tests;

public class ImporterTests
{
	public static void Test()
	{
		Console.WriteLine("Import started");

		AssimpContext importer = new();
		Scene?        scene    = importer.ImportFile("f:\\git\\_testProject2\\Assets\\sword_2h_RustyIronLongSword.fbx", PostProcessSteps.Triangulate);

		foreach (Mesh? mesh in scene.Meshes)
		{
			Console.WriteLine(mesh.Name);

			mesh.Vertices.ForEach(v => Console.WriteLine($"v: {v.X} {v.Y} {v.Z}"));
		}

		Console.WriteLine("Import finished");
	}
}