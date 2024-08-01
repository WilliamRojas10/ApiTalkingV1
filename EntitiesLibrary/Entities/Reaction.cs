using EntitiesLibrary.Entities.Enum;

namespace EntitiesLibrary.Entities;


public class Reaction
{
    
    public required int Id { get; set; }

    //FK
    public required User User { get; set; }
    public required int UserId { get; set; }

    //FK
    public required Post Post { get; set; }
    public required int PostId { get; set; }
    
    public required ReactionType ReactionType { get; set; }
}