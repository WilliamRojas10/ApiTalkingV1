namespace ApiTalking.DTOs.Login;

public class ResponseLoginDTO
{
    public required string email { get; set; }
    //public required string password { get; set; } 
    public required string token {get; set; }
}