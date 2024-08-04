using EntitiesLibrary.Entities.Enum;

namespace EntitiesLibrary.Entities;

public class Reaction
{
    public required int Id { get; set; }
    public required int IdUser { get; set; }
    public required int IdPost { get; set; }
    public required ReactionStatus ReactionType { get; set; }
}