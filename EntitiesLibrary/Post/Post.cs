using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EntitiesLibrary.FileSystem;

namespace EntitiesLibrary.Post;
public class Post
{    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string? Description { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime RegistrationDateTime { get; set; }

    public required EntitiesLibrary.User.User User { get; set; }
   
    public EntitiesLibrary.FileSystem.PublishedFile? File { get; set; }
    //public required PostStatus PostStatus { get; set; }
    public required EntitiesLibrary.Common.EntityStatus EntityStatus { get; set; }
}