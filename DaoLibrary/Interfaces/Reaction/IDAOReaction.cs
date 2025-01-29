

namespace DaoLibrary.Interfaces.Reaction;
public interface IDAOReaction
{
    Task<(List<EntitiesLibrary.Reaction.Reaction> Reactions, int TotalCount)> GetReactionsPaged(int pageNumber, int pageSize, EntitiesLibrary.Common.EntityStatus? entityStatus);
    Task<List<EntitiesLibrary.Reaction.Reaction>> GetAllReactions();
    Task<EntitiesLibrary.Reaction.Reaction?> GetReactionById(int id);
    Task<EntitiesLibrary.Reaction.Reaction?> GetReactionById(int id, EntitiesLibrary.Common.EntityStatus? entityStatus);
    Task AddReaction(EntitiesLibrary.Reaction.Reaction reaction);
    Task UpdateReaction(EntitiesLibrary.Reaction.Reaction reaction);
    Task DeleteReaction(int id);
}

