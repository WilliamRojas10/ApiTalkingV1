using System.Collections.Generic;

namespace DaoLibrary.Interfaces.User
{
    public interface IDAOUser
    {
        IEnumerable<EntitiesLibrary.User.User> GetUsersPaged(int pageNumber, int pageSize, bool? isActive);
        IEnumerable<EntitiesLibrary.User.User> GetAllUsers();
        EntitiesLibrary.User.User? GetUserById(int id);
        void AddUser(EntitiesLibrary.User.User user);
        void UpdateUser(EntitiesLibrary.User.User user);
        void DeleteUser(int id);
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