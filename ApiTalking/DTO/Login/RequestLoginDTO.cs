namespace ApiTalking.DTO.Login;

public class RequestLoginDTO
{
    public required string email { get; set; }
    public required string password { get; set; } //TODO : Encriptar
}