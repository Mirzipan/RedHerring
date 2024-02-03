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

		Scene assimpScene = context.ImportFileFromStream(stream,
			PostProcessSteps.Triangulate
		);
		
		if(!assimpScene.HasMeshes)
		{
			return ImporterResult.Finished;
		}
		
		Directory.CreateDirectory(Path.GetDirectoryName(resourcePath)!);

		Model model = new();

		// meshes ---------------------------------------
		bool settingsChanged = false;
		for (int i = 0; i < assimpScene.Meshes.Count; ++i)
		{
			// update settings
			Assimp.Mesh assimpMesh = assimpScene.Meshes[i];
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

		// hierarchy ----------------------------------------
		if (assimpScene.RootNode != null)
		{
			model.Root = new ModelNode();
			ImportNode(model.Root, assimpScene.RootNode);
			ImportChildNodesRecursive(model.Root, assimpScene.RootNode);
		}
		
		// import
		byte[] json = SerializationUtility.SerializeValue(model, DataFormat.Binary);
		File.WriteAllBytes($"{resourcePath}.scene", json);

		// cut the rest
		if (settings.Meshes.Count > assimpScene.Meshes.Count)
		{
			settings.Meshes.RemoveRange(assimpScene.Meshes.Count, settings.Meshes.Count - assimpScene.Meshes.Count);
			settingsChanged = true;
		}

		return settingsChanged ? ImporterResult.FinishedSettingsChanged : ImporterResult.Finished;
	}

	private void ImportChildNodesRecursive(ModelNode targetNode, Assimp.Node source)
	{
		if (source.Children == null)
		{
			return;
		}

		foreach (Node sourceChild in source.Children)
		{
			targetNode.Children ??= new List<ModelNode>();

			ModelNode child = new();
			ImportNode(child, sourceChild);
			targetNode.Children.Add(child);

			ImportChildNodesRecursive(child, sourceChild);
		}
	}

	private void ImportNode(ModelNode targetNode, Assimp.Node source)
	{
		targetNode.Name = source.Name;

		source.Transform.Decompose(out Assimp.Vector3D scale, out Assimp.Quaternion rotation, out Assimp.Vector3D translation);
		targetNode.Translation = new Vector3D<float>(translation.X, translation.Y, translation.Z);
		targetNode.Rotation    = new Quaternion<float>(rotation.X, rotation.Y, rotation.Z, rotation.W);
		targetNode.Scale       = new Vector3D<float>(scale.X, scale.Y, scale.Z);

		if (source.HasMeshes)
		{
			targetNode.MeshIndices = new List<int>();
			for (int i = 0; i < source.MeshIndices.Count; ++i)
			{
				targetNode.MeshIndices.Add(source.MeshIndices[i]);
			}
		}
	}
}