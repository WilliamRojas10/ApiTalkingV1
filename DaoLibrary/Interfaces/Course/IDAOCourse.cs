

using DaoLibrary.Migrations;
using EntitiesLibrary.Course;

namespace DaoLibrary.Interfaces.Course
{
    public interface IDAOCourse
    {
        Task<(List<EntitiesLibrary.Course.Course> Courses, int TotalCount)> GetCoursesPaged
        (int pageNumber, int pageSize, EntitiesLibrary.Common.EntityStatus? entityStatus);

        Task<List<EntitiesLibrary.Course.Course>> GetCoursesByLevel(LevelCourse level, int page, int pageSize);


        //Task<List<EntitiesLibrary.Course.Course>> GetCoursesByLevel(LevelCourse level, int page, int pageSize);

        // 🔹 Agregar el método GetCourseById
        Task<EntitiesLibrary.Course.Course?> GetCourseById(int id, EntitiesLibrary.Common.EntityStatus? entityStatus);

        // 🔹 Agregar el método AddCourse
        Task AddCourse(EntitiesLibrary.Course.Course course);  // Método para agregar un curso

        Task UpdateCourse(EntitiesLibrary.Course.Course course);  // Método para modificar un curso

        // 🔹 Agregar el método DeleteCourse
        Task DeleteCourse(int idcourse);  // Método para eliminar un curso
        
       

    }

}

