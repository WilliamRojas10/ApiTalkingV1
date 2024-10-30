using System.Collections.Generic;
using System.Threading.Tasks;

namespace DaoLibrary.Interfaces.User
{
    public interface IDAOUser
    {
        Task<List<EntitiesLibrary.User.User>> GetUsersPaged(int pageNumber, int pageSize, bool? isActive);
        Task<List<EntitiesLibrary.User.User>> GetAllUsers();
        Task<EntitiesLibrary.User.User?> GetUserById(int id);
        Task AddUser(EntitiesLibrary.User.User user);
        Task UpdateUser(EntitiesLibrary.User.User user);
        Task DeleteUser(int id);
    }
}



// using EntitiesLibrary.User;

// namespace DaoLibrary.Interfaces;

// public interface IDAOUser
// {
//     Task<IEnumerable<User>> GetAll();
//     Task<User> GetById(long id);
//     Task Save(User person);
//     Task Delete(User person);
// }