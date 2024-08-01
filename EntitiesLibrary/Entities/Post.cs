

namespace EntitiesLibrary.Entities;


public class Post{
    public required int Id { get; set; }
    public string? Description { get; set; }
    public required User User { get; set; }
    public File? File { get; set; }//TODO COMO SE LISTAS TODOS LOS COMENTARIOS
    public Comment? Comment { get; set; }//TODO COMO SE LISTAS TODOS LOS COMENTARIOS
}