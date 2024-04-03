using Silk.NET.Assimp;
using Api = Silk.NET.Assimp.Assimp;
using AssimpMesh = Silk.NET.Assimp.Mesh;
using AssimpMaterial = Silk.NET.Assimp.Material;

namespace RedHerring.Studio.Import;

internal static class AssimpUtils
{
    public const int MaxNumberOfTextureCoords = 8;
    public const int MaxNumberOfColorSets = 8;
    
    // ReSharper disable once InconsistentNaming
    public static unsafe int UVChannelCount(AssimpMesh* mesh)
    {
        int result = 0;
        while (result < MaxNumberOfTextureCoords && mesh->MTextureCoords[result] != null)
        {
            ++result;
        }

        return result;
    }

    public static unsafe int ColorChannelCount(AssimpMesh* mesh)
    {
        int result = 0;
        while (result < MaxNumberOfColorSets && mesh->MColors[result] != null)
        {
            ++result;
        }

        return result;
    }
    
    public static unsafe uint[]? UnsignedIndices(AssimpMesh* mesh)
    {
        if (mesh->MNumFaces == 0)
        {
            return null;
        }
        
        List<uint> result = new List<uint>();
        for (int i = 0; i < mesh->MNumFaces; i++)
        {
            var face = mesh->MFaces[i];
            for (int index = 0; index < face.MNumIndices; index++)
            {
                result.Add(face.MIndices[i]);
            }
        }
        
        return result.ToArray();
    }

    public static unsafe string MaterialName(Api api, AssimpMaterial* material)
    {
        AssimpString aiMaterial = new AssimpString();
        var result = api.GetMaterialString(material, Api.MaterialNameBase, 0, 0, ref aiMaterial);
        return result == Return.Success ? aiMaterial.AsString : "Material";
    }

    public static TimeSpan TimeToTimeSpan(double time, double ticksPerSecond)
    {
        double adjustedTime = TimeSpan.TicksPerSecond / ticksPerSecond * time;
        return TimeSpan.FromTicks((long)adjustedTime);
    }
}