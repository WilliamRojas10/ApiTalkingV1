using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntitiesLibrary.Post;
public class Post
{    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public required int Id { get; set; }
    public string? Description { get; set; }
    public required PostStatus PostStatus { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime RegistrationDateTime { get; set; }
    public required int IdUser { get; set; }
    public int? IdFile { get; set; }
   
}