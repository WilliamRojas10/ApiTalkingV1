
namespace ApiTalking.DTO.User;

public class ResponseUserDTO
{
  //  public required int Id { get; set; }
    public required string Name { get; set; }
    public required string LastName { get; set; }
   // public required string Email { get; set; }
    public required DateTime BirthDate { get; set; }
    public string? Nationality { get; set; }= "";
    public string? Province { get; set; } = "";
    public string UserStatus { get; set; } = "Activo";
}
