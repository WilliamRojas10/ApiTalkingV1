

namespace ApiTalking.DTO.Post;
public class RequestPostDTO
{    
 
    public required int idPost { get; set; }
    public string? description { get; set; }
    public int postStatus { get; set; }


  //  public DateTime RegistrationDateTime { get; set; }
    public required int idUser { get; set; }
    public int? idFile { get; set; }
   
}