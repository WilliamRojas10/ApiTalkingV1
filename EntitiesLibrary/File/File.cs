using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntitiesLibrary.File;
[Table("file")]

public class File
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public required string Name { get; set; }
    public required FileType Type { get; set; }
    public required string Path { get; set; }
    public required EntitiesLibrary.Common.EntityStatus EntityStatus { get; set; }
}