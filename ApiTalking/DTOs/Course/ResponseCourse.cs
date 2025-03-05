

namespace ApiTalking.DTOs.Course; 
public class ResponseCourseDTO
{
    
    public int id { get; set; }
    public required string name { get; set; }

    public string? description { get; set; }
 
    public required string URL { get; set; }

    public int UserId { get; set; }  
    public string? userName { get; set; } 

    public int entityStatus { get; set; } 
    public string Level { get; set; }  
    public int statusLevel { get; set; }
}