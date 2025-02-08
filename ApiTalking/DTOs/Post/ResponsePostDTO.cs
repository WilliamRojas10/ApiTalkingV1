namespace ApiTalking.DTOs.Post;
public class ResponsePostDTO
{    
 
    public required int idPost { get; set; }
    public string? description { get; set; }
  //  public required PostStatus PostStatus { get; set; }

    public required string registrationDateTime { get; set; }
    public required int idUser { get; set; }

    public string nameUser {get; set;}
    public string lastNameUser {get; set;}
    
    public int? idFile { get; set; }
   
}