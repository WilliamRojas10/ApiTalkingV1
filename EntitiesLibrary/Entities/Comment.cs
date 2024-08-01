

namespace EntitiesLibrary.Entities;

public class Comment
{
    public required int Id { get; set; }
    public string? Text { get; set; }
    public required User User { get; set; }
}