using Microsoft.AspNetCore.Mvc; // REALIZAR LOS METODOS HTTP
using Microsoft.EntityFrameworkCore; //Paquete para trabajar con la base de datos
using ApiTalking.Data; //Contextualizacion de la DB
using EntitiesLibrary.Entities; //Entidad con la que trabajo controller

using ApiTalking.DTO.User;

namespace ApiTalking.Controllers
{
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
        return await _context.Users
            .Select(u => new ResponseUserDTO
            {
                //Id = u.Id,
                Name = u.Name,
                LastName = u.LastName,
                //Email = u.Email,
                BirthDate = u.BirthDate,
                Nationality = u.Nationality,
                Province = u.Province,
                UserStatus = u.UserStatus.ToString()
            })
            .ToListAsync();
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
           // Id = user.Id,
            Name = user.Name,
            LastName = user.LastName,
            //Email = user.Email,
            BirthDate = user.BirthDate,
            Nationality = user.Nationality,
            Province = user.Province,
            UserStatus = user.UserStatus.ToString()
        };
    }

    [HttpPost]
    public async Task<ActionResult<ResponseUserDTO>> CreateUser(RegisterUserDTO userDTO)
    {
        var user = new User
        {
            Name = userDTO.Name,
            LastName = userDTO.LastName,
            Email = userDTO.Email,
            Password = userDTO.Password, // Hashing the password
            BirthDate = userDTO.BirthDate,
            Nationality = userDTO.Nationality,
            Province = userDTO.Province,
            UserStatus = userDTO.UserStatus
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, userDTO);
    }

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
        user.UserStatus = userDTO.UserStatus ;

        await _context.SaveChangesAsync();

        return NoContent();
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
}
