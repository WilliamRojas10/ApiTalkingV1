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
        public required string Name { get; set; }  

        public string? Description { get; set; }

       public required EntitiesLibrary.Common.EntityStatus EntityStatus{ get; set; } 

        public required EntitiesLibrary.User.User User { get; set; }

        public required string URL { get; set; }

        public required LevelCourse Level { get; set; } 

        
}

