

namespace ApiTalking.DTOs.Post;
public class RequestPostDTO
{    
    public string? description { get; set; }
    public IFormFile? image { get; set; }
}


public class UploadFileDTO
{
    public IFormFile? image { get; set; } // Ahora puede ser nulo
}
