using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntitiesLibrary.Comment;
[Table("comment")]
public class Comment
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string? Text { get; set; }
    
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public  DateTime RegistrationDateTime { get; set; }

    public required EntitiesLibrary.User.User User { get; set; }

    public required EntitiesLibrary.Post.Post Post { get; set; }

    public required EntitiesLibrary.Common.EntityStatus EntityStatus { get; set; }
}