using System.Numerics;
using OdinSerializer;
using RedHerring.Numbers;
using RedHerring.Render;
using RedHerring.Render.Models;
using RedHerring.Studio.Models.Project.FileSystem;
using RedHerring.Studio.Models.ViewModels.Console;
using Silk.NET.Assimp;
using Silk.NET.Maths;
using Veldrid;
using File = System.IO.File;
using AssimpScene = Silk.NET.Assimp.Scene;
using AssimpMesh = Silk.NET.Assimp.Mesh;
using AssimpMaterial = Silk.NET.Assimp.Material;
using Scene = RedHerring.Render.Models.Scene;

namespace RedHerring.Studio.Import;

[Importer(ProjectNodeKind.AssetScene)]
public class AssimpSceneImporter : Importer<AssimpScene>
{
	private readonly Silk.NET.Assimp.Assimp _assimp = Silk.NET.Assimp.Assimp.GetApi();
	private readonly AssimpContext _context = new();

	public AssimpSceneImporter(ProjectNode owner) : base(owner)
	{
	}

	public override string ReferenceType => nameof(SceneReference);

	public override void UpdateCache()
	{
	}

	public override void ClearCache()
	{
	}

	public override unsafe void Import(string resourcesRootPath, out string? relativeResourcePath)
	{
		AssimpSceneImporterSettings? settings = Owner.Meta?.ImporterSettings as AssimpSceneImporterSettings;
		if (settings == null)
		{
			ConsoleViewModel.LogError($"Cannot import '{Owner.RelativePath}'. Settings are missing or invalid!");
			relativeResourcePath = null;
			return;
		}

		relativeResourcePath = null;

		uint importFlags = 0;
		uint postProcessFlags = 0;

		var scene = FreshScene(Owner.AbsolutePath, importFlags, postProcessFlags);
		var renderScene = ConvertScene(scene);

		// import
		byte[] json = SerializationUtility.SerializeValue(renderScene, DataFormat.Binary);
		relativeResourcePath = $"{Owner.RelativePath}.scene";
		string absolutePath = Path.Join(resourcesRootPath, relativeResourcePath);
		Directory.CreateDirectory(Path.GetDirectoryName(absolutePath)!);
		File.WriteAllBytes(absolutePath, json);
	}

	#region Private

	private unsafe Scene ConvertScene(AssimpScene* scene)
	{
		Scene result = new();

		if (scene->MNumMeshes > 0)
		{
			result.Meshes = new List<SceneMesh>();
			for (int i = 0; i < scene->MNumMeshes; i++)
			{
				var mesh = ProcessMesh(scene, scene->MMeshes[i]);
				if (mesh is null)
				{
					continue;
				}

				result.Meshes.Add(mesh);
			}
		}

		if (scene->MRootNode is not null)
		{
			result.Root = new SceneNode();
			// TODO(Mirzi): import node
			// TODO(Mirzi): import child nodes recursively
		}

		return result;
	}

	private unsafe SceneMesh? ProcessMesh(AssimpScene* scene, AssimpMesh* mesh)
	{
		if (mesh->MNumVertices == 0)
		{
			return null; // vertices are mandatory
		}

		if (mesh->MPrimitiveTypes != (uint)PrimitiveType.Triangle)
		{
			return null; // ignore this mesh
		}

		var result = new SceneMesh();
		result.Name = mesh->MName.ToString();
		result.MaterialIndex = (int)mesh->MMaterialIndex;

		// only the triangle list is supported now
		result.Topology = PrimitiveTopology.TriangleList;

		var bb = mesh->MAABB;
		result.BoundingBox = new BoundingBox(bb.Min.ToSystem(), bb.Max.ToSystem());

		// positions
		int vertexCount = (int)mesh->MNumVertices;
		result.Positions = new List<Vector3>(vertexCount);

		// normals
		bool hasNormals = mesh->MNormals is not null;
		if (hasNormals)
		{
			result.Normals = new List<Vector3>(vertexCount);
		}

		// tangents and bitangents
		bool hasTangents = mesh->MTangents is not null;
		if (hasTangents)
		{
			result.Tangents = new List<Vector3>(vertexCount);
		}

		bool hasBiTangents = mesh->MTangents is not null;
		if (hasBiTangents)
		{
			result.BiTangents = new List<Vector3>(vertexCount);
		}

		// UV
		int uvChannelCount = AssimpUtils.UVChannelCount(mesh);
		if (uvChannelCount > 0)
		{
			result.TextureCoordinateChannels = new List<SceneMeshTextureCoordinateChannel>(uvChannelCount);
			for (int channelIndex = 0; channelIndex < uvChannelCount; channelIndex++)
			{
				uint size = mesh->MNumUVComponents[channelIndex];
				var channel = new SceneMeshTextureCoordinateChannel();
				result.TextureCoordinateChannels.Add(channel);
				switch (size)
				{
					case 2:
						channel.UV = new List<Vector2>(vertexCount);
						break;
					case 3:
						channel.UVW = new List<Vector3>(vertexCount);
						break;
				}
			}
		}

		// colors
		int colorChannelCount = AssimpUtils.ColorChannelCount(mesh);
		if (colorChannelCount > 0)
		{
			result.VertexColorChannels = new List<SceneMeshVertexColorChannel>(colorChannelCount);
			for (int channelIndex = 0; channelIndex < colorChannelCount; channelIndex++)
			{
				var channel = new SceneMeshVertexColorChannel();
				result.VertexColorChannels.Add(channel);
				channel.Colors = new List<Color4>();
			}
		}

		for (int i = 0; i < vertexCount; i++)
		{
			result.Positions.Add(mesh->MVertices[i]);

			if (hasNormals)
			{
				result.Normals!.Add(mesh->MNormals[i]);
			}

			if (hasTangents)
			{
				result.Tangents!.Add(mesh->MTangents[i]);
			}

			if (hasBiTangents)
			{
				result.BiTangents!.Add(mesh->MBitangents[i]);
			}

			for (int channelIndex = 0; channelIndex < uvChannelCount; channelIndex++)
			{
				var uv = mesh->MTextureCoords[channelIndex][i];
				uint size = mesh->MNumUVComponents[channelIndex];
				var channel = result.TextureCoordinateChannels![channelIndex];
				switch (size)
				{
					case 2:
						channel.UV!.Add(uv.XY());
						break;
					case 3:
						channel.UVW!.Add(uv);
						break;
				}
			}

			for (int channelIndex = 0; channelIndex < colorChannelCount; channelIndex++)
			{
				var color = mesh->MColors[channelIndex][i];
				var channel = result.VertexColorChannels![channelIndex];
				channel.Colors.Add(new Color4(color));
			}
		}

		// faces
		bool hasFaces = mesh->MNumFaces > 0;
		if (hasFaces)
		{
			uint[]? unsignedIndices = AssimpUtils.UnsignedIndices(mesh);
			if (unsignedIndices is not null)
			{
				if (result.Positions.Count <= 0xffff)
				{
					result.UShortIndices = unsignedIndices.Select(idx => (ushort)idx).ToArray();
				}
				else
				{
					result.UIntIndices = unsignedIndices;
				}
			}
		}

		return result;
	}

	private unsafe AssimpScene* FreshScene(string filePath, uint importFlags, uint postProcessFlags)
	{
		_context.Clear();

		var scene = _assimp.ImportFile(filePath, importFlags);
		scene = _assimp.ApplyPostProcessing(scene, postProcessFlags);

		return scene;
	}

	#endregion Private

	#region Settings

	public override ImporterSettings CreateImportSettings()
	{
		AssimpSceneImporterSettings settings = new AssimpSceneImporterSettings();

		if (Owner.Extension == ".fbx")
		{
			settings.CompensateFBXScale = true;
		}

		UpdateImportSettings(settings);
		return settings;
	}

	public override unsafe bool UpdateImportSettings(ImporterSettings settings)
	{
		AssimpSceneImporterSettings? sceneSettings = settings as AssimpSceneImporterSettings;
		if (sceneSettings == null)
		{
			return false;
		}

		uint importFlags = 0;
		uint postProcessFlags = 0;

		var freshScene = FreshScene(Owner.AbsolutePath, importFlags, postProcessFlags);

		bool settingsChanged = false;

		//----- meshes
		settingsChanged |= ResizeAndUpdateList(
			ref sceneSettings.Meshes!,
			(int)freshScene->MNumMeshes,
			(settingsMesh, index) => settingsMesh.Name != freshScene->MMeshes[index]->MName ||
			                         settingsMesh.MaterialIndex != freshScene->MMeshes[index]->MMaterialIndex,
			index => sceneSettings.Meshes[index] = new AssimpSceneImporterMeshSettings(freshScene->MMeshes[index]->MName,
				(int)freshScene->MMeshes[index]->MMaterialIndex)
		);

		//----- materials
		settingsChanged |= ResizeAndUpdateList(
			ref sceneSettings.Materials!,
			(int)freshScene->MNumMaterials,
			(settingsMaterial, index) =>
				settingsMaterial.Name != AssimpUtils.MaterialName(freshScene->MMaterials[index]),
			index => sceneSettings.Materials[index] =
				new AssimpSceneImporterMaterialSettings(AssimpUtils.MaterialName(freshScene->MMaterials[index]))
		);

		// animations
		settingsChanged |= ResizeAndUpdateList(
			ref sceneSettings.Animations!,
			(int)freshScene->MNumAnimations,
			(settingsAnimation, index) =>
				string.CompareOrdinal(settingsAnimation.Name, freshScene->MAnimations[index]->MName) != 0,
			index => sceneSettings.Animations[index] =
				new AssimpSceneImporterAnimationSettings(freshScene->MAnimations[index]->MName)
		);


		//----- hierarchy
		Node* root = freshScene->MRootNode;
		sceneSettings.Root ??= new AssimpSceneImporterHierarchyNodeSettings(root is not null ? root->MName : string.Empty);
		settingsChanged |= UpdateImportSettingsHierarchyNode(root, sceneSettings.Root);

		return settingsChanged;
	}

	private unsafe bool UpdateImportSettingsHierarchyNode(
		Node* node,
		AssimpSceneImporterHierarchyNodeSettings hierarchyNodeSettings)
	{
		bool settingsChanged = false;
		if (node is null)
		{
			return settingsChanged;
		}

		// children
		settingsChanged |= ResizeAndUpdateList(
			ref hierarchyNodeSettings.Children,
			(int)node->MNumChildren,
			(child, index) => child.Name != node->MChildren[index]->MName,
			index => hierarchyNodeSettings.Children![index] =
				new AssimpSceneImporterHierarchyNodeSettings(node->MChildren[index]->MName)
		);

		for (uint i = 0; i < node->MNumChildren; ++i)
		{
			settingsChanged |= UpdateImportSettingsHierarchyNode(node->MChildren[i],
				hierarchyNodeSettings.Children![(int)i]);
		}

		// meshes
		settingsChanged |= ResizeAndUpdateList(
			ref hierarchyNodeSettings.Meshes,
			(int)node->MNumMeshes,
			(meshIndex, index) => meshIndex != node->MMeshes[index],
			index => hierarchyNodeSettings.Meshes[index] = (int)node->MMeshes[index]
		);

		return settingsChanged;
	}

	private static bool ResizeAndUpdateList<T>(
		ref List<T>? list,
		int desiredCount,
		Func<T, int, bool> needsUpdate,
		Action<int> update)
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

	#endregion Settings

	#region Unused

	public override AssimpScene Load()
	{
		throw new NotImplementedException();
	}

	public override void Save(AssimpScene data)
	{
		throw new InvalidOperationException();
	}

	#endregion Unused
}