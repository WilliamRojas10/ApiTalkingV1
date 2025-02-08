using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using EntitiesLibrary.User;
using DaoLibrary.Interfaces.User;
using ApiTalking.DTOs.common;
using ApiTalking.DTOs.User;
using ApiTalking.Helpers;
using ApiTalking.DTOs.Post;
using Microsoft.AspNetCore.Authorization;

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
                    sucess = false,
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
                sucess = false,
                message = "Error en getUsers(): " + ex.Message
            });
        }
    }

    [Authorize(Roles = "Administrator")]
    [HttpGet("{idUser}")]
    public async Task<IActionResult> GetUserById
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
                    sucess = false,
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
                sucess = false,
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
                    sucess = false,
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
                sucess = true,
                message = "Usuario guardado correctamente"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDTO
            {
                sucess = false,
                message = "Error al crear el usuario: " + ex.Message
            });
        }
    }


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
                    sucess = false,
                    message = "Datos del usuario no válidos"
                });
            }

            var user = await _daoUser.GetUserById(idUser, activeStatus);
            if (user == null)
            {
                return NotFound(new ErrorResponseDTO
                {
                    sucess = false,
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
                sucess = true,
                message = "Usuario actualizado correctamente"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDTO
            {
                sucess = false,
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
                    sucess = false,
                    message = "No se encontró el usuario con el Id: " + idUser
                });
            }
            user.EntityStatus = EntitiesLibrary.Common.EntityStatus.Blocked;

            await _daoUser.UpdateUser(user);

            return Ok(new ResponseDTO
            {
                sucess = true,
                message = "Usuario eliminado correctamente"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDTO
            {
                sucess = false,
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
                    sucess = false,
                    message = "No se encontró el usuario con el Id: " + idUser
                });
            }
            user.EntityStatus = EntitiesLibrary.Common.EntityStatus.Active;

            await _daoUser.UpdateUser(user);

            return Ok(new ResponseDTO
            {
                sucess = true,
                message = "Usuario activado correctamente"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDTO
            {
                sucess = false,
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
                    sucess = false,
                    message = "No se encontró el usuario con el Id: " + idUser
                });
            }
            user.EntityStatus = EntitiesLibrary.Common.EntityStatus.Deleted;

            await _daoUser.UpdateUser(user);

            return Ok(new ResponseDTO
            {
                sucess = true,
                message = "Usuario eliminado correctamente"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDTO
            {
                sucess = false,
                message = "Error al actualizar el usuario: " + ex.Message
            });
        }
    }


    // agregar imagen desde el ordenador -------------------------------------------
    [HttpPost("subir-imagen")]
    public async Task<IActionResult> UploadUserImage([FromForm] UploadFileDTO imageDTO)
    {
        try
        {
            // Validar si la imagen se proporciona correctamente
            if (imageDTO.image == null || imageDTO.image.Length == 0)
            {
                return BadRequest(new ErrorResponseDTO
                {
                    sucess = false,
                    message = "No se proporcionó ninguna imagen o el archivo está vacío."
                });
            }

            // Verificar si el usuario existe
            var user = await _daoUser.GetUserById(imageDTO.userId);
            if (user == null)
            {
                return NotFound(new ErrorResponseDTO
                {
                    sucess = false,
                    message = $"No se encontró el usuario con el Id: {imageDTO.userId}"
                });
            }

            // Validar la extensión del archivo
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var extension = Path.GetExtension(imageDTO.image.FileName).ToLower();
            if (!allowedExtensions.Contains(extension))
            {
                return BadRequest(new ErrorResponseDTO
                {
                    sucess = false,
                    message = "El formato del archivo no es válido. Solo se permiten .jpg, .jpeg, .png, .gif."
                });
            }

            // Preparar el directorio de almacenamiento
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "users");
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            // Generar un nombre único para la imagen
            var imageFileName = $"{imageDTO.userId}_{Guid.NewGuid()}{extension}";
            var imagePath = Path.Combine(uploadPath, imageFileName);

            // Guardar la imagen físicamente en el servidor
            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await imageDTO.image.CopyToAsync(fileStream);
            }

            // Actualizar la información del usuario con la nueva ruta de imagen
            user.ProfileImagePath = $"/images/users/{imageFileName}";
            await _daoUser.UpdateUser(user);

            return Ok(new ResponseDTO
            {
                sucess = true,
                message = "La imagen fue subida exitosamente.",
                data = new { userId = imageDTO.userId, imagePath = user.ProfileImagePath }
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDTO
            {
                sucess = false,
                message = "Error al subir la imagen: " + ex.Message
            });
        }
    }



}

