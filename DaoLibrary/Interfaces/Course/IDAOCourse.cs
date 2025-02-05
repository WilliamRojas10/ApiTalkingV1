

namespace DaoLibrary.Interfaces.Course
{
    public interface IDAOCourse
    {
      Task<(List<EntitiesLibrary.Course.Course> Courses, int TotalCount)> GetCoursesPaged
        (int pageNumber, int pageSize, EntitiesLibrary.Common.EntityStatus? entityStatus);
    }
}
