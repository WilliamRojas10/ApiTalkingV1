
namespace ApiTalking.DTO.User;

public class RequestPostDTO
{
    public string? Description { get; set; } 
    public required string PostStatus { get; set; }
    public required DateTime RegistrationDate { get; set; } 
    public required int IdUser { get; set; } 
    public int IdFile { get; set; }
   
}