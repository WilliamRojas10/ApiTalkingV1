using System.Collections.Generic;
using System.Threading.Tasks;
using EntitiesLibrary.User;

namespace DaoLibrary.Interfaces.User
{
    public interface IDAOUser
    {
        Task<(List<EntitiesLibrary.User.User> Users, int TotalCount)> GetUsersPaged (int pageNumber, int pageSize, EntitiesLibrary.User.UserStatus? userStatus);
        Task<List<EntitiesLibrary.User.User>> GetAllUsers();
        Task<EntitiesLibrary.User.User?> GetUserById(int id);
        Task<EntitiesLibrary.User.User?> GetUserById(int id, EntitiesLibrary.User.UserStatus? userStatus);
        Task AddUser(EntitiesLibrary.User.User user);
        Task UpdateUser(EntitiesLibrary.User.User user);
        Task DeleteUser(int id);
    }
}
