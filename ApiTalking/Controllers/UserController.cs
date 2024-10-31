using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DaoLibrary.Interfaces.User;
using ApiTalking.DTO.common;

namespace ApiTalking.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IDAOUser _daoUser;

    // Cambiado a inyección directa de IDAOUser
    public UserController(IDAOUser daoUser)
    {
        _daoUser = daoUser;
    }

    [HttpGet("paged")]
    public async Task<IActionResult> GetUser(int page, int pageSize)
    {
        try
        {
            (var users, int totalRegistro) = await _daoUser.GetUsersPaged(page, pageSize, null);
            if (users == null || !users.Any())
            {
                return BadRequest(new ErrorResponseDTO
                {
                    sucess = false,
                    message = "No se encontraron usuarios"
                });
            }
            return Ok(users);
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDTO
            {
                sucess = false,
                message = "Error en getUser(): " + ex.Message
            });
        }
    }

    [HttpGet("{idUser}")]
    public async Task<IActionResult> GetUserById(int idUser)
    {
        try
        {
            var user = await _daoUser.GetUserById(idUser);
            if (user == null)
            {
                return BadRequest(new ErrorResponseDTO
                {
                    sucess = false,
                    message = "No se encontró el usuario con el Id: " + idUser
                });
            }
            return Ok(user); // O aquí puedes mapear a ResponseUserDTO
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDTO
            {
                sucess = false,
                message = "Error en getUserById(): " + ex.Message
            });
        }
    }
}
