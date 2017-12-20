using Messenger.Data;
using System.Collections.Generic;

namespace Messenger.Services
{
    public interface IUsersService
    {
        IEnumerable<User> GetAllUsers();

        IEnumerable<Chatroom> GetAllChatroomsForUser(int userID);

        void AddUser(User user);
    }
}
