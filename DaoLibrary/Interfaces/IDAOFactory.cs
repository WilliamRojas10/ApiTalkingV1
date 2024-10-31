// using dao_library.Interfaces.login;
// using dao_library.Interfaces.post;
using DaoLibrary.Interfaces.User;
using DaoLibrary.Interfaces.Post;

namespace DaoLibrary.Interfaces;

public interface IDAOFactory
{
    IDAOUser CreateDAOUser();
    // IDAOPerson CreateDAOPerson();
    // IDAOUserBan CreateDAOUserBan();
    IDAOPost CreateDAOPost();
}