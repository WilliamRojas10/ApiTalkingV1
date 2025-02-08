namespace ApiTalking.DTOs.Comment;

public class ResponseCommentDTO
{
    public required int idComment { get; set; }
    public required string text { get; set; }
    public required string registrationDate { get; set; }
   // public  string commentStatus { get; set; }
}