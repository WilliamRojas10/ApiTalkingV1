using Microsoft.AspNetCore.Mvc;
using DaoLibrary.Interfaces.Course;



namespace ApiTalking.Controllers
{
    [ApiController]
    [Route("api/courses")]
    public class CourseController : ControllerBase
    {
        private readonly IDAOCourse _courseDAO;

        public CourseController(IDAOCourse courseDAO)
        {
            _courseDAO = courseDAO;
        }

        [HttpGet]
        public async Task<ActionResult<List<ResponseCourseDTO>>> GetAll(int page = 1, int size = 10)
        {
            var (courses, totalCount) = await _courseDAO.GetCoursesPaged(page, size, null);

            var courseDTOs = courses.Select(static c => new ResponseCourseDTO{});
           

            return Ok(courseDTOs);
        }
    }

    public class ResponseCourseDTO
    {
    }
}
