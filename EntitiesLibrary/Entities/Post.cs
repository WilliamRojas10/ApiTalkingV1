using EntitiesLibrary.Entities.Enum;

namespace EntitiesLibrary.Entities;
public class Post{
    public required int Id { get; set; }
    public string? Description { get; set; }
    public required PostStatus PostStatus { get; set; }
    public required DateTime RegistrationDate { get; set; }
    public required int IdUser { get; set; }
    public int IdFile { get; set; }
   
}