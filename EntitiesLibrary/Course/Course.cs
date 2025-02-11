using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace EntitiesLibrary.Course;
[Table("course")]

public class Course
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }  // Ej: "Curso de Inglés"

        public string? Description { get; set; }

       public required EntitiesLibrary.Common.EntityStatus EntityStatus{ get; set; } 

        public required EntitiesLibrary.User.User User { get; set; } // Usuario obligatorio

        public required string URL { get; set; }
    }

