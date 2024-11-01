using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntitiesLibrary.Reaction;
public class Reaction
{
    public required int Id { get; set; }
    public required int IdUser { get; set; }
    public required int IdPost { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime RegistrationDateTime { get; set; }
    public required ReactionStatus ReactionStatus { get; set; }
}