using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DaoLibrary.Interfaces.Post;
using Microsoft.EntityFrameworkCore;

namespace DaoLibrary.EFCore.Post
{
    public class DAOPost : IDAOPost
    {
        private readonly MyDbContext _context;

        public DAOPost(MyDbContext context) 
        {
            _context = context;
        }

        public async Task<(List<EntitiesLibrary.Post.Post> posts, int TotalCount)> GetPostsPaged
        (int pageNumber, int pageSize, EntitiesLibrary.Post.PostStatus? postStatus)
        {
            var query = _context.Set<EntitiesLibrary.Post.Post>().AsQueryable();


            if (postStatus.HasValue)
            {
                query = query.Where(post => post.PostStatus == postStatus.Value);
            }

            var totalCount = await query.CountAsync();

            var posts = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (posts, totalCount);
        }


        public async Task<List<EntitiesLibrary.Post.Post>> GetAllPosts()
        {
            return await _context.Set<EntitiesLibrary.Post.Post>().ToListAsync();
        }

        public async Task<EntitiesLibrary.Post.Post?> GetPostById(int id)
        {
            return await _context.Set<EntitiesLibrary.Post.Post>().FindAsync(id);
        }

         public async Task<EntitiesLibrary.Post.Post?> GetPostById
        (int id, EntitiesLibrary.Post.PostStatus? postStatus)
        {
            return await _context.Set<EntitiesLibrary.Post.Post>()
                .FirstOrDefaultAsync(post => post.Id == id && post.PostStatus == postStatus);
        }

        public async Task AddPost(EntitiesLibrary.Post.Post post)
        {
            await _context.Set<EntitiesLibrary.Post.Post>().AddAsync(post);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePost(EntitiesLibrary.Post.Post post)
        {
            _context.Set<EntitiesLibrary.Post.Post>().Update(post);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePost(int id)
        {
            var post = await _context.Set<EntitiesLibrary.Post.Post>().FindAsync(id);
            if (post != null)
            {
                _context.Set<EntitiesLibrary.Post.Post>().Remove(post);
                await _context.SaveChangesAsync();
            }
        }
    }
}
