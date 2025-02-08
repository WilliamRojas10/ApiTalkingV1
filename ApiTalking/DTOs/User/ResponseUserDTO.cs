
namespace ApiTalking.DTOs.User;

public class ResponseUserDTO
{
    public required int idUser { get; set; }
    public required string name { get; set; }
    public required string lastName { get; set; }
    public required string email { get; set; }
    public required string birthDate { get; set; }
    public string registrationDateTime { get; set; }
    public string? nationality { get; set; } = "";
    public string? province { get; set; } = "";
    public string? ProfileImagePath { get; set; }
        
    
    // public string userStatus { get; set; } = "";

    
}
