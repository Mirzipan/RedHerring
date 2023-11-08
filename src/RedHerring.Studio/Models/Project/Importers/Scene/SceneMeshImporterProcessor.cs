using OdinSerializer;
using RedHerring.Render.Models;
using Silk.NET.Maths;

namespace RedHerring.Studio.Models.Project.Importers;

[ImporterProcessor(typeof(SceneIntermediate))]
public sealed class SceneMeshImporterProcessor : AssetImporterProcessor<SceneIntermediate, SceneImporterSettings>
{
	protected override void Process(SceneIntermediate? intermediate, SceneImporterSettings settings, string resourcePath)
	{
		if (intermediate == null)
		{
			return;
		}
		
		if(!intermediate.Scene.HasMeshes)
		{
			return;
		}
		
		Directory.CreateDirectory(Path.GetDirectoryName(resourcePath)!);

		// TODO - far from finished
		foreach (Assimp.Mesh? assimpMesh in intermediate.Scene.Meshes)
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