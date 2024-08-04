using EntitiesLibrary.Entities.Enum;

namespace EntitiesLibrary.Entities

{
    public class User
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required DateTime BirthDate { get; set; }
        public string? Nationality { get; set; }
        public string? Province { get; set; }

        // Enum para el estado del usuario
        public UserStatus UserStatus { get; set; }
    }
}
