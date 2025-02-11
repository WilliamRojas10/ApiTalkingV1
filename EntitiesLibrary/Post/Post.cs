using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntitiesLibrary.Post;
[Table("post")]
public class Post
{    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string? Description { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime RegistrationDateTime { get; set; }

    public required EntitiesLibrary.User.User User { get; set; }
   
    public EntitiesLibrary.File.File? File { get; set; }
    //public required PostStatus PostStatus { get; set; }
    public required EntitiesLibrary.Common.EntityStatus EntityStatus { get; set; }
}