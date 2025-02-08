using DaoLibrary.Interfaces.User;
using ApiTalking.DTOs.Login;
using ApiTalking.DTOs.common;
using Microsoft.AspNetCore.Mvc;
using ApiTalking.Service;

namespace ApiTalking.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{

    private readonly ILogger<LoginController> _logger;
    private readonly AuthService _authService;
    private readonly IDAOUser _daoUser;
    public LoginController
    (
        ILogger<LoginController> logger,
        IDAOUser daoUser,
        AuthService authService
    )
    {
        _logger = logger;
        _daoUser = daoUser;
        _authService = authService;

    }


  

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] RequestLoginDTO requestLoginDTO)
    {
        var token = await _authService.AuthenticateUser(requestLoginDTO.email, requestLoginDTO.password);

        if (token == null)
        {
            return Unauthorized(new { message = "Usuario o contraseña incorrectos" });
        }

        return Ok(new ResponseDTO
        {
            sucess = true,
            message = "Login exitoso",
            data = new ResponseLoginDTO
            {
                email = requestLoginDTO.email,
                token = token
            }
        });
    }

}