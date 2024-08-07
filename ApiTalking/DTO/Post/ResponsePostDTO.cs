
namespace ApiTalking.DTO.Post;

public class ResponsePostDTO
{
    public int id { get; set; }
    public string? description { get; set; }
    public string postStatus { get; set; }
    public DateTime registrationDate { get; set; }
    public int idUser { get; set; }
    public int? idFile { get; set; }
}