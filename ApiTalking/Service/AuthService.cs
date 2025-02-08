using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using DaoLibrary.Interfaces.User;
using EntitiesLibrary.User;

namespace ApiTalking.Service;
public class AuthService
{

    private readonly IDAOUser _daoUser;
    private readonly IConfiguration _config;
    public AuthService(IDAOUser daoUser, IConfiguration config)
    {
        _daoUser = daoUser;
        _config = config;
    }

    public async Task<string?> AuthenticateUser(string email, string password)
    {
        EntitiesLibrary.User.User? user = await _daoUser.GetUserByEmail(email);


        if (user == null  || user.EntityStatus != EntitiesLibrary.Common.EntityStatus.Active)
        {
            return null; // Usuario no encontrado, contraseña incorrecta o usuario inactivo
        }
        if (!user.VerifyPassword(password))
        {
            return null;
        }
        return GenerateJwtToken(user);
    }

    private string GenerateJwtToken(EntitiesLibrary.User.User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.Role, user.UserType.ToString()), // Admin o Usuario
            new Claim("UserId", user.Id.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2), // Expira en 2 horas
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
