using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DaoLibrary.Interfaces.Comment;
using Microsoft.EntityFrameworkCore;

namespace DaoLibrary.EFCore.Comment;
public class DAOComment : IDAOComment
{
    private readonly MyDbContext _context;

    public DAOComment(MyDbContext context) 
    {
        _context = context;
    }


    public async Task<(List<EntitiesLibrary.Comment.Comment> Comments, int TotalCount)> GetCommentsPaged
    (int pageNumber, int pageSize, EntitiesLibrary.Common.EntityStatus? entityStatus)
    {
        var query = _context.Set<EntitiesLibrary.Comment.Comment>().AsQueryable();


        if (entityStatus.HasValue)
        {
            query = query.Where(comment => comment.EntityStatus == entityStatus.Value);
        }

        var totalCount = await query.CountAsync();

        var comments = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (comments, totalCount);
    }


    public async Task<List<EntitiesLibrary.Comment.Comment>> GetAllComments()
    {
        return await _context.Set<EntitiesLibrary.Comment.Comment>().ToListAsync();
    }

    public async Task<EntitiesLibrary.Comment.Comment?> GetCommentById(int id)
    {   
        return await _context.Set<EntitiesLibrary.Comment.Comment>().FindAsync(id);
    }

    public async Task<EntitiesLibrary.Comment.Comment?> GetCommentById
   (int id, EntitiesLibrary.Common.EntityStatus? entityStatus)
    {
        return await _context.Set<EntitiesLibrary.Comment.Comment>()
            .FirstOrDefaultAsync(comment => comment.Id == id && comment.EntityStatus == entityStatus);
    }

    public async Task AddComment(EntitiesLibrary.Comment.Comment comment)
    {
        await _context.Set<EntitiesLibrary.Comment.Comment>().AddAsync(comment);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateComment(EntitiesLibrary.Comment.Comment comment)
    {
        _context.Set<EntitiesLibrary.Comment.Comment>().Update(comment);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteComment(int id)
    {
        var comment = await _context.Set<EntitiesLibrary.Comment.Comment>().FindAsync(id);
        if (comment != null)
        {
            _context.Set<EntitiesLibrary.Comment.Comment>().Remove(comment);
            await _context.SaveChangesAsync();
        }
    }
}
