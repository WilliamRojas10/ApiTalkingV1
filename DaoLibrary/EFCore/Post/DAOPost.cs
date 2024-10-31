using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DaoLibrary.Interfaces.Post;
using Microsoft.EntityFrameworkCore;

namespace DaoLibrary.EFCore.Post
{
    public class DAOPost : IDAOPost
    {
        private readonly DbContext _context;

        public DAOPost(DbContext context)
        {
            _context = context;
        }


        public async Task<List<EntitiesLibrary.Post.Post>> GetPostsPaged(int pageNumber, int pageSize, bool? isActive)
        {
            var query = _context.Set<EntitiesLibrary.Post.Post>().AsQueryable();

            // Filtrar según el estado si está presente
            if (isActive.HasValue)
            {
                query = query.Where(user => user.PostStatus == (isActive.Value 
                                ? EntitiesLibrary.Post.PostStatus.Active 
                                : EntitiesLibrary.Post.PostStatus.Deleted));
            }

            // Aplicar la paginación
            return await query
                .Skip((pageNumber - 1) * pageSize)  
                .Take(pageSize)                      
                .ToListAsync()
                ;
        }

        public async Task<List<EntitiesLibrary.Post.Post>> GetAllPosts()
        {
            return await _context.Set<EntitiesLibrary.Post.Post>()
                .ToListAsync()
                ;
        }

        public async Task<EntitiesLibrary.Post.Post?> GetPostById(int id)
        {
            return await _context.Set<EntitiesLibrary.Post.Post>()
                .FindAsync(id)
                ;
        }

        public async Task AddPost(EntitiesLibrary.Post.Post Post)
        {
            await _context.Set<EntitiesLibrary.Post.Post>().AddAsync(Post);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePost(EntitiesLibrary.Post.Post Post)
        {
            _context.Set<EntitiesLibrary.Post.Post>().Update(Post);
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
