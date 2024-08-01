using Microsoft.AspNetCore.Mvc;

using EntitiesLibrary.Entities;
using EntitiesLibrary.Entities.Enum;

namespace ApiTalking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserMock : ControllerBase
    {

        //MOCK DE USUARIOS - DATOS DE PRUEBA
        private static List<User> users = new List<User>
        {
            new User
            {
                Id = 1,
                Name = "Javier",
                LastName = "Hernandez",
                Password = "1234",
                Email = "javier@gmail.com",
                BirthDate = DateTime.Now,
                Nationality = "Colombia",
                Province = "Bogota",
                UserStatus = UserStatus.Active
            },
            new User
            {
                Id = 2,
                Name = "Guillermo",
                LastName = "Di Marco",
                Password = "123412dqa",
                Email = "guillermo@hotmail.com",
                BirthDate = DateTime.Now,
                Nationality = "Argentina",
                Province = "Cordoba",
                UserStatus = UserStatus.Blocked
            }
        };

        //FIN

        //METODOS HTTP

        [HttpGet]
        public IActionResult GetUsers()
        {
            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            var user = users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public IActionResult CreateUser(User user)
        {
            user.Id = users.Max(u => u.Id) + 1;
            users.Add(user);
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, User updatedUser)
        {
            var user = users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            user.Name = updatedUser.Name;
            user.LastName = updatedUser.LastName;
            user.Password = updatedUser.Password;
            user.Email = updatedUser.Email;
            user.BirthDate = updatedUser.BirthDate;
            user.Nationality = updatedUser.Nationality;
            user.Province = updatedUser.Province;
            user.UserStatus = updatedUser.UserStatus;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            users.Remove(user);
            return NoContent();
        }
    }
}
