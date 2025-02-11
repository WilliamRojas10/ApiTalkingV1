
namespace DaoLibrary.Interfaces.File;
public interface IDAOFile
{
    Task<(List<EntitiesLibrary.File.File> files, int TotalCount)> GetFilesPaged
            (int pageNumber, int pageSize, EntitiesLibrary.Common.EntityStatus? entityStatus);

    Task<List<EntitiesLibrary.File.File>> GetAllFiles();

    Task<EntitiesLibrary.File.File?> GetFileById(int id);
    Task<EntitiesLibrary.File.File> AddFile(EntitiesLibrary.File.File file);

    Task<EntitiesLibrary.File.File?> GetFileById
    (int id, EntitiesLibrary.Common.EntityStatus? entityStatus);


    Task UpdateFile(EntitiesLibrary.File.File file);

    Task DeleteFile(int id);
}
