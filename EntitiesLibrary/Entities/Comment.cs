
namespace EntitiesLibrary.Entities;

public class Comment
{
    public required int Id { get; set; }
    public string? Text { get; set; }
    public required DateTime RegistrationDate { get; set; }
    public required int IdUser { get; set; }
    public required int IdPost { get; set; }
}