
using DaoLibrary.Interfaces.User;
using DaoLibrary.Interfaces.Post;
using DaoLibrary.Interfaces.Comment;
using DaoLibrary.Interfaces.Reaction;
using DaoLibrary.Interfaces.Course;
using DaoLibrary.Interfaces.File;

namespace DaoLibrary.Interfaces;

public interface IDAOFactory
{
    IDAOUser CreateDAOUser();
    IDAOPost CreateDAOPost();
    IDAOComment CreateDAOComment();
    IDAOReaction CreateDAOReaction();
    IDAOCourse CreateDAOCourse();
    IDAOFile CreateDAOFile();
}