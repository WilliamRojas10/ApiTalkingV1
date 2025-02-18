namespace ApiTalking.DTOs.Reaction;
public class RequestReactionDTO
{
    public required int idReaction{ get; set; }
    //public required int idUser { get; set; }
    public required int idPost { get; set; }
    //public DateTime registrationDateTime { get; set; }
    //public required int ReactionStatus { get; set; }
}