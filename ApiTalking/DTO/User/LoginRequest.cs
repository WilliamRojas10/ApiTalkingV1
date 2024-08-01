namespace ApiTalking.DTO.Login;

public class Login 
{
    public required string Email { get; set; }
    public required string Password { get; set; } //TODO : Encriptar
}