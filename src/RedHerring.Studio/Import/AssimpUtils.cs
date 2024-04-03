using System.Text;
using Silk.NET.Assimp;

namespace RedHerring.Studio.Import;

internal static class AssimpUtils
{
    public const int MaxNumberOfTextureCoords = 8;
    public const int MaxNumberOfColorSets = 8;
    
    // ReSharper disable once InconsistentNaming
    public static unsafe int UVChannelCount(Mesh* mesh)
    {
        int result = 0;
        while (result < MaxNumberOfTextureCoords && mesh->MTextureCoords[result] != null)
        {
            ++result;
        }

        return result;
    }

    public static unsafe int ColorChannelCount(Mesh* mesh)
    {
        int result = 0;
        while (result < MaxNumberOfColorSets && mesh->MColors[result] != null)
        {
            ++result;
        }

        return result;
    }
    
    public static unsafe uint[]? UnsignedIndices(Mesh* mesh)
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

    public static unsafe string MaterialName(Material* material)
    {
        if (material->MNumProperties == 0)
        {
            return string.Empty;
        }

        for (int i = 0; i < material->MNumProperties; i++)
        {
            var property = material->MProperties[i];
            if (property->MKey == Silk.NET.Assimp.Assimp.MatkeyName)
            {
                string result = Encoding.UTF8.GetString(property->MData, (int)property->MDataLength);
                return result;
            }
        }
        
        return string.Empty;
    }

    public static TimeSpan TimeToTimeSpan(double time, double ticksPerSecond)
    {
        double adjustedTime = TimeSpan.TicksPerSecond / ticksPerSecond * time;
        return TimeSpan.FromTicks((long)adjustedTime);
    }
}