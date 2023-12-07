using Assimp;
using Assimp.Configs;
using OdinSerializer;
using RedHerring.Render.Models;
using Silk.NET.Maths;
using Mesh = RedHerring.Render.Models.Mesh;

namespace RedHerring.Studio.Models.Project.Importers;

[Importer(".fbx", ".obj")]
public class SceneImporter : AssetImporter<SceneImporterSettings>
{
	protected override SceneImporterSettings CreateImporterSettings() => new();
	
	protected override void Import(Stream stream, SceneImporterSettings settings, string resourcePath, CancellationToken cancellationToken)
	{
		AssimpContext context = new();
		context.SetConfig(new NormalSmoothingAngleConfig(66.0f)); // just for testing

		Scene scene = context.ImportFileFromStream(stream,
			PostProcessSteps.Triangulate
		);
		
		if(!scene.HasMeshes)
		{
			return;
		}
		
		Directory.CreateDirectory(Path.GetDirectoryName(resourcePath)!);

		// TODO - far from finished
		foreach (Assimp.Mesh? assimpMesh in scene.Meshes)
		{
			Model model = new();
			
			model.Vertices = new(assimpMesh.VertexCount);
			foreach (Assimp.Vector3D vertex in assimpMesh.Vertices)
			{
				model.Vertices.Add(new Vector3D<float>(vertex.X, vertex.Y, vertex.Z));
			}

			model.Normals = new(assimpMesh.VertexCount);
			foreach (Assimp.Vector3D normal in assimpMesh.Normals)
			{
				model.Normals.Add(new Vector3D<float>(normal.X, normal.Y, normal.Z));
			}

			model.Indices = new(assimpMesh.Faces.Count * 3);
			foreach(Assimp.Face face in assimpMesh.Faces)
			{
				model.Indices.Add(face.Indices[0]);
				model.Indices.Add(face.Indices[1]);
				model.Indices.Add(face.Indices[2]);
			}

			Mesh mesh = new()
			            {
				            Name          = assimpMesh.Name,
				            MaterialIndex = 0,
				            IndexStart    = 0,
				            TriStart      = 0,
				            TriCount      = model.Indices.Count / 3
			            };
			model.Meshes = new() { mesh };
			
			byte[] json = SerializationUtility.SerializeValue(model, DataFormat.JSON);
			File.WriteAllBytes($"{resourcePath}_{mesh.Name}_.mesh", json);
		}
	}
}