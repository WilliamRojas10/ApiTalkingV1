

namespace ApiTalking.DTO.Reaction;

public class ResponseReactionDTO
{
     //public required int Id { get; set; }
    public required int idUser { get; set; }
    public required int idPost { get; set; }
    public required string reactionStatus { get; set; }
}