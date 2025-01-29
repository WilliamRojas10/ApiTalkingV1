using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using EntitiesLibrary.User;
using DaoLibrary.Interfaces.User;
using ApiTalking.DTO.common;
using ApiTalking.DTO.User;
using ApiTalking.Helpers;

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
                    EntityStatus = EntitiesLibrary.Common.EntityStatus.Active
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
                user.UserStatus = EntitiesLibrary.Common.EntityStatus.Active;

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

        [HttpPut("eliminar/{idUser}")]
        public async Task<IActionResult> DeleteUser(int idUser)
        {
            try
            {
                var activeStatus = EntitiesLibrary.Common.EntityStatus.Active;
                var user = await _daoUser.GetUserById(idUser, activeStatus );
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



    



}
