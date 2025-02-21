

namespace ApiTalking.DTOs.Course; 
public class ResponseCourseDTO
{
    
    public int id { get; set; }
    public required string name { get; set; }

    public string? description { get; set; }
 
    public required string URL { get; set; }

    public int UserId { get; set; }  // 📌 Para saber quién creó el curso
    public string? userName { get; set; } // 📌 Nombre del usuario asociado

    public string EntityStatus { get; set; }  // 📌 Estado del curso (Activo/Inactivo)
    public string Level { get; set; }  // 📌 Nivel del curso (ej: Básico, Intermedio)
}