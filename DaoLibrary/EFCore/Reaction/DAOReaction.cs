using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DaoLibrary.Interfaces.Reaction;
using Microsoft.EntityFrameworkCore;


namespace DaoLibrary.EFCore.Reaction;
public class DAOReaction : IDAOReaction
{
    private readonly MyDbContext _context;

    public DAOReaction(MyDbContext context)
    {
        _context = context;
    }

    public async Task<(List<EntitiesLibrary.Reaction.Reaction> Reactions, int TotalCount)> GetReactionsPaged
    (int pageNumber, int pageSize, EntitiesLibrary.Common.EntityStatus? entityStatus)
    {
        var query = _context.Set<EntitiesLibrary.Reaction.Reaction>().AsQueryable();


        if (entityStatus.HasValue)
        {
            query = query.Where(reaction => reaction.EntityStatus == entityStatus.Value);
        }

        var totalCount = await query.CountAsync();

        var reactions = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (reactions, totalCount);
    }


    public async Task<List<dynamic>> GetAllReactionsByIdPost(int idPost)
    {
        var reactions = await _context.Set<EntitiesLibrary.Reaction.Reaction>()
            .Where(r => r.Post.Id == idPost) // Filtrar por el ID del post
            .GroupBy(r => r.ReactionStatus)  // Agrupar por el estado de reacción
            .Select(group => new
            {
                ReactionStatus = group.Key,  // Estado de la reacción
                Count = group.Count()        // Cantidad de reacciones por estado
            })
            .ToListAsync();

        return reactions.Cast<dynamic>().ToList(); // Retorna la lista como dinámica o ajusta al tipo deseado
    }


    public async Task<EntitiesLibrary.Reaction.Reaction?> GetReactionById(int id)
    {
        return await _context.Set<EntitiesLibrary.Reaction.Reaction>().FindAsync(id);
    }

    public async Task<EntitiesLibrary.Reaction.Reaction?> GetReactionById
   (int id, EntitiesLibrary.Common.EntityStatus? entityStatus)
    {
        return await _context.Set<EntitiesLibrary.Reaction.Reaction>()
            .FirstOrDefaultAsync(reaction => reaction.Id == id && reaction.EntityStatus == entityStatus);
    }

    public async Task AddReaction(EntitiesLibrary.Reaction.Reaction reaction)
    {
        await _context.Set<EntitiesLibrary.Reaction.Reaction>().AddAsync(reaction);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateReaction(EntitiesLibrary.Reaction.Reaction reaction)
    {
        _context.Set<EntitiesLibrary.Reaction.Reaction>().Update(reaction);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteReaction(int id)
    {
        var reaction = await _context.Set<EntitiesLibrary.Reaction.Reaction>().FindAsync(id);
        if (reaction != null)
        {
            _context.Set<EntitiesLibrary.Reaction.Reaction>().Remove(reaction);
            await _context.SaveChangesAsync();
        }
    }
}

