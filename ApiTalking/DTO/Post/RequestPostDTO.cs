
using EntitiesLibrary.Entities.Enum;

namespace ApiTalking.DTO.Post;

public class RequestPostDTO
{
    public string? description { get; set; } 
    public required PostStatus postStatus { get; set; } = PostStatus.Active;
    public required DateTime registrationDate { get; set; } 
    public required int idUser { get; set; } 
    public int? idFile { get; set; }
   
}