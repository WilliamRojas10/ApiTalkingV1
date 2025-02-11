using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using EntitiesLibrary.User;
using DaoLibrary.Interfaces.User;
using ApiTalking.DTOs.common;
using ApiTalking.DTOs.User;
using ApiTalking.Helpers;
using ApiTalking.DTOs.Post;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ApiTalking.Controllers;
//[Authorize(Roles = "Administrator")]

[ApiController]
[Route("api/[controller]")]
public class AdministratorController : ControllerBase
{
    private readonly IDAOUser _daoUser;


    public AdministratorController(IDAOUser daoUser)
    {
        _daoUser = daoUser;
    }

    [Authorize(Roles = "Administrator")]
    [HttpGet("paginado")]
    public async Task<IActionResult> GetAdministrators(int page, int pageSize)
    {
        try
        {
            var activeStatus = EntitiesLibrary.Common.EntityStatus.Active;
            (var users, int totalRecords) = await _daoUser.GetUsersPaged
            (
            page,
            pageSize,
            activeStatus
            );
            if (users == null || !users.Any())
            {
                return BadRequest(new ErrorResponseDTO
                {
                    success = false,
                    message = "No se encontraron usuarios"
                });
            }
            var userDTO = users.Select(user => new ResponseUserDTO
            {
                idUser = user.Id,
                name = user.Name,
                lastName = user.LastName,
                email = user.Email,
                birthDate = user.BirthDate.ToString(),
                nationality = user.Nationality,
                province = user.Province
            });
            return Ok(new
            {
                totalRecords = totalRecords,
                users = userDTO
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDTO
            {
                success = false,
                message = "Error en getUsers(): " + ex.Message
            });
        }
    }

    [HttpGet("automatic")]
    public async Task<IActionResult> GetUserById
(
    //int idUser,
    //EntitiesLibrary.Common.EntityStatus entityStatus
)
    {
        try
        {
            // Obtener el id del administrador logueado desde los claims
            var administratorId = User.FindFirstValue("UserId"); // Usamos "UserId" que se agregó en los claims
            var userRole = User.FindFirstValue(ClaimTypes.Role); // Obtener el rol (por ejemplo, "Administrator")
            var userEmail = User.FindFirstValue(ClaimTypes.Name); // Obtener el email del administrador

            int idAdministrator = Int32.Parse(administratorId);
            // Imprimir los claims para depuración o auditoría
            Console.WriteLine($"Administrator ID: {administratorId}");
            Console.WriteLine($"Role: {userRole}");
            Console.WriteLine($"Email: {userEmail}");
            var active = EntitiesLibrary.Common.EntityStatus.Active;

            // Obtener el usuario por su id
            var user = await _daoUser.GetUserById(idAdministrator, active);
            if (user == null)
            {
                return BadRequest(new ErrorResponseDTO
                {
                    success = false,
                    message = "No se encontró el usuario con el Id: " + idAdministrator
                });
            }

            // Devolver la información del usuario junto con los claims
            return Ok(new
            {
                AdministratorId = administratorId,
                Role = userRole,
                Email = userEmail,
                UserData = new ResponseUserDTO
                {
                    idUser = user.Id,
                    name = user.Name,
                    lastName = user.LastName,
                    email = user.Email,
                    birthDate = user.BirthDate.ToString(),
                    nationality = user.Nationality,
                    province = user.Province
                }
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDTO
            {
                success = false,
                message = "Error al obtener un usuario usando VIPGetUserById(): " + ex.Message
            });
        }
    }


    


    [HttpGet("{idUser}")]
    public async Task<IActionResult> GetAdministratorById
    (
        int idUser,
        EntitiesLibrary.Common.EntityStatus entityStatus
    )
    {
        try
        {
            var user = await _daoUser.GetUserById(idUser, entityStatus);
            if (user == null)
            {
                return BadRequest(new ErrorResponseDTO
                {
                    success = false,
                    message = "No se encontró el usuario con el Id: " + idUser
                });
            }
            return Ok(new ResponseUserDTO
            {
                idUser = user.Id,
                name = user.Name,
                lastName = user.LastName,
                email = user.Email,
                birthDate = user.BirthDate.ToString(),
                nationality = user.Nationality,
                province = user.Province
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDTO
            {
                success = false,
                message = "Error al obtener un usuario usando GetUserById(): " + ex.Message
            });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] RequestUserDTO userDTO)
    {
        try
        {
            if (userDTO == null)
            {
                return BadRequest(new ErrorResponseDTO
                {
                    success = false,
                    message = "Datos del usuario no válidos"
                });
            }

            var user = new User
            {
                Name = userDTO.name,
                LastName = userDTO.lastName,
                Email = userDTO.email,
                Password = userDTO.password,
                BirthDate = Converter.convertStringToDateOnly(userDTO.birthDate),
                Nationality = userDTO.nationality,
                Province = userDTO.province,
                EntityStatus = EntitiesLibrary.Common.EntityStatus.Active,
                UserType = EntitiesLibrary.User.UserType.Administrator
                
            };

            await _daoUser.AddUser(user);

            return Ok(new ResponseDTO
            {
                success = true,
                message = "Usuario ADMISTRADOR guardado correctamente"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDTO
            {
                success = false,
                message = "Error al crear el usuario de ADMINISTRADOR: " + ex.Message
            });
        }
    }
    [Authorize(Roles = "Administrator")]

    [HttpPut("modificar/{idUser}")]
    public async Task<IActionResult> UpdateUser(int idUser, [FromBody] RequestUserDTO userDTO)
    {
        try
        {
            var activeStatus = EntitiesLibrary.Common.EntityStatus.Active;
            if (userDTO == null)
            {
                return BadRequest(new ErrorResponseDTO
                {
                    success = false,
                    message = "Datos del usuario no válidos"
                });
            }

            var user = await _daoUser.GetUserById(idUser, activeStatus);
            if (user == null)
            {
                return NotFound(new ErrorResponseDTO
                {
                    success = false,
                    message = "No se encontró el usuario con el Id: " + idUser
                });
            }
            user.Name = userDTO.name;
            user.LastName = userDTO.lastName;
            user.Email = userDTO.email;
            user.BirthDate = Converter.convertStringToDateOnly(userDTO.birthDate);
            user.Nationality = userDTO.nationality;
            user.Province = userDTO.province;

            await _daoUser.UpdateUser(user);

            return Ok(new ResponseDTO
            {
                success = true,
                message = "Usuario actualizado correctamente"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDTO
            {
                success = false,
                message = "Error al actualizar el usuario: " + ex.Message
            });
        }
    }
    [Authorize(Roles = "Administrator")]

    [HttpPut("bloquear/{idUser}")]
    public async Task<IActionResult> BlockUser(int idUser)
    {
        try
        {
            var activeStatus = EntitiesLibrary.Common.EntityStatus.Active;

            var user = await _daoUser.GetUserById(idUser, activeStatus);
            if (user == null)
            {
                return NotFound(new ErrorResponseDTO
                {
                    success = false,
                    message = "No se encontró el usuario con el Id: " + idUser
                });
            }
            user.EntityStatus = EntitiesLibrary.Common.EntityStatus.Blocked;

            await _daoUser.UpdateUser(user);

            return Ok(new ResponseDTO
            {
                success = true,
                message = "Usuario eliminado correctamente"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDTO
            {
                success = false,
                message = "Error al actualizar el usuario: " + ex.Message
            });
        }
    }
    [Authorize(Roles = "Administrator")]


    [HttpPut("activar/{idUser}")]
    public async Task<IActionResult> ActivateUser(int idUser)
    {
        try
        {
            var user = await _daoUser.GetUserById(idUser);
            if (user == null)
            {
                return NotFound(new ErrorResponseDTO
                {
                    success = false,
                    message = "No se encontró el usuario con el Id: " + idUser
                });
            }
            user.EntityStatus = EntitiesLibrary.Common.EntityStatus.Active;

            await _daoUser.UpdateUser(user);

            return Ok(new ResponseDTO
            {
                success = true,
                message = "Usuario activado correctamente"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDTO
            {
                success = false,
                message = "Error al activar el usuario: " + ex.Message
            });
        }
    }
    [Authorize(Roles = "Administrator")]

    [HttpDelete("eliminar/{idUser}")]
    public async Task<IActionResult> DeleteUser(int idUser)
    {
        try
        {
            var activeStatus = EntitiesLibrary.Common.EntityStatus.Active;
            var user = await _daoUser.GetUserById(idUser, activeStatus);
            if (user == null)
            {
                return NotFound(new ErrorResponseDTO
                {
                    success = false,
                    message = "No se encontró el usuario con el Id: " + idUser
                });
            }
            user.EntityStatus = EntitiesLibrary.Common.EntityStatus.Deleted;

            await _daoUser.UpdateUser(user);

            return Ok(new ResponseDTO
            {
                success = true,
                message = "Usuario eliminado correctamente"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDTO
            {
                success = false,
                message = "Error al actualizar el usuario: " + ex.Message
            });
        }
    }



}

