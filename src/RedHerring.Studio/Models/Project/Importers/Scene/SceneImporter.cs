using Assimp;
using Assimp.Configs;
using Migration;
using OdinSerializer;
using RedHerring.Render.Models;
using Silk.NET.Maths;
using Mesh = RedHerring.Render.Models.Mesh;

namespace RedHerring.Studio.Models.Project.Importers;

[Importer(".fbx", ".obj")]
public class SceneImporter : AssetImporter<SceneImporterSettings>
{
	protected override SceneImporterSettings CreateImporterSettings() => new();
	
	protected override ImporterResult Import(Stream stream, SceneImporterSettings settings, string resourcePath, MigrationManager migrationManager,
		CancellationToken                           cancellationToken)
	{
		AssimpContext context = new();
		context.SetConfig(new NormalSmoothingAngleConfig(66.0f)); // just for testing

		Scene scene = context.ImportFileFromStream(stream,
			PostProcessSteps.Triangulate
		);
		
		if(!scene.HasMeshes)
		{
			return ImporterResult.Finished;
		}
		
		Directory.CreateDirectory(Path.GetDirectoryName(resourcePath)!);

		// TODO - far from finished
		bool settingsChanged = false;
		for (int i = 0; i < scene.Meshes.Count; ++i)
		{
			// update settings
			Assimp.Mesh assimpMesh = scene.Meshes[i];
			if (i == settings.Meshes.Count)
			{
				settings.Meshes.Add(new SceneImporterMeshSettings(assimpMesh.Name));
				settingsChanged = true;
			}
			else if(settings.Meshes[i].Name != assimpMesh.Name)
			{
				settings.Meshes[i] = new SceneImporterMeshSettings(assimpMesh.Name);
				settingsChanged    = true;
			}

			// check
			if (!settings.Meshes[i].Import)
			{
				continue;
			}

			// import
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

		// cut the rest
		if (settings.Meshes.Count > scene.Meshes.Count)
		{
			settings.Meshes.RemoveRange(scene.Meshes.Count, settings.Meshes.Count - scene.Meshes.Count);
			settingsChanged = true;
		}

		return settingsChanged ? ImporterResult.FinishedSettingsChanged : ImporterResult.Finished;
	}
}