using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntitiesLibrary.User;
[Table("user")]

public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }

        public void SetPassword(string password)
        {
            Password = BCrypt.Net.BCrypt.HashPassword(password);
        }
        public bool VerifyPassword(string password)
        {
            
            return BCrypt.Net.BCrypt.Verify(password, Password);
        }

        public required DateOnly BirthDate { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime RegistrationDateTime { get; set; }
        public string? Nationality { get; set; }
        public string? Province { get; set; }
        //public required UserStatus UserStatus { get; set; }

        public required EntitiesLibrary.User.UserType UserType { get; set; } 
        public required EntitiesLibrary.Common.EntityStatus EntityStatus { get; set; }
    

        //public required UserStatus UserStatus { get; set; }

         //public string? ProfileImagePath { get; set; }
    }

