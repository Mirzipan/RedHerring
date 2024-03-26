using Assimp;
using Assimp.Configs;
using Migration;
using OdinSerializer;
using RedHerring.Numbers;
using RedHerring.Render;
using RedHerring.Render.Models;
using Silk.NET.Maths;
using Scene = RedHerring.Render.Models.Scene;
using Vector3D = Assimp.Vector3D;

namespace RedHerring.Studio.Models.Project.Importers;

[Importer(".fbx", ".obj")]
public class SceneImporter : AssetImporter<SceneImporterSettings>
{
	protected override SceneImporterSettings CreateImporterSettings() => new();
	
	protected override ImporterResult Import(Stream stream,            SceneImporterSettings settings, string resourcePath, MigrationManager migrationManager,
		CancellationToken                           cancellationToken, out string            referenceClassName)
	{
		referenceClassName = nameof(SceneReference);
		
		AssimpContext context = new();
		context.SetConfig(new NormalSmoothingAngleConfig(settings.NormalSmoothingAngle));

		Assimp.Scene assimpScene = context.ImportFileFromStream(stream,
			PostProcessSteps.Triangulate
		);
		
		if(!assimpScene.HasMeshes)
		{
			return ImporterResult.Finished;
		}
		
		Directory.CreateDirectory(Path.GetDirectoryName(resourcePath)!);

		Scene scene = new();

		// meshes ---------------------------------------
		bool settingsChanged = false;
		for (int i = 0; i < assimpScene.Meshes.Count; ++i)
		{
			Mesh assimpMesh = assimpScene.Meshes[i];

			// update settings
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
			SceneMesh  sceneMesh  = new();
			sceneMesh.Name = assimpMesh.Name;
			scene.Meshes.Add(sceneMesh);

			// positions
			if (assimpMesh.HasVertices)
			{
				sceneMesh.Positions = assimpMesh.Vertices.Select(v => new Vector3D<float>(v.X, v.Y, v.Z)).ToList();
			}
			else
			{
				sceneMesh.Positions = new List<Vector3D<float>>();
			}

			// normals
			if (assimpMesh.HasNormals)
			{
				sceneMesh.Normals = assimpMesh.Normals.Select(n => new Vector3D<float>(n.X, n.Y, n.Z)).ToList();
			}
			
			// tangents and bitangents
			if (assimpMesh.HasTangentBasis)
			{
				sceneMesh.Tangents   = assimpMesh.Tangents.Select(t => new Vector3D<float>(t.X,   t.Y, t.Z)).ToList();
				sceneMesh.BiTangents = assimpMesh.BiTangents.Select(b => new Vector3D<float>(b.X, b.Y, b.Z)).ToList();
			}

			// UV
			if (assimpMesh.TextureCoordinateChannelCount > 0)
			{
				sceneMesh.TextureCoordinateChannels = new List<SceneMeshTextureCoordinateChannel>();
				for (int channelIndex = 0; channelIndex < assimpMesh.TextureCoordinateChannelCount; ++channelIndex)
				{
					SceneMeshTextureCoordinateChannel channel = new ();
					
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

					sceneMesh.TextureCoordinateChannels.Add(channel);
				}
			}
			
			// colors
			if (assimpMesh.VertexColorChannelCount > 0)
			{
				sceneMesh.VertexColorChannels = new List<SceneMeshVertexColorChannel>();
				for (int channelIndex = 0; channelIndex < assimpMesh.VertexColorChannelCount; ++channelIndex)
				{
					SceneMeshVertexColorChannel channel = new();
					sceneMesh.VertexColorChannels.Add(channel);
					channel.Colors = assimpMesh.VertexColorChannels[channelIndex].Select(color => new Color4(color.R, color.G, color.B, color.A)).ToList();
				}
			}
			
			// TODO - rest: bones, animation, morphing

			// faces
			if (assimpMesh.HasFaces)
			{
				if (sceneMesh.Positions is not null && sceneMesh.Positions.Count <= 0xffff)
				{
					sceneMesh.UShortIndices = assimpMesh.GetUnsignedIndices().Select(idx => (ushort)idx).ToArray();
				}
				else
				{
					sceneMesh.UIntIndices = assimpMesh.GetUnsignedIndices().ToArray();
				}
			}
		}

		// hierarchy ----------------------------------------
		if (assimpScene.RootNode != null)
		{
			scene.Root = new SceneNode();
			ImportNode(scene.Root, assimpScene.RootNode);
			ImportChildNodesRecursive(scene.Root, assimpScene.RootNode);
		}
		
		// import
		byte[] json = SerializationUtility.SerializeValue(scene, DataFormat.Binary);
		File.WriteAllBytes($"{resourcePath}.scene", json);

		// cut the rest
		if (settings.Meshes.Count > assimpScene.Meshes.Count)
		{
			settings.Meshes.RemoveRange(assimpScene.Meshes.Count, settings.Meshes.Count - assimpScene.Meshes.Count);
			settingsChanged = true;
		}

		return settingsChanged ? ImporterResult.FinishedSettingsChanged : ImporterResult.Finished;
	}

	private void ImportChildNodesRecursive(SceneNode targetNode, Node source)
	{
		if (source.Children == null)
		{
			return;
		}

		foreach (Node sourceChild in source.Children)
		{
			targetNode.Children ??= new List<SceneNode>();

			SceneNode child = new();
			ImportNode(child, sourceChild);
			targetNode.Children.Add(child);

			ImportChildNodesRecursive(child, sourceChild);
		}
	}

	private void ImportNode(SceneNode targetNode, Node source)
	{
		targetNode.Name = source.Name;

		source.Transform.Decompose(out Vector3D scale, out Quaternion rotation, out Vector3D translation);
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