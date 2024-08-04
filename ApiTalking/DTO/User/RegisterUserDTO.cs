using EntitiesLibrary.Entities.Enum;

namespace ApiTalking.DTO.User;

public class RegisterUserDTO
{
        public required string Name { get; set; }
        public required string LastName { get; set; }
        public required string Password { get; set; }
        public required string Email { get; set; }
        public required DateTime BirthDate { get; set; }
        public string? Nationality { get; set; }
        public string? Province { get; set; }
        public UserStatus UserStatus { get; set; } = UserStatus.Active;
}