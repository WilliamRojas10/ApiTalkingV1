
namespace ApiTalking.DTO.CommentDTO;

public class ResponseCommentDTO
{
    public required int id { get; set; }
    public required string text { get; set; }
     public required DateTime registrationDate { get; set; }
    public  string commentStatus { get; set; } = "";
}