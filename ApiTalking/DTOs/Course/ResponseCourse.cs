

namespace ApiTalking.DTOs.Course; 
public class ResponseCourseDTO
{
    
    public int id { get; set; }
    public required string name { get; set; }

    public string? description { get; set; }
 
    public required string URL { get; set; }
}