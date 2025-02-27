using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using EntitiesLibrary.Course;
using DaoLibrary.Interfaces.Course;
using ApiTalking.DTOs.common;
using ApiTalking.DTOs.Course;
using ApiTalking.Helpers;
using ApiTalking.DTOs.Post;
using Microsoft.AspNetCore.Authorization;
using DaoLibrary.Interfaces.User;
using EntitiesLibrary.User;



namespace ApiTalking.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CourseController : ControllerBase
{
    private readonly IDAOCourse _daoCourse;
    private readonly IDAOUser _daoUser;


    public CourseController(IDAOCourse daoCourse, IDAOUser daoUser)
    {
        _daoCourse = daoCourse;
        _daoUser=daoUser;
    }


    [HttpGet("paginado")]
    public async Task<IActionResult> GetCourses(int page, int pageSize)
    {
        try
        {
            var activeStatus = EntitiesLibrary.Common.EntityStatus.Active;
            (var Courses, int totalRecords) = await _daoCourse.GetCoursesPaged
            (
            page,
            pageSize,
            activeStatus
            );
            if (Courses == null || !Courses.Any())
            {
                return BadRequest(new ErrorResponseDTO
                {
                    success = false,
                    message = "No se encontraron cursos"
                });
            }
            var CourseDTO = Courses.Select(Course => new ResponseCourseDTO
            {
                id = Course.Id,
                name = Course.Name,
                description = Course.Description,
                URL = Course.URL,


            });
            return Ok(new
            {
                totalRecords,
                Courses = CourseDTO
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDTO
            {
                success = false,
                message = "Error en getCourses(): " + ex.Message
            });
        }
    }

    [Authorize(Roles = "Administrator")]
    [HttpGet("{idCourse}")]
    public async Task<IActionResult> GetCourseById
    (
        int idCourse,
        EntitiesLibrary.Common.EntityStatus entityStatus
    )
    {
        try
        {
            var Course = await _daoCourse.GetCourseById(idCourse, entityStatus);
            if (Course == null)
            {
                return BadRequest(new ErrorResponseDTO
                {
                    success = false,
                    message = "No se encontró el curso con el Id: " + idCourse
                });
            }
            return Ok(new ResponseCourseDTO
            {
                id = Course.Id,
                name = Course.Name,
                description = Course.Description,
                URL = Course.URL,

            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDTO
            {
                success = false,
                message = "Error al obtener un curso usando GetCourseById(): " + ex.Message
            });
        }
    }

    [Authorize(Roles = "Administrator")]
    [HttpPost]
    public async Task<IActionResult> CreateCourse([FromBody] RequestCourseDTO CourseDTO)
    {
        try
        {
            if (CourseDTO == null)
            {
                return BadRequest(new ErrorResponseDTO
                {
                    success = false,
                    message = "Datos del curso no válidos",


                });
            }

            // Corrección aquí: debe ser CourseDTO.UserId (no userId)
            var user = await _daoUser.GetUserById(CourseDTO.userId);
            if (user == null)
            {
                return BadRequest(new ErrorResponseDTO
                {
                    success = false,
                    message = "Usuario asociado al curso no encontrado"

                });
            }

            var course = new Course
            {
                Name = CourseDTO.name, // También corregí la mayúscula en Name
                Description = CourseDTO.description,
                URL = CourseDTO.URL,
                EntityStatus = EntitiesLibrary.Common.EntityStatus.Active,
                User = user,
                Level = Enum.Parse<LevelCourse>(CourseDTO.Level, true) //  'true' para que ignore mayúsculas/minúsculas
            };

            await _daoCourse.AddCourse(course);

            return Ok(new ResponseDTO
            {
                success = true,
                message = "Curso guardado correctamente"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDTO
            {
                success = false,
                message = "Error al crear el curso: " + ex.Message
            });
        }
    }

    //[HttpPost]
    //public async Task<IActionResult> CreateCourse([FromBody] RequestCourseDTO CourseDTO)
    //{
    //    try
    //    {
    //        if (CourseDTO == null)
    //        {
    //            return BadRequest(new ErrorResponseDTO
    //            {
    //                success = false,
    //                message = "Datos del usuario no válidos",
    //                user = _daoUser.GetUserById(CourseDTO.id)

    //            });
    //        }

    //        var Course = new Course
    //        {

    //            Id = CourseDTO.id,
    //            Name = CourseDTO.name,
    //            Description = CourseDTO.description,
    //            URL = CourseDTO.URL,
    //            EntityStatus = EntitiesLibrary.Common.EntityStatus.Active,
    //            User = _daoUser.GetUserById(CourseDTO.id)

    //        };

    //        await _daoCourse.AddCourse(Course);

    //        return Ok(new ResponseDTO
    //        {
    //            success = true,
    //            message = "Usuario guardado correctamente"
    //        });
    //    }
    //    catch (Exception ex)
    //    {
    //        return BadRequest(new ErrorResponseDTO
    //        {
    //            success = false,
    //            message = "Error al crear el usuario: " + ex.Message
    //        });
    //    }
    //}


    [HttpPut("modificar/{idCourse}")]
    public async Task<IActionResult> UpdateCourse(int idCourse, [FromBody] RequestCourseDTO CourseDTO)
    {
        try
        {
            var activeStatus = EntitiesLibrary.Common.EntityStatus.Active;
            if (CourseDTO == null)
            {
                return BadRequest(new ErrorResponseDTO
                {
                    success = false,
                    message = "Datos del curso no válidos"
                });
            }

            var Course = await _daoCourse.GetCourseById(idCourse, activeStatus);
            if (Course == null)
            {
                return NotFound(new ErrorResponseDTO
                {
                    success = false,
                    message = "No se encontró el curso con el Id: " + idCourse
                });
            }
            Course.Name = CourseDTO.name;
            Course.Description = CourseDTO.description;
            Course.URL = CourseDTO.URL;




            await _daoCourse.UpdateCourse(Course);

            return Ok(new ResponseDTO
            {
                success = true,
                message = "curso actualizado correctamente"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDTO
            {
                success = false,
                message = "Error al actualizar el curso: " + ex.Message
            });
        }
    }

    [Authorize(Roles = "Administrator")]
    [HttpPut("bloquear/{idCourse}")]
    public async Task<IActionResult> BlockCourse(int idCourse)
    {
        try
        {
            var activeStatus = EntitiesLibrary.Common.EntityStatus.Active;

            var Course = await _daoCourse.GetCourseById(idCourse, activeStatus);
            if (Course == null)
            {
                return NotFound(new ErrorResponseDTO
                {
                    success = false,
                    message = "No se encontró el curso con el Id: " + idCourse
                });
            }
            Course.EntityStatus = EntitiesLibrary.Common.EntityStatus.Blocked;

            await _daoCourse.UpdateCourse(Course);

            return Ok(new ResponseDTO
            {
                success = true,
                message = "curso eliminado correctamente"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDTO
            {
                success = false,
                message = "Error al actualizar el curso: " + ex.Message
            });
        }
    }

    //[Authorize(Roles = "Administrator")]
    //[HttpPut("activar/{idCourse}")]
    //public async Task<IActionResult> ActivateCourse(int idCourse)
    //{
    //    try
    //    {
    //        var Course = await _daoCourse.GetCourseById(idCourse);
    //        if (Course == null)
    //        {
    //            return NotFound(new ErrorResponseDTO
    //            {
    //                success = false,
    //                message = "No se encontró el usuario con el Id: " + idCourse
    //            });
    //        }
    //        Course.EntityStatus = EntitiesLibrary.Common.EntityStatus.Active;

    //        await _daoCourse.UpdateCourse(Course);

    //        return Ok(new ResponseDTO
    //        {
    //            success = true,
    //            message = "Curso activado correctamente"
    //        });
    //    }
    //    catch (Exception ex)
    //    {
    //        return BadRequest(new ErrorResponseDTO
    //        {
    //            success = false,
    //            message = "Error al activar el usuario: " + ex.Message
    //        });
    //    }
    //}

    [Authorize(Roles = "Administrator")]
    [HttpPut("activar/{idCourse}")]
    public async Task<IActionResult> ActivateCourse(int idCourse)
    {
        try
        {
            var activeStatus = EntitiesLibrary.Common.EntityStatus.Active;

            // Llamar a GetCourseById con el parámetro entityStatus
            var course = await _daoCourse.GetCourseById(idCourse, activeStatus);

            if (course == null)
            {
                return NotFound(new ErrorResponseDTO
                {
                    success = false,
                    message = "No se encontró el curso con el Id: " + idCourse
                });
            }

            // Activar el curso
            course.EntityStatus = EntitiesLibrary.Common.EntityStatus.Active;

            // Actualizar el curso
            await _daoCourse.UpdateCourse(course);

            return Ok(new ResponseDTO
            {
                success = true,
                message = "Curso activado correctamente"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDTO
            {
                success = false,
                message = "Error al activar el curso: " + ex.Message
            });
        }
    }

    [Authorize(Roles = "Administrator")]
    [HttpDelete("{idCourse}")]
    public async Task<IActionResult> DeleteCourse(int idCourse)
    {
        try
        {
            var activeStatus = EntitiesLibrary.Common.EntityStatus.Active;
            var Course = await _daoCourse.GetCourseById(idCourse, activeStatus);
            if (Course == null)
            {
                return NotFound(new ErrorResponseDTO
                {
                    success = false,
                    message = "No se encontró el curso con el Id: " + idCourse
                });
            }
            Course.EntityStatus = EntitiesLibrary.Common.EntityStatus.Deleted;

            await _daoCourse.UpdateCourse(Course);

            return Ok(new ResponseDTO
            {
                success = true,
                message = "Curso eliminado correctamente"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDTO
            {
                success = false,
                message = "Error al actualizar el Curso: " + ex.Message
            });
        }
    }


    [HttpPost("create-course/{idCourse}")]
    public async Task<IActionResult> RequestCourseDTO([FromBody] RequestCourseDTO courseCreateDTO)
    {
        try
        {
            EntitiesLibrary.User.User? user = await _daoUser.GetUserById(courseCreateDTO.userId);
            // Convertir el string del nivel a LevelCourse (enum)
            if (Enum.TryParse(courseCreateDTO.Level, true, out LevelCourse levelEnum))
            {
                var newCourse = new Course
                {
                    Name = courseCreateDTO.name,
                    Description = courseCreateDTO.description,
                    EntityStatus = EntitiesLibrary.Common.EntityStatus.Active,
                    User = user,
                    URL = courseCreateDTO.URL,
                    Level = levelEnum  //  asignar el nivel correctamente
                };

                await _daoCourse.AddCourse(newCourse);
                return Ok(new { message = "Curso creado exitosamente." }); }
            else
            {
                return BadRequest(new { message = "Nivel de curso no válido." });
            }
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Error al crear el curso: " + ex.Message });
        }
    }



    [HttpGet("filtrar-paged-level/{level}")]
    public async Task<IActionResult> GetCoursesByLevel(string level, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            if (level == "Todos")
            {
                (var coursesAll, int totalRecordsAll) = await _daoCourse.GetCoursesPaged
                (
                   page,
                   pageSize,
                   EntitiesLibrary.Common.EntityStatus.Active
                );

                return Ok(new ResponseDTO
                {
                    success = true,
                    message = "Se obtuvo los cursos filtados correctamente",
                    data = new {
                        courses = coursesAll,
                        totalRecords = totalRecordsAll }
                });
            }
            if (!Enum.TryParse(level, true, out LevelCourse levelEnum))
            {
                return BadRequest(new ErrorResponseDTO
                {   success = false,
                    message = "Nivel de curso no válido." 
                });
            }

            (var coursesFiltered, int totalRecordsFiltered) = await _daoCourse.GetCoursesByLevel(levelEnum, page, pageSize);

            if (coursesFiltered == null || !coursesFiltered.Any())
            {
                return NotFound(new ErrorResponseDTO 
                {   success = false,
                    message = "No hay cursos disponibles para este nivel." 
                });
            }

            return Ok(new ResponseDTO
            {
                success = true,
                message = "Se obtuvo los cursos filtados correctamente",
                data = new
                {
                    courses = coursesFiltered,
                    totalRecords = totalRecordsFiltered
                }
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDTO
            {
                message = "Error al obtener cursos: " + ex.Message,
                success = false
            });
        }
    }
}

