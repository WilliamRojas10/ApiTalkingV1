using DaoLibrary;
using DaoLibrary.Interfaces;
using DaoLibrary.Interfaces.User;
using DaoLibrary.EFCore.User;
// using dao_library.Interfaces.login;
// using dao_library.entity_framework.login;
// using dao_library.Interfaces.post;
// using dao_library.entity_framework.post;

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

    // public IDAOPost CreateDAOPost()
    // {
    //     return new DAOEFPost(context);
    // }

    public IDAOUser CreateDAOUser()
    {
        return new DAOUser(context);
    }

    // public IDAOUserBan CreateDAOUserBan()
    // {
    //     return new DAOEFUserBan(context);
    // }
}