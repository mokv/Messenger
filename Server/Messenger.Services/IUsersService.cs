using Messenger.Data;
using System.Collections.Generic;

namespace Messenger.Services
{
    public interface IUsersService
    {
        User GetUserById(int id);

        User GetUserByUsername(string username);

        IEnumerable<User> GetAllUsers();

        IEnumerable<Chatroom> GetAllChatroomsForUser(int userID);

        void AddUser(User user);
    }
}
