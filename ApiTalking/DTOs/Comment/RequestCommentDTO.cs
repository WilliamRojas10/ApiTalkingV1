namespace ApiTalking.DTOs.Comment;

public class RequestCommentDTO
{
    // public required int id { get; set; }
    public string? text { get; set; }
    // public required DateTime registrationDate { get; set; }
   // public int commentStatus { get; set; }
    public required int idUser { get; set; }
    public required int idPost { get; set; }
}