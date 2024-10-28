using System.Collections.Generic;
using System.Linq;
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

        public IEnumerable<EntitiesLibrary.User.User> GetUsersPaged(int pageNumber, int pageSize, bool? isActive)
        {
            var query = _context.Set<EntitiesLibrary.User.User>().AsQueryable();

            // Filtrar según el estado
            if (isActive.HasValue)
            {
                query = query.Where(user => user.UserStatus == EntitiesLibrary.User.UserStatus.Active);
            }

            // Aplicar la paginación
            return query
                .Skip((pageNumber - 1) * pageSize)  // Saltar los elementos de las páginas anteriores
                .Take(pageSize)                      // Tomar solo el número especificado de elementos
                .ToList();
        }

        public IEnumerable<EntitiesLibrary.User.User> GetAllUsers()
        {
            return _context.Set<EntitiesLibrary.User.User>().ToList();
        }

        public EntitiesLibrary.User.User? GetUserById(int id)
        {
            return _context.Set<EntitiesLibrary.User.User>().Find(id);
        }

        public void AddUser(EntitiesLibrary.User.User user)
        {
            _context.Set<EntitiesLibrary.User.User>().Add(user);
            _context.SaveChanges();
        }

        public void UpdateUser(EntitiesLibrary.User.User user)
        {
            _context.Set<EntitiesLibrary.User.User>().Update(user);
            _context.SaveChanges();
        }

        public void DeleteUser(int id)
        {
            var user = _context.Set<EntitiesLibrary.User.User>().Find(id);
            if (user != null)
            {
                _context.Set<EntitiesLibrary.User.User>().Remove(user);
                _context.SaveChanges();
            }
        }
    }
}
