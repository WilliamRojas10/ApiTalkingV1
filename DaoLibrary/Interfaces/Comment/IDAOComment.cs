namespace DaoLibrary.Interfaces.Comment;
public interface IDAOComment
{
    Task<(List<EntitiesLibrary.Comment.Comment> Comments, int TotalCount)> GetCommentsPaged(int pageNumber, int pageSize, EntitiesLibrary.Common.EntityStatus? entityStatus);
    Task<List<EntitiesLibrary.Comment.Comment>> GetAllComments();
    Task<EntitiesLibrary.Comment.Comment?> GetCommentById(int id);
    Task<EntitiesLibrary.Comment.Comment?> GetCommentById(int id, EntitiesLibrary.Common.EntityStatus? entityStatus);
    Task AddComment(EntitiesLibrary.Comment.Comment comment);
    Task UpdateComment(EntitiesLibrary.Comment.Comment comment);
    Task DeleteComment(int id);
}

