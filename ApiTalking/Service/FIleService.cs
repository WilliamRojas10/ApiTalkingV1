using DaoLibrary.Interfaces.File;
namespace ApiTalking.Service;
public class FileService 
{
    private readonly IDAOFile _daoFile;
    private readonly string _basePath = Path.Combine(Directory.GetCurrentDirectory(), "FilesSystem", "Images");
    public FileService(IDAOFile daoFile)
    {
        _daoFile = daoFile;
    }
    //private readonly IDaoPublishedFile _daoPublishedFile

    public async Task<EntitiesLibrary.File.File> SaveImage(IFormFile image, int userId, string folder)
    {
        if (image == null || image.Length == 0)
        {
            throw new ArgumentException("No se proporcionó ninguna imagen o el archivo está vacío.");
        }

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        var extension = Path.GetExtension(image.FileName).ToLower();
        if (!allowedExtensions.Contains(extension))
        {
            throw new ArgumentException("El formato del archivo no es válido. Solo se permiten .jpg, .jpeg, .png, .gif.");
        }

        var uploadPath = Path.Combine(_basePath, folder);
        if (!Directory.Exists(uploadPath))
        {
            Directory.CreateDirectory(uploadPath);
        }

        var imageFileName = $"{userId}_{Guid.NewGuid()}{extension}";
        var imagePath = Path.Combine(uploadPath, imageFileName);

        using (var fileStream = new FileStream(imagePath, FileMode.Create))
        {
            await image.CopyToAsync(fileStream);
        }

        EntitiesLibrary.File.File fileEntity = new EntitiesLibrary.File.File
        {
            Name = imageFileName,
            Path = $"/FilesSystem/Images/{folder}/{imageFileName}",
            EntityStatus = EntitiesLibrary.Common.EntityStatus.Active,
            Type = new EntitiesLibrary.File.FileType { TypeFile = "image" }
        };

        return await _daoFile.AddFile(fileEntity);
    }
}
