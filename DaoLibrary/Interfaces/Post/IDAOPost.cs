using System.Collections.Generic;
using System.Threading.Tasks;
using EntitiesLibrary.Post;

namespace DaoLibrary.Interfaces.Post;
    public interface IDAOPost
    {
        Task<(List<EntitiesLibrary.Post.Post> posts, int TotalCount)> GetPostsPaged
        (int pageNumber, int pageSize, EntitiesLibrary.Common.EntityStatus? entityStatus, string orden);
        Task<List<EntitiesLibrary.Post.Post>> GetAllPosts();
        Task<EntitiesLibrary.Post.Post?> GetPostById(int id);
        Task<EntitiesLibrary.Post.Post?> GetPostById(int id, EntitiesLibrary.Common.EntityStatus? entityStatus);
        Task AddPost(EntitiesLibrary.Post.Post post);
        Task UpdatePost(EntitiesLibrary.Post.Post post);
        Task DeletePost(int id);
    }
