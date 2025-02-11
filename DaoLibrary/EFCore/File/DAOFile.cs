using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DaoLibrary.Interfaces.File;
using Microsoft.EntityFrameworkCore;

namespace DaoLibrary.EFCore.File
{
    public class DAOFile : IDAOFile
    {
        private readonly MyDbContext _context;

        public DAOFile(MyDbContext context)
        {
            _context = context;
        }

        public async Task<(List<EntitiesLibrary.File.File> files, int TotalCount)> GetFilesPaged
        (int pageNumber, int pageSize, EntitiesLibrary.Common.EntityStatus? entityStatus)
        {
            var query = _context.Set<EntitiesLibrary.File.File>().AsQueryable();


            if (entityStatus.HasValue)
            {
                query = query.Where(file => file.EntityStatus == entityStatus.Value);
            }

            var totalCount = await query.CountAsync();

            var files = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (files, totalCount);
        }

        public async Task<List<EntitiesLibrary.File.File>> GetAllFiles()
        {
            return await _context.Set<EntitiesLibrary.File.File>().ToListAsync();
        }

        public async Task<EntitiesLibrary.File.File?> GetFileById(int id)
        {
            return await _context.Set<EntitiesLibrary.File.File>().FindAsync(id);
        }

        public async Task<EntitiesLibrary.File.File?> GetFileById
       (int id, EntitiesLibrary.Common.EntityStatus? entityStatus)
        {
            return await _context.Set<EntitiesLibrary.File.File>()
                .FirstOrDefaultAsync(file => file.Id == id && file.EntityStatus == entityStatus);
        }

        public async Task<EntitiesLibrary.File.File> AddFile (EntitiesLibrary.File.File file)
        {
            await _context.Set<EntitiesLibrary.File.File>().AddAsync(file);
            await _context.SaveChangesAsync();
            return file; 
        }


        public async Task UpdateFile(EntitiesLibrary.File.File file)
        {
            _context.Set<EntitiesLibrary.File.File>().Update(file);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFile(int id)
        {
            var file = await _context.Set<EntitiesLibrary.File.File>().FindAsync(id);
            if (file != null)
            {
                _context.Set<EntitiesLibrary.File.File>().Remove(file);
                await _context.SaveChangesAsync();
            }
        }
    }
}
