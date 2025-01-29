using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DaoLibrary.Interfaces.User;
using Microsoft.EntityFrameworkCore;

namespace DaoLibrary.EFCore.User;
    public class DAOUser : IDAOUser
    {
        private readonly MyDbContext _context;

        public DAOUser(MyDbContext context)  // Cambiado de DbContext a MyDbContext
        {
            _context = context;
        }


        public async Task<(List<EntitiesLibrary.User.User> Users, int TotalCount)> GetUsersPaged
        (int pageNumber, int pageSize, EntitiesLibrary.Common.EntityStatus? entityStatus)
        {
            var query = _context.Set<EntitiesLibrary.User.User>().AsQueryable();


            if (entityStatus.HasValue)
            {
                query = query.Where(user => user.EntityStatus == entityStatus.Value);
            }

            var totalCount = await query.CountAsync();

            var users = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (users, totalCount);
        }


        public async Task<List<EntitiesLibrary.User.User>> GetAllUsers()
        {
            return await _context.Set<EntitiesLibrary.User.User>().ToListAsync();
        }

        public async Task<EntitiesLibrary.User.User?> GetUserById(int id)
        {
            return await _context.Set<EntitiesLibrary.User.User>().FindAsync(id);
        }

         public async Task<EntitiesLibrary.User.User?> GetUserById
        (int id, EntitiesLibrary.Common.EntityStatus? entityStatus)
        {
            return await _context.Set<EntitiesLibrary.User.User>()
                .FirstOrDefaultAsync(user => user.Id == id && user.EntityStatus == entityStatus);
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
