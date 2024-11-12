using System.Collections.Generic;
using System.Threading.Tasks;
using EntitiesLibrary.Post;

namespace DaoLibrary.Interfaces.Post
{
    public interface IDAOPost
    {
        Task<(List<EntitiesLibrary.Post.Post> posts, int TotalCount)> GetPostsPaged (int pageNumber, int pageSize, EntitiesLibrary.Post.PostStatus? postStatus);
        Task<List<EntitiesLibrary.Post.Post>> GetAllPosts();
        Task<EntitiesLibrary.Post.Post?> GetPostById(int id);
        Task<EntitiesLibrary.Post.Post?> GetPostById(int id, EntitiesLibrary.Post.PostStatus? postStatus);
        Task AddPost(EntitiesLibrary.Post.Post post);
        Task UpdatePost(EntitiesLibrary.Post.Post post);
        Task DeletePost(int id);
    }
}
