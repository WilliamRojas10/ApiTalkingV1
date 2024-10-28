using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EntitiesLibrary.Entities.Enum;

namespace EntitiesLibrary.Post;
public class Post
{    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public required int Id { get; set; }
    public string? Description { get; set; }
    public required PostStatus PostStatus { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime RegistrationDate { get; set; }
    public required int IdUser { get; set; }
    public int? IdFile { get; set; }
   
}