using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntitiesLibrary.Reaction;
public class Reaction
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public required EntitiesLibrary.User.User User { get; set; }
    public required EntitiesLibrary.Post.PostStatus PostStatus { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime RegistrationDateTime { get; set; }
    public required ReactionStatus ReactionStatus { get; set; }
}