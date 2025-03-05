namespace ApiTalking.DTOs.Course;
    public class RequestCourseDTO
    {
        public required string name { get; set; }
        public string? description { get; set; }
        public required string URL { get; set; }
        public int level { get; set; }


        
    }

