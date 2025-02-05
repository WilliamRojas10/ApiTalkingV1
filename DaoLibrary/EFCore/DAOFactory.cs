using DaoLibrary;
using DaoLibrary.Interfaces;
using DaoLibrary.Interfaces.User;
using DaoLibrary.EFCore.User;

using DaoLibrary.Interfaces.Post;
using DaoLibrary.EFCore.Post;

using DaoLibrary.Interfaces.Reaction;
using DaoLibrary.EFCore.Reaction;

using DaoLibrary.Interfaces.Comment;
using DaoLibrary.EFCore.Comment;

using DaoLibrary.Interfaces.Course;
using DaoLibrary.EFCore.Course;

namespace DaoLibrary.EFCore;

public class DAOFactory : IDAOFactory
{
    private readonly MyDbContext context;

    public DAOFactory(MyDbContext context)
    {
        this.context = context;
    }

    // public IDAOPerson CreateDAOPerson()
    // {
    //     return new DAOEFPerson(context);
    // }

     public IDAOPost CreateDAOPost()
     {
         return new DAOPost(context);
     }

    public IDAOUser CreateDAOUser()
    {
        return new DAOUser(context);
    }

    public IDAOReaction CreateDAOReaction()
    {
        return new DAOReaction(context);
    }

    public IDAOComment CreateDAOComment()
    {
        return new DAOComment(context);
    }
     public IDAOCourse CreateDAOCourse()
    {
        return new DAOCourse(context);
    }
}