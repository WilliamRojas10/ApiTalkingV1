using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntitiesLibrary.FileSystem;

public class PublishedFile {
     [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Type { get; set; }//FILE TYPE FOR KNOW IF IS FORMAT JPG, PNG, PDF, MP4 etc.
    public required string Path { get; set; }

}