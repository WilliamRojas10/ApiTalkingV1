namespace ApiTalking.DTO.Reaction;
public class ResponseReactionDTO
{
    public required int idReaction{ get; set; }
    public required int idUser { get; set; }
    public required int idPost { get; set; }
    public string registrationDateTime { get; set; }
    //public required int ReactionStatus { get; set; }
}