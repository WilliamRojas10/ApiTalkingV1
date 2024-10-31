using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DaoLibrary;
using DaoLibrary.EFCore;
using DaoLibrary.Interfaces.User;

using EntitiesLibrary.User;
using ApiTalking.DTO.User;
using ApiTalking.DTO.common;
using DaoLibrary.EFCore.User;

namespace ApiTalking.Controllers;


   [ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IDAOUser _daoUser;

    public UserController(DAOFactory daoFactory)
    {
        _daoUser = daoFactory.CreateDAOUser();
    }

    [HttpGet("paged")]
    public async Task<ActionResult> GetUser(int page, int pageSize)
    {
        try
        {
            var users = await _daoUser.GetAllUsers();
            if (users == null)
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
    public async Task<ActionResult> GetUserById(int idUser)
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
            return Ok(new ResponseUserDTO
            {
                name = user.Name,
                lastName = user.LastName,
                birthDate = user.BirthDate,
                nationality = user.Nationality,
                province = user.Province,
                email = user.Email
            });
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

     