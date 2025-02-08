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

    [Authorize(Roles = "Administrator")]
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
                    sucess = false,
                    message = "No se encontraron usuarios"
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
                totalRecords = totalRecords,
                Courses = CourseDTO
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDTO
            {
                sucess = false,
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
                    sucess = false,
                    message = "No se encontró el usuario con el Id: " + idCourse
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
                sucess = false,
                message = "Error al obtener un usuario usando GetCourseById(): " + ex.Message
            });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateCourse([FromBody] RequestCourseDTO CourseDTO)
    {
        try
        {
            if (CourseDTO == null)
            {
                return BadRequest(new ErrorResponseDTO
                {
                    sucess = false,
                    message = "Datos del curso no válidos"
                });
            }

            var user = await _daoUser.GetUserById(CourseDTO.userId);
            if (user == null)
            {
                return BadRequest(new ErrorResponseDTO
                {
                    sucess = false,
                    message = "Usuario asociado al curso no encontrado"
                });
            }

            var course = new Course
            {
                Name = CourseDTO.name,
                Description = CourseDTO.description,
                URL = CourseDTO.URL,
                EntityStatus = EntitiesLibrary.Common.EntityStatus.Active,
                User = user
            };

            await _daoCourse.AddCourse(course);

            return Ok(new ResponseDTO
            {
                sucess = true,
                message = "Curso guardado correctamente"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDTO
            {
                sucess = false,
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
    //                sucess = false,
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
    //            sucess = true,
    //            message = "Usuario guardado correctamente"
    //        });
    //    }
    //    catch (Exception ex)
    //    {
    //        return BadRequest(new ErrorResponseDTO
    //        {
    //            sucess = false,
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
                    sucess = false,
                    message = "Datos del usuario no válidos"
                });
            }

            var Course = await _daoCourse.GetCourseById(idCourse, activeStatus);
            if (Course == null)
            {
                return NotFound(new ErrorResponseDTO
                {
                    sucess = false,
                    message = "No se encontró el usuario con el Id: " + idCourse
                });
            }
            Course.Name = CourseDTO.name;
            Course.Description = CourseDTO.description;
            Course.URL = CourseDTO.URL;
            Course.Id = CourseDTO.id;



            await _daoCourse.UpdateCourse(Course);

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
                    sucess = false,
                    message = "No se encontró el usuario con el Id: " + idCourse
                });
            }
            Course.EntityStatus = EntitiesLibrary.Common.EntityStatus.Blocked;

            await _daoCourse.UpdateCourse(Course);

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
    //                sucess = false,
    //                message = "No se encontró el usuario con el Id: " + idCourse
    //            });
    //        }
    //        Course.EntityStatus = EntitiesLibrary.Common.EntityStatus.Active;

    //        await _daoCourse.UpdateCourse(Course);

    //        return Ok(new ResponseDTO
    //        {
    //            sucess = true,
    //            message = "Curso activado correctamente"
    //        });
    //    }
    //    catch (Exception ex)
    //    {
    //        return BadRequest(new ErrorResponseDTO
    //        {
    //            sucess = false,
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
                    sucess = false,
                    message = "No se encontró el curso con el Id: " + idCourse
                });
            }

            // Activar el curso
            course.EntityStatus = EntitiesLibrary.Common.EntityStatus.Active;

            // Actualizar el curso
            await _daoCourse.UpdateCourse(course);

            return Ok(new ResponseDTO
            {
                sucess = true,
                message = "Curso activado correctamente"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponseDTO
            {
                sucess = false,
                message = "Error al activar el curso: " + ex.Message
            });
        }
    }

    [Authorize(Roles = "Administrator")]
    [HttpDelete("eliminar/{idCourse}")]
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
                    sucess = false,
                    message = "No se encontró el usuario con el Id: " + idCourse
                });
            }
            Course.EntityStatus = EntitiesLibrary.Common.EntityStatus.Deleted;

            await _daoCourse.UpdateCourse(Course);

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


   
