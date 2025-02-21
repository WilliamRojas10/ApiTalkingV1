
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DaoLibrary.Interfaces.Course;
using EntitiesLibrary.Course;
using Microsoft.EntityFrameworkCore;
using EntitiesLibrary.Common;




namespace DaoLibrary.EFCore.Course
{
    public class DAOCourse : IDAOCourse
    {
        private readonly MyDbContext _context;

        public DAOCourse(MyDbContext context)
        {
            _context = context;
        }

        public async Task<(List<EntitiesLibrary.Course.Course> Courses, int TotalCount)> GetCoursesPaged(int pageNumber, int pageSize, EntityStatus? entityStatus)
        {
            var query = _context.Set<EntitiesLibrary.Course.Course>().AsQueryable();

            if (entityStatus.HasValue)
            {
                query = query.Where(course => course.EntityStatus == entityStatus.Value);
            }

            var totalCount = await query.CountAsync();

            var courses = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (courses, totalCount);
        }

        public async Task<List<EntitiesLibrary.Course.Course>> GetAllCourses()
        {
            return await _context.Set<EntitiesLibrary.Course.Course>().ToListAsync();
        }

        public async Task<EntitiesLibrary.Course.Course?> GetCourseById(int id, EntityStatus? entityStatus)
        {
            return await _context.Set<EntitiesLibrary.Course.Course>()
                .Include(c => c.User) // Cargar la relación con User
                .FirstOrDefaultAsync(course => course.Id == id && course.EntityStatus == entityStatus);
        }

        public async Task AddCourse(EntitiesLibrary.Course.Course course)
        {
            if (course.User == null)
            {
                throw new ArgumentException("El curso debe estar asociado a un usuario.");
            }

            await _context.Set<EntitiesLibrary.Course.Course>().AddAsync(course);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCourse(EntitiesLibrary.Course.Course course)
        {
            _context.Set<EntitiesLibrary.Course.Course>().Update(course);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCourse(int id)
        {
            var course = await _context.Set<EntitiesLibrary.Course.Course>().FindAsync(id);
            if (course != null)
            {
                _context.Set<EntitiesLibrary.Course.Course>().Remove(course);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<EntitiesLibrary.Course.Course>> GetCoursesByLevel(LevelCourse level, int page, int pageSize)
        {
            var query = _context.Set<EntitiesLibrary.Course.Course>()
                .Where(course => course.Level == level); // Comparación correcta de enums

            // Aplicar la paginación
            var courses = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return courses; // Solo devolvemos la lista de cursos
        }

    }
}


//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using DaoLibrary.Interfaces.Course;
//using EntitiesLibrary.Course;
//using Microsoft.EntityFrameworkCore;

//namespace DaoLibrary.EFCore.Course;
//public class DAOCourse : IDAOCourse
//{
//    private readonly MyDbContext _context;

//    public DAOCourse(MyDbContext context)  // Cambiado de DbContext a MyDbContext
//    {
//        _context = context;
//    }


//    public async Task<(List<EntitiesLibrary.Course.Course> Courses, int TotalCount)> GetCoursesPaged
//    (int pageNumber, int pageSize, EntitiesLibrary.Common.EntityStatus? entityStatus)
//    {
//        var query = _context.Set<EntitiesLibrary.Course.Course>().AsQueryable();


//        if (entityStatus.HasValue)
//        {
//            query = query.Where(Course => Course.EntityStatus == entityStatus.Value);
//        }

//        var totalCount = await query.CountAsync();

//        var Courses = await query
//            .Skip((pageNumber - 1) * pageSize)
//            .Take(pageSize)
//            .ToListAsync();

//        return (Courses, totalCount);
//    }


//    public async Task<List<EntitiesLibrary.Course.Course>> GetAllCourses()
//    {
//        return await _context.Set<EntitiesLibrary.Course.Course>().ToListAsync();
//    }


//    public async Task<EntitiesLibrary.Course.Course?> GetCourseById
//   (int id, EntitiesLibrary.Common.EntityStatus? entityStatus)
//    {
//        return await _context.Set<EntitiesLibrary.Course.Course>()
//            .Include(c => c.User) // 📌 Cargar la relación con User
//            .FirstOrDefaultAsync(Course => Course.Id == id && Course.EntityStatus == entityStatus);
//    }


//    public async Task AddCourse(EntitiesLibrary.Course.Course Course)
//    {
//        if (Course.User == null)
//        {
//            throw new ArgumentException("El curso debe estar asociado a un usuario.");
//        }

//        await _context.Set<EntitiesLibrary.Course.Course>().AddAsync(Course);
//        await _context.SaveChangesAsync();
//    }

//    public async Task UpdateCourse(EntitiesLibrary.Course.Course Course)
//    {
//        _context.Set<EntitiesLibrary.Course.Course>().Update(Course);
//        await _context.SaveChangesAsync();
//    }

//    public async Task DeleteCourse(int id)
//    {
//        var Course = await _context.Set<EntitiesLibrary.Course.Course>().FindAsync(id);
//        if (Course != null)
//        {
//            _context.Set<EntitiesLibrary.Course.Course>().Remove(Course);
//            await _context.SaveChangesAsync();
//        }
//    }
//    public async Task<List<EntitiesLibrary.Course.Course>> GetCoursesByLevel(string level)
//    {
//        // comparar la propiedad Level con el enum correcto
//        if (Enum.TryParse(level, true, out LevelCourse levelEnum))
//        {
//            return await _context.Set<EntitiesLibrary.Course.Course>()
//                                 .Where(c => c.Level == levelEnum)  // Usar LevelEnum en lugar de string
//                                 .ToListAsync();
//        }

//        return new List<EntitiesLibrary.Course.Course>();
//    }

//}



