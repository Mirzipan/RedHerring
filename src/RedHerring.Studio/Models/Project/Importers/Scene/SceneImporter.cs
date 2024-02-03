using Assimp;
using Assimp.Configs;
using Migration;
using OdinSerializer;
using RedHerring.Numbers;
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
		context.SetConfig(new NormalSmoothingAngleConfig(settings.NormalSmoothingAngle));

		Scene scene = context.ImportFileFromStream(stream,
			PostProcessSteps.Triangulate
		);
		
		if(!scene.HasMeshes)
		{
			return ImporterResult.Finished;
		}
		
		Directory.CreateDirectory(Path.GetDirectoryName(resourcePath)!);

		Model model = new();
		
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
			// if (!settings.Meshes[i].Import)
			// {
			// 	continue;
			// }

			// import
			Mesh  mesh  = new();
			mesh.Name = assimpMesh.Name;
			model.Meshes.Add(mesh);

			// positions
			if (assimpMesh.HasVertices)
			{
				mesh.Positions = assimpMesh.Vertices.Select(v => new Vector3D<float>(v.X, v.Y, v.Z)).ToList();
			}

			// normals
			if (assimpMesh.HasNormals)
			{
				mesh.Normals = assimpMesh.Normals.Select(n => new Vector3D<float>(n.X, n.Y, n.Z)).ToList();
			}
			
			// tangents and bitangents
			if (assimpMesh.HasTangentBasis)
			{
				mesh.Tangents   = assimpMesh.Tangents.Select(t => new Vector3D<float>(t.X,   t.Y, t.Z)).ToList();
				mesh.BiTangents = assimpMesh.BiTangents.Select(b => new Vector3D<float>(b.X, b.Y, b.Z)).ToList();
			}

			// UV
			if (assimpMesh.TextureCoordinateChannelCount > 0)
			{
				mesh.TextureCoordinateChannels = new List<MeshTextureCoordinateChannel>();
				for (int channelIndex = 0; channelIndex < assimpMesh.TextureCoordinateChannelCount; ++channelIndex)
				{
					MeshTextureCoordinateChannel channel = new ();
					
					if (assimpMesh.UVComponentCount[channelIndex] == 2)
					{
						channel.UV = assimpMesh.TextureCoordinateChannels[channelIndex].Select(uv => new Vector2D<float>(uv.X, uv.Y)).ToList();
					}
					else if (assimpMesh.UVComponentCount[channelIndex] == 3)
					{
						channel.UVW = assimpMesh.TextureCoordinateChannels[channelIndex].Select(uv => new Vector3D<float>(uv.X, uv.Y, uv.Z)).ToList();
					}
					else
					{
						continue;
					}

					mesh.TextureCoordinateChannels.Add(channel);
				}
			}
			
			// colors
			if (assimpMesh.VertexColorChannelCount > 0)
			{
				mesh.VertexColorChannels = new List<MeshVertexColorChannel>();
				for (int channelIndex = 0; channelIndex < assimpMesh.VertexColorChannelCount; ++channelIndex)
				{
					MeshVertexColorChannel channel = new();
					mesh.VertexColorChannels.Add(channel);
					channel.Colors = assimpMesh.VertexColorChannels[channelIndex].Select(color => new Color4(color.R, color.G, color.B, color.A)).ToList();
				}
			}
			
			// TODO - rest: bones, animation, morphing

			// faces
			if (assimpMesh.HasFaces)
			{
				if (mesh.Positions is not null && mesh.Positions.Count <= 0xffff)
				{
					mesh.UShortIndices = assimpMesh.GetUnsignedIndices().Select(idx => (ushort)idx).ToList();
				}
				else
				{
					mesh.UIntIndices = assimpMesh.GetUnsignedIndices().ToList();
				}
			}
		}

		byte[] json = SerializationUtility.SerializeValue(model, DataFormat.Binary);
		File.WriteAllBytes($"{resourcePath}.scene", json);

		// cut the rest
		if (settings.Meshes.Count > scene.Meshes.Count)
		{
			settings.Meshes.RemoveRange(scene.Meshes.Count, settings.Meshes.Count - scene.Meshes.Count);
			settingsChanged = true;
		}

		return settingsChanged ? ImporterResult.FinishedSettingsChanged : ImporterResult.Finished;
	}
}