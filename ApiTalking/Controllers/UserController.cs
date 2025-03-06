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
using EntitiesLibrary.Post;

namespace ApiTalking.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IDAOUser _daoUser;
    public UserController(IDAOUser daoUser)
    {
        _daoUser = daoUser;
    }


    [Authorize(Roles = "Administrator")]
    [HttpGet("paginado")]
    public async Task<IActionResult> GetUsers(int page, int pageSize)
    {
        try
        {
            (var users, int totalRecords) = await _daoUser.GetUsersPaged
            (
            page,
            pageSize
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
                province = user.Province,
                entityStatus = (int)user.EntityStatus
            });
            return Ok(new ResponseDTO
            {
                success = true,
                message = "Lista de usuarios paginados obtenidos exitosamente",
                data = new
                {
                    totalRecords,
                    users = userDTO
                }
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

    [Authorize(Roles = "Administrator")]
    [HttpGet("{idUser}")]
    public async Task<IActionResult> GetUserById
    (
        int idUser
        
    )
    {
        try
        {
            var user = await _daoUser.GetUserById(idUser);
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
                province = user.Province,
                entityStatus = (int)user.EntityStatus
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


    [Authorize(Roles = "Administrator, User")]
    [HttpGet("obtener-mi-usuario")]
    public async Task<IActionResult> GetMyUserByLogin ()
    {
        try
        {
            EntitiesLibrary.File.File file = null;
            var userIdClaim = User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized(new ErrorResponseDTO { success = false, message = "Usuario no autenticado." });
            int userId = int.Parse(userIdClaim);

            var user = await _daoUser.GetUserById(userId, EntitiesLibrary.Common.EntityStatus.Active);
            if (user == null)
            {
                return BadRequest(new ErrorResponseDTO
                {
                    success = false,
                    message = "No se encontró el usuario con el Id: " + userId
                });
            }
            return Ok(new ResponseDTO
            {
                success = true,
                message = "Se obtuvo los datos del usuario logueado",
                data = new ResponseUserDTO
                {
                    idUser = user.Id,
                    name = user.Name,
                    lastName = user.LastName,
                    email = user.Email,
                    birthDate = user.BirthDate.ToString(),
                    nationality = user.Nationality,
                    province = user.Province, 
                    
                }
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
                UserType = EntitiesLibrary.User.UserType.User
            };

            await _daoUser.AddUser(user);

            return Ok(new ResponseDTO
            {
                success = true,
                message = "Usuario guardado correctamente"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDTO
            {
                success = false,
                message = "Error al crear el usuario: " + ex.Message
            });
        }
    }

    [Authorize(Roles = "Administrator, User")]
    [HttpPut("modificar/{idUser}")]
    public async Task<IActionResult> UpdateUser(int idUser, [FromBody] RequestUserDTO userDTO)
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

            var user = await _daoUser.GetUserById(idUser);
            if (user == null)
            {
                return NotFound(new ErrorResponseDTO
                {
                    success = false,
                    message = "No se encontró el usuario con el Id: " + idUser
                });
            }
            if (!string.IsNullOrEmpty(userDTO.name))
            {
                user.Name = userDTO.name;
            }
            if (!string.IsNullOrEmpty(userDTO.lastName))
            { 
            user.LastName = userDTO.lastName;
        }
            if (!string.IsNullOrEmpty(userDTO.email))
            {
                user.Email = userDTO.email;
            }
            if (!string.IsNullOrEmpty(userDTO.birthDate))
            {
                user.BirthDate = Converter.convertStringToDateOnly(userDTO.birthDate);
            }
            if (!string.IsNullOrEmpty(userDTO.nationality))
            {
                user.Nationality = userDTO.nationality;
            }

            if (!string.IsNullOrEmpty(userDTO.province))
            {
                user.Province = userDTO.province;
            }

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
            

            var user = await _daoUser.GetUserById(idUser);
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
                message = "Usuario bloqueado correctamente"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDTO
            {
                success = false,
                message = "Error al bloquear el usuario: " + ex.Message
            });
        }
    }

    [Authorize(Roles = "Administrator")]
    [HttpPut("activar/{idUser}")]
    public async Task<IActionResult> ActivateUser(int idUser)
    {
        try
        {
            var activeStatus = EntitiesLibrary.Common.EntityStatus.Active;
            var user = await _daoUser.GetUserById(idUser);
            if (user == null)
            {
                return NotFound(new ErrorResponseDTO
                {
                    success = false,
                    message = "No se encontró el usuario con el Id: " + idUser
                });
            }
            if (user.EntityStatus == activeStatus)
            {
                return NotFound(new ErrorResponseDTO
                {
                    success = false,
                    message = "No se puede activar, el post se encuentra activo"
                });
            }
            user.EntityStatus = activeStatus;

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
    [HttpDelete("{idUser}")]
    public async Task<IActionResult> DeleteUser(int idUser)
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

