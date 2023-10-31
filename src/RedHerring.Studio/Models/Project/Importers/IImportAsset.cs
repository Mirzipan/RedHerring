using RedHerring.Studio.Models.Project.FileSystem;

namespace RedHerring.Studio.Models.Project.Importers;

public interface IImportAsset
{
    object? Import(Stream stream);
}