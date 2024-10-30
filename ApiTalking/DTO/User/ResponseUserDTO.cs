
namespace ApiTalking.DTO.User;

public class ResponseUserDTO
{
  //  public required int Id { get; set; }
    public required string name { get; set; }
    public required string lastName { get; set; }
    public required string email { get; set; }
    public required DateTime birthDate { get; set; }
    public string? nationality { get; set; }= "";
     public string? province { get; set; } = "";
    // public string userStatus { get; set; } = "";
    // public int age { get; set; }
}
