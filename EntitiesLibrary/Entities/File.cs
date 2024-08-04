

namespace EntitiesLibrary.Entities;

public class PublishedFile {
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Type { get; set; }//FILE TYPE FOR KNOW IF IS FORMAT JPG, PNG, PDF, MP4 etc.
    public required string Path { get; set; }

}