public class ResponseUserDTO
{
    public required int IdUser { get; set; }
    public required string Name { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string BirthDate { get; set; }
    public string? RegistrationDateTime { get; set; }
    public string? Nationality { get; set; } = "";
    public string? Province { get; set; } = "";
    public string? ProfileImagePath { get; set; }}