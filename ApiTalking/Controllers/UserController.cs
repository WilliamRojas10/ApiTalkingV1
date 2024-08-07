using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiTalking.Data;
using EntitiesLibrary.Entities;
using EntitiesLibrary.Entities.Enum;
using ApiTalking.DTO.User;
using ApiTalking.DTO.common;

namespace ApiTalking.Controllers;
[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    //CONEXION A LA CONTEXTUALIZACION DE LA BASE DE DATOS
    private readonly MyDbContext _context;
    public UserController(MyDbContext context)
    {
        _context = context;
    }
    //FIN

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ResponseUserDTO>>> GetUsers()
    {
        try
        {
            return await _context.Users
                .Where(u => u.UserStatus == UserStatus.Active)
                .Select(u => new ResponseUserDTO
                {
                    name = u.Name,
                    lastName = u.LastName,
                    birthDate = u.BirthDate,
                    nationality = u.Nationality,
                    province = u.Province,
                    userStatus = u.UserStatus.ToString(),
                    age = CalculateAge(u.BirthDate) // calcule age
                })
                .ToListAsync();
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDTO
            {
                sucess = false,
                message = ex.Message
            });
        }
    }



    [HttpGet("{id}")]
    public async Task<ActionResult<ResponseUserDTO>> GetUser(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound();
        }
        return new ResponseUserDTO
        {
            name = user.Name,
            lastName = user.LastName,
            birthDate = user.BirthDate,
            nationality = user.Nationality,
            province = user.Province,
            userStatus = user.UserStatus.ToString(),
            age = CalculateAge(user.BirthDate) // calcule age
        };
    }
    // Fuction to calculate age
    private static int CalculateAge(DateTime birthDate)
    {
        var today = DateTime.Today;
        var age = today.Year - birthDate.Year;
        if (birthDate.Date > today.AddYears(-age)) age--;
        return age;
    }


    [HttpPost]
    public async Task<ActionResult> CreateUser(RegisterUserDTO userDTO)
    {
        try
        {
            if (userDTO == null)
            {
                return BadRequest(new ErrorResponseDTO
                {
                    sucess = false,
                    message = "Datos ingresados erroneos"
                });
            }
            if (string.IsNullOrEmpty(userDTO.name))
            {
                return BadRequest(new ErrorResponseDTO
                {
                    sucess = false,
                    message = "El nombre es un dato obligatorio"
                });
            }
            if (string.IsNullOrEmpty(userDTO.lastName))
            {
                return BadRequest(new ErrorResponseDTO
                {
                    sucess = false,
                    message = "El apellido es un dato obligatorio"
                });
            }
            if (string.IsNullOrEmpty(userDTO.email))
            {
                return BadRequest(new ErrorResponseDTO
                {
                    sucess = false,
                    message = "El correo es un dato obligatorio"
                });
            }
            if (string.IsNullOrEmpty(userDTO.password))
            {
                return BadRequest(new ErrorResponseDTO
                {
                    sucess = false,
                    message = "La contraseña es un dato obligatorio"
                });
            }
            var userSaved = new User
            {
                Name = userDTO.name,
                LastName = userDTO.lastName,
                Email = userDTO.email,
                Password = userDTO.password, // Hashing the password
                BirthDate = userDTO.birthDate,
                Nationality = userDTO.nationality,
                Province = userDTO.province,
                UserStatus = userDTO.userStatus
            };

            _context.Users.Add(userSaved);
            await _context.SaveChangesAsync();

            return Ok(new ResponseDTO
            {
                sucess = true,
                message = "Usuario creado con exito"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDTO
            {
                sucess = false,
                message = ex.Message
            });
        }
    }

    [HttpPut("Eliminar/{id}")]
    public async Task<IActionResult> DeletedUser(int id)
    {
        try
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            if (user.UserStatus == UserStatus.Deleted)
            {
                return BadRequest("El usuario está eliminado, por lo que no puede ser eliminado.");
            }
            user.UserStatus = UserStatus.Deleted;
            await _context.SaveChangesAsync();

            return Ok(new ResponseDTO
            {
                sucess = true,
                message = "Usuario eliminado."
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDTO
            {
                sucess = false,
                message = ex.Message
            });
        }
    }


    //TODO Falta hacer mas robusto la modificacion
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, User userDTO)
    {
        if (id != userDTO.Id)
        {
            return BadRequest();
        }

        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        user.Name = userDTO.Name;
        user.LastName = userDTO.LastName;
        user.Email = userDTO.Email;
        user.Nationality = userDTO.Nationality;
        user.Province = userDTO.Province;
        user.UserStatus = userDTO.UserStatus;

        await _context.SaveChangesAsync();

        return NoContent();
    }




    [HttpPut("CambiarEstado/{id}/status")]
    public async Task<IActionResult> UpdateStatusUserForId(int id, int status)
    {
        try
        {


            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            if (!Enum.IsDefined(typeof(UserStatus), status))
            {
                return BadRequest("Invalid status value");
            }
            user.UserStatus = (UserStatus)status;
            await _context.SaveChangesAsync();

            return Ok(new ResponseDTO
            {
                sucess = true,
                message = "El Estado del usuario fue cambiado exitosamente"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDTO
            {
                sucess = false,
                message = ex.Message
            });
        }
    }



    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return NoContent();
    }

}
