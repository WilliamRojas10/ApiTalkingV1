using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EntitiesLibrary.Comment;

namespace EntitiesLibrary.Comment;

public class Comment
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public required int Id { get; set; }
    public string? Text { get; set; }
    public  DateTime RegistrationDate { get; set; }
    public  CommentStatus CommentStatus { get; set; }
    public required int IdUser { get; set; }
    public required int IdPost { get; set; }

}