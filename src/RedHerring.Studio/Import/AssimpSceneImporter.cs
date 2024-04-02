using System.Numerics;
using RedHerring.Alexandria.Extensions;
using RedHerring.Numbers;
using RedHerring.Render;
using RedHerring.Render.Models;
using RedHerring.Studio.Models.Project.FileSystem;
using RedHerring.Studio.Models.ViewModels.Console;
using Silk.NET.Assimp;
using Silk.NET.Maths;
using Veldrid;
using Scene = Silk.NET.Assimp.Scene;
using RenderScene = RedHerring.Render.Models.Scene;

namespace RedHerring.Studio.Import;

[Importer(ProjectNodeKind.AssetScene)]
public class AssimpSceneImporter : Importer<Scene>
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

        // TODO(Mirzi): do more stuff?
    }

    #region Private

    private unsafe RenderScene ConvertScene(Scene* scene)
    {
        RenderScene result = new();

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

    private unsafe SceneMesh? ProcessMesh(Scene* scene, Mesh* mesh)
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
        for (int i = 0; i < vertexCount; i++)
        {
            result.Positions.Add(mesh->MVertices[i]);
        }
        
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
                var channel = result.TextureCoordinateChannels[channelIndex];
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
                var channel = result.VertexColorChannels[channelIndex];
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

    private unsafe Scene* FreshScene(string filePath, uint importFlags, uint postProcessFlags)
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

    public override bool UpdateImportSettings(ImporterSettings settings)
    {
        return false;
    }

    #endregion Settings

    #region Unused

    public override Scene Load()
    {
        throw new NotImplementedException();
    }

    public override void Save(Scene data)
    {
        throw new InvalidOperationException();
    }

    #endregion Unused
}