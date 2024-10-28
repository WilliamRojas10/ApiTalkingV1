namespace ApiTalking.DTO.Comment;

public class ResponseCommentDTO
{
    public required int id { get; set; }
    public required string text { get; set; }
    public required string registrationDate { get; set; }
    public  string commentStatus { get; set; }
}