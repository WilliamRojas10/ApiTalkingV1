using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntitiesLibrary.FileSystem;

public class PublishedFile
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required FileType Type { get; set; }
    public required string Path { get; set; }
    public required EntitiesLibrary.Common.EntityStatus EntityStatus { get; set; }
}