using System.Collections.Generic;
using System.Threading.Tasks;

namespace DaoLibrary.Interfaces.Post
{
    public interface IDAOPost
    {
        Task<List<EntitiesLibrary.Post.Post>> GetPostsPaged(int pageNumber, int pageSize, bool? isActive);
        Task<List<EntitiesLibrary.Post.Post>> GetAllPosts();
        Task<EntitiesLibrary.Post.Post?> GetPostById(int id);
        Task AddPost(EntitiesLibrary.Post.Post post);
        Task UpdatePost(EntitiesLibrary.Post.Post post);
        Task DeletePost(int id);
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