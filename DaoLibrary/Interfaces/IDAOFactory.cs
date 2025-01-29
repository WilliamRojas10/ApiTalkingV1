
using DaoLibrary.Interfaces.User;
using DaoLibrary.Interfaces.Post;
using DaoLibrary.Interfaces.Comment;
using DaoLibrary.Interfaces.Reaction;

namespace DaoLibrary.Interfaces;

public interface IDAOFactory
{
    IDAOUser CreateDAOUser();
    IDAOPost CreateDAOPost();
    IDAOComment CreateDAOComment();
    IDAOReaction CreateDAOReaction();
}