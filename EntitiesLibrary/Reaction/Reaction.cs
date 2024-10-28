using EntitiesLibrary.Reaction;

namespace EntitiesLibrary.Reaction;
public class Reaction
{
    public required int Id { get; set; }
    public required int IdUser { get; set; }
    public required int IdPost { get; set; }
    public ReactionStatus ReactionStatus { get; set; }
}