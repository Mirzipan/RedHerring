using System.Numerics;
using Assimp;
using Assimp.Configs;
using OdinSerializer;
using RedHerring.Render;
using RedHerring.Render.Models;
using RedHerring.Studio.Models.Project.FileSystem;
using RedHerring.Studio.Models.ViewModels.Console;
using Veldrid;

namespace RedHerring.Studio;

[Importer(ProjectNodeType.AssetScene)]
public sealed class SceneImporter : Importer<Assimp.Scene>
{
	public override string ReferenceType => nameof(SceneReference);
	
	public SceneImporter(ProjectNode owner) : base(owner)
	{
	}

	public override void UpdateCache()
	{
	}

	public override void ClearCache()
	{
	}
	
	public override Assimp.Scene? Load()
	{
		throw new NotImplementedException();
		
		// SceneImporterSettings? settings = Owner.Meta?.ImporterSettings as SceneImporterSettings;
		// if (settings == null)
		// {
		// 	return null;
		// }
		//
		// AssimpContext context = new AssimpContext();
		//
		// Scene? assimpScene = context.ImportFile(
		// 	Owner.AbsolutePath,
		// 	PostProcessSteps.Triangulate
		// );
		//
		// if(!assimpScene.HasMeshes)
		// {
		// 	settings.Meshes.Clear();
		// 	return assimpScene;
		// }
		//
		// bool settingsChanged = false;
		// for (int i = 0; i < assimpScene.Meshes.Count; ++i)
		// {
		// 	// update settings
		// 	Mesh assimpMesh = assimpScene.Meshes[i];
		// 	if (i == settings.Meshes.Count)
		// 	{
		// 		settings.Meshes.Add(new SceneImporterMeshSettings(assimpMesh.Name));
		// 		settingsChanged = true;
		// 	}
		// 	else if(settings.Meshes[i].Name != assimpMesh.Name)
		// 	{
		// 		settings.Meshes[i] = new SceneImporterMeshSettings(assimpMesh.Name);
		// 		settingsChanged    = true;
		// 	}
		// }
		//
		// // cut the rest
		// if (settings.Meshes.Count > assimpScene.Meshes.Count)
		// {
		// 	settings.Meshes.RemoveRange(assimpScene.Meshes.Count, settings.Meshes.Count - assimpScene.Meshes.Count);
		// 	settingsChanged = true;
		// }
		//
		// //return settingsChanged ? ImporterResult.FinishedSettingsChanged : ImporterResult.Finished;
		// return assimpScene;
	}

	public override void Save(Assimp.Scene data)
	{
		throw new InvalidOperationException();
	}

	public override void Import(string resourcesRootPath, out string? relativeResourcePath)
	{
		SceneImporterSettings? settings = Owner.Meta?.ImporterSettings as SceneImporterSettings;
		if (settings == null)
		{
			ConsoleViewModel.LogError($"Cannot import '{Owner.RelativePath}'. Settings are missing or invalid!");
			relativeResourcePath = null;
			return;
		}

		AssimpContext context = new AssimpContext();
		context.SetConfig(new NormalSmoothingAngleConfig(settings.NormalSmoothingAngle));

		Assimp.Scene? assimpScene = context.ImportFile(
			Owner.AbsolutePath,
			PostProcessSteps.Triangulate // always - only triangles are supported
		);

		if (assimpScene == null)
		{
			ConsoleViewModel.LogError($"Cannot import '{Owner.RelativePath}'. Mesh was not loaded!");
			relativeResourcePath = null;
			return;
		}

		RedHerring.Render.Models.Scene scene = new();
		
		// meshes ---------------------------------------
		if (assimpScene.HasMeshes)
		{
			scene.Meshes = new List<SceneMesh>();
			for (int i = 0; i < assimpScene.Meshes.Count; ++i)
			{
				Mesh assimpMesh = assimpScene.Meshes[i];

				// sanity checks
				if (!assimpMesh.HasVertices)
				{
					continue; // vertices are mandatory
				}

				if (assimpMesh.PrimitiveType != PrimitiveType.Triangle)
				{
					continue; // ignore this mesh
				}
				
				SceneMesh mesh = new();

				// at least positions are there, so it should be safe to add the mesh
				scene.Meshes.Add(mesh);

				mesh.Name        = assimpMesh.Name;
				mesh.BoundingBox = new RedHerring.Numbers.BoundingBox(
					new Vector3(assimpMesh.BoundingBox.Min.X, assimpMesh.BoundingBox.Min.Y, assimpMesh.BoundingBox.Min.Z),
					new Vector3(assimpMesh.BoundingBox.Max.X, assimpMesh.BoundingBox.Max.Y, assimpMesh.BoundingBox.Max.Z)
				);

				// only the triangle list is supported now
				mesh.Topology = PrimitiveTopology.TriangleList;
				
				// positions
				mesh.Positions = assimpMesh.Vertices.Select(v => new Vector3(v.X, v.Y, v.Z)).ToList();

				// normals
				if (assimpMesh.HasNormals)
				{
					mesh.Normals = assimpMesh.Normals.Select(n => new Vector3(n.X, n.Y, n.Z)).ToList();
				}

				// tangents and bitangents
				if (assimpMesh.HasTangentBasis)
				{
					mesh.Tangents = assimpMesh.Tangents.Select(t => new Vector3(t.X, t.Y, t.Z)).ToList();
					mesh.BiTangents = assimpMesh.BiTangents.Select(b => new Vector3(b.X, b.Y, b.Z)).ToList();
				}

				// UV
				if (assimpMesh.TextureCoordinateChannelCount > 0)
				{
					mesh.TextureCoordinateChannels = new List<SceneMeshTextureCoordinateChannel>();
					for (int channelIndex = 0; channelIndex < assimpMesh.TextureCoordinateChannelCount; ++channelIndex)
					{
						SceneMeshTextureCoordinateChannel channel = new ();
						
						if (assimpMesh.UVComponentCount[channelIndex] == 2)
						{
							channel.UV = assimpMesh.TextureCoordinateChannels[channelIndex].Select(uv => new Vector2(uv.X, uv.Y)).ToList();
						}
						else if (assimpMesh.UVComponentCount[channelIndex] == 3)
						{
							channel.UVW = assimpMesh.TextureCoordinateChannels[channelIndex].Select(uv => new Vector3(uv.X, uv.Y, uv.Z)).ToList();
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
					mesh.VertexColorChannels = new List<SceneMeshVertexColorChannel>();
					for (int channelIndex = 0; channelIndex < assimpMesh.VertexColorChannelCount; ++channelIndex)
					{
						SceneMeshVertexColorChannel channel = new();
						mesh.VertexColorChannels.Add(channel);
						channel.Colors = assimpMesh.VertexColorChannels[channelIndex].Select(color => new RedHerring.Numbers.Color4(color.R, color.G, color.B, color.A)).ToList();
					}
				}
				
				// TODO - rest: bones, animation, morphing

				// faces
				if (assimpMesh.HasFaces)
				{
					if (mesh.Positions is not null && mesh.Positions.Count <= 0xffff)
					{
						mesh.UShortIndices = assimpMesh.GetUnsignedIndices().Select(idx => (ushort)idx).ToArray();
					}
					else
					{
						mesh.UIntIndices = assimpMesh.GetUnsignedIndices().ToArray();
					}
				}
			}
		}
		
		// hierarchy ----------------------------------------
		if (assimpScene.RootNode != null)
		{
			scene.Root      = new SceneNode();
			ImportNode(scene.Root, assimpScene.RootNode, settings);
			ImportChildNodesRecursive(scene.Root, assimpScene.RootNode, settings);
		}

		// import
		byte[] json         = SerializationUtility.SerializeValue(scene, DataFormat.Binary);
		relativeResourcePath = $"{Owner.RelativePath}.scene";
		string absolutePath = Path.Join(resourcesRootPath, relativeResourcePath);
		File.WriteAllBytes(absolutePath, json);
	}

	private void ImportChildNodesRecursive(SceneNode targetNode, Node source, SceneImporterSettings settings)
	{
		if (source.Children == null)
		{
			return;
		}
		
		foreach (Node sourceChild in source.Children)
		{
			targetNode.Children ??= new List<SceneNode>();

			SceneNode child = new();
			ImportNode(child, sourceChild, settings);
			targetNode.Children.Add(child);

			ImportChildNodesRecursive(child, sourceChild, settings);
		}
	}

	private void ImportNode(SceneNode targetNode, Node source, SceneImporterSettings settings)
	{
		targetNode.Name = source.Name;

		source.Transform.Decompose(out Assimp.Vector3D scale, out Assimp.Quaternion rotation, out Assimp.Vector3D translation);
		targetNode.Translation = new Vector3(translation.X, translation.Y, translation.Z);
		targetNode.Rotation    = new System.Numerics.Quaternion(rotation.X, rotation.Y, rotation.Z, rotation.W);

		if (settings.CompensateFBXScale && source.Parent is not null && source.Parent.Parent is null)
		{
			targetNode.Scale = new Vector3(scale.X * 0.01f, scale.Y * 0.01f, scale.Z * 0.01f);	// convert cm -> m
		}
		else
		{
			targetNode.Scale = new Vector3(scale.X, scale.Y, scale.Z);
		}

		if (source.HasMeshes)
		{
			targetNode.MeshIndices = new List<int>();
			for (int i = 0; i < source.MeshIndices.Count; ++i)
			{
				targetNode.MeshIndices.Add(source.MeshIndices[i]);
			}
		}
	}

	public override ImporterSettings CreateImportSettings()
	{
		SceneImporterSettings settings = new SceneImporterSettings();
		
		if (Owner.Extension == ".fbx")
		{
			settings.CompensateFBXScale = true;
		}

		UpdateImportSettings(settings);
		return settings;
	}

	public override bool UpdateImportSettings(ImporterSettings settings)
	{
		SceneImporterSettings? sceneSettings = settings as SceneImporterSettings;
		if (sceneSettings == null)
		{
			return false;
		}

		AssimpContext context = new AssimpContext();

		Assimp.Scene? scene = context.ImportFile(
			Owner.AbsolutePath,
			PostProcessSteps.None
		);
		
		// if(!_scene.HasMeshes) // should be covered by the loop
		// {
		// 	settings.Meshes.Clear();
		// 	return;
		// }

		bool settingsChanged = false;

		//----- meshes
		settingsChanged |= ResizeAndUpdateList(
			ref sceneSettings.Meshes,
			scene.Meshes.Count,
			(settingsMesh, index) => settingsMesh.Name != scene.Meshes[index].Name || settingsMesh.MaterialIndex != scene.Meshes[index].MaterialIndex,
			index => sceneSettings.Meshes[index] = new SceneImporterMeshSettings(scene.Meshes[index].Name, scene.Meshes[index].MaterialIndex)
		);

		//----- materials
		settingsChanged |= ResizeAndUpdateList(
			ref sceneSettings.Materials,
			scene.MaterialCount,
			(settingsMaterial, index) => settingsMaterial.Name != scene.Materials[index].Name,
			index => sceneSettings.Materials[index] = new SceneImporterMaterialSettings(scene.Materials[index].Name)
		);
		
		//----- hierarchy
		Node root = scene.RootNode ?? new Node();
		sceneSettings.Root ??= new SceneImporterHierarchyNodeSettings(root.Name);
		settingsChanged    |=  UpdateImportSettingsHierarchyNode(root, sceneSettings.Root);

		return settingsChanged;
	}

	private bool UpdateImportSettingsHierarchyNode(Node node, SceneImporterHierarchyNodeSettings sceneImporterHierarchyNodeSettings)
	{
		bool settingsChanged = false;

		// children
		settingsChanged |= ResizeAndUpdateList(
			ref sceneImporterHierarchyNodeSettings.Children,
			node.ChildCount,
			(child, index) => child.Name != node.Children[index].Name,
			index => sceneImporterHierarchyNodeSettings.Children![index] = new SceneImporterHierarchyNodeSettings(node.Children[index].Name)
		);

		for (int i = 0; i < node.ChildCount; ++i)
		{
			settingsChanged |= UpdateImportSettingsHierarchyNode(node.Children[i], sceneImporterHierarchyNodeSettings.Children![i]);
		}
		
		// meshes
		settingsChanged |= ResizeAndUpdateList(
			ref sceneImporterHierarchyNodeSettings.Meshes,
			node.HasMeshes ? node.MeshCount : 0,
			(meshIndex, index) => meshIndex != node.MeshIndices[index],
			index => sceneImporterHierarchyNodeSettings.Meshes[index] = node.MeshIndices[index]
		);

		return settingsChanged;
	}

	private static bool ResizeAndUpdateList<T>(ref List<T>? list, int desiredCount, Func<T, int, bool> needsUpdate, Action<int> update)
	{
		bool changed = false;
		
		list ??= new List<T>();
		for (int i = 0; i < desiredCount; ++i)
		{
			if (i == list.Count)
			{
				list.Add(default);
				update(i);
				changed = true;
			}
			else if (needsUpdate(list[i], i))
			{
				update(i);
				changed = true;
			}
		}

		if (list.Count > desiredCount)
		{
			list.RemoveRange(desiredCount, list.Count - desiredCount);
			changed = true;
		}

		return changed;
	}
}