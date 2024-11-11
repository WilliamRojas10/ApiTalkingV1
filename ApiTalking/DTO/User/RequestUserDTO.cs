
 namespace ApiTalking.DTO.User;
 public class RequestUserDTO
 {
         public required string name { get; set; }
         public required string lastName { get; set; }
         public required string password { get; set; }
         public required string email { get; set; }
         public required string birthDate { get; set; }
         public string? nationality { get; set; }
         public string? province { get; set; }
        // public int userStatus { get; set; } 
 }

