namespace ApiTalking.DTOs.Course
{
    public class RequestCourseDTO
    {
        public required string name { get; set; }
        public string? description { get; set; }
        public required int userId { get; set; }

       
        

        public required string URL { get; set; }
        public string Level { get; set; } // Nivel del curso como cadena

        // Agregar esta propiedad si es necesaria
        
    }
}
