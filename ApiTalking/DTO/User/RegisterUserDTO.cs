using EntitiesLibrary.Entities.Enum;

namespace ApiTalking.DTO.User;

public class RegisterUserDTO
{
        public required string name { get; set; }
        public required string lastName { get; set; }
        public required string password { get; set; }
        public required string email { get; set; }
        public required DateTime birthDate { get; set; }
        public string? nationality { get; set; }
        public string? province { get; set; }
        public UserStatus userStatus { get; set; } = UserStatus.Active;
}