using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DaoLibrary.Interfaces.Course;
using Microsoft.EntityFrameworkCore;

namespace DaoLibrary.EFCore.Course;
    public class DAOCourse : IDAOCourse
    {
        private readonly MyDbContext _context;

        public DAOCourse (MyDbContext context)  // Cambiado de DbContext a MyDbContext
        {
            _context = context;
        }


        public async Task<(List<EntitiesLibrary.Course.Course> Courses, int TotalCount)> GetCoursesPaged
        (int pageNumber, int pageSize, EntitiesLibrary.Common.EntityStatus? entityStatus)
        {
            var query = _context.Set<EntitiesLibrary.Course.Course>().AsQueryable();


            if (entityStatus.HasValue)
            {
                query = query.Where(Course => Course.EntityStatus == entityStatus.Value);
            }

            var totalCount = await query.CountAsync();

            var Courses = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (Courses, totalCount);
        }


        public async Task<List<EntitiesLibrary.Course.Course>> GetAllCourses()
        {
            return await _context.Set<EntitiesLibrary.Course.Course>().ToListAsync();
        }

        public async Task<EntitiesLibrary.Course.Course?> GetCourseById(int id)
        {
            return await _context.Set<EntitiesLibrary.Course.Course>().FindAsync(id);
        }

         public async Task<EntitiesLibrary.Course.Course?> GetCourseById
        (int id, EntitiesLibrary.Common.EntityStatus? entityStatus)
        {
            return await _context.Set<EntitiesLibrary.Course.Course>()
                .FirstOrDefaultAsync(Course => Course.Id == id && Course.EntityStatus == entityStatus);
        }


        public async Task AddCourse(EntitiesLibrary.Course.Course Course)
        {
            await _context.Set<EntitiesLibrary.Course.Course>().AddAsync(Course);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCourse(EntitiesLibrary.Course.Course Course)
        {
            _context.Set<EntitiesLibrary.Course.Course>().Update(Course);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCourse(int id)
        {
            var Course = await _context.Set<EntitiesLibrary.Course.Course>().FindAsync(id);
            if (Course != null)
            {
                _context.Set<EntitiesLibrary.Course.Course>().Remove(Course);
                await _context.SaveChangesAsync();
            }
        }
    }
