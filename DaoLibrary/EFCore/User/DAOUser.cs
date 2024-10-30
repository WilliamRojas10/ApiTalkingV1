using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DaoLibrary.Interfaces.User;
using Microsoft.EntityFrameworkCore;

namespace DaoLibrary.EFCore.User
{
    public class DAOUser : IDAOUser
    {
        private readonly DbContext _context;

        public DAOUser(DbContext context)
        {
            _context = context;
        }


        public async Task<List<EntitiesLibrary.User.User>> GetUsersPaged(int pageNumber, int pageSize, bool? isActive)
        {
            var query = _context.Set<EntitiesLibrary.User.User>().AsQueryable();

            // Filtrar según el estado si está presente
            if (isActive.HasValue)
            {
                query = query.Where(user => user.UserStatus == (isActive.Value 
                                ? EntitiesLibrary.User.UserStatus.Active 
                                : EntitiesLibrary.User.UserStatus.Deleted));
            }

            // Aplicar la paginación
            return await query
                .Skip((pageNumber - 1) * pageSize)  
                .Take(pageSize)                      
                .ToListAsync()
                ;
        }

        public async Task<List<EntitiesLibrary.User.User>> GetAllUsers()
        {
            return await _context.Set<EntitiesLibrary.User.User>()
                .ToListAsync()
                ;
        }

        public async Task<EntitiesLibrary.User.User?> GetUserById(int id)
        {
            return await _context.Set<EntitiesLibrary.User.User>()
                .FindAsync(id)
                ;
        }

        public async Task AddUser(EntitiesLibrary.User.User user)
        {
            await _context.Set<EntitiesLibrary.User.User>().AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUser(EntitiesLibrary.User.User user)
        {
            _context.Set<EntitiesLibrary.User.User>().Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUser(int id)
        {
            var user = await _context.Set<EntitiesLibrary.User.User>().FindAsync(id);
            if (user != null)
            {
                _context.Set<EntitiesLibrary.User.User>().Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}
