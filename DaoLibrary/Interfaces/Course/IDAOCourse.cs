

using DaoLibrary.Migrations;
using EntitiesLibrary.Course;
namespace DaoLibrary.Interfaces.Course;
public interface IDAOCourse
{
    Task<(List<EntitiesLibrary.Course.Course> Courses, int TotalCount)> GetCoursesPaged
    (int pageNumber, int pageSize, EntitiesLibrary.Common.EntityStatus? entityStatus);

    Task<(List<EntitiesLibrary.Course.Course> Courses, int TotalCount)> GetCoursesByLevel(LevelCourse level, int page, int pageSize);

    Task<EntitiesLibrary.Course.Course?> GetCourseById(int id, EntitiesLibrary.Common.EntityStatus? entityStatus);
    Task AddCourse(EntitiesLibrary.Course.Course course);

    Task UpdateCourse(EntitiesLibrary.Course.Course course);

    Task DeleteCourse(int idcourse);


}

