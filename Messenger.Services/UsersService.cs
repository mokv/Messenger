using Messenger.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Messenger.Services
{
    public class UsersService : BaseService, IUsersService
    {
        public IEnumerable<User> GetAllUsers()
        {
            List<User> chatrooms = context.Users.ToList();
            return chatrooms;
        }

        public IEnumerable<Chatroom> GetAllChatroomsForUser(int userID)
        {
            User user = context.Users.FirstOrDefault(u => u.UserID == userID);
            List<Chatroom> chatrooms = user.Chatrooms.ToList();
            return chatrooms;
        }

        public void AddUser(User user)
        {
            bool usernameExists = CheckUsernameExistence(user.Username);

            if (usernameExists)
            {
                throw new ArgumentException("Username already exists.");
            }

            context.Users.Add(user);
            context.SaveChanges();
        }


        private bool CheckUsernameExistence(string username)
        {
            foreach (User user in context.Users)
            {
                if (username == user.Username)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
