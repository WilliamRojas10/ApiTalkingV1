
namespace ApiTalking.DTO.User;

public class ResponsePostDTO
{
    public string? Description { get; set; } = "";
    public required string PostStatus { get; set; } = "Activo";
    public required DateTime RegistrationDate { get; set; } 
    public required int IdUser { get; set; } = 0;
    //public int IdFile { get; set; }
   
}