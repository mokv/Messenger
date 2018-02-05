using Messenger.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Messenger.Services
{
    public class UsersService : BaseService, IUsersService
    {
        public User GetUserById(int id)
        {
            using (context)
            {
                User user = context.Users.FirstOrDefault(u => u.UserID == id);

                if (user == null)
                {
                    throw new ArgumentNullException("User doesn't exist.");
                }

                return user;
            }
        }

        public User GetUserByUsername(string username)
        {
            using (context)
            {
                User user = context.Users.FirstOrDefault(u => u.Username == username);

                if (user == null)
                {
                    throw new ArgumentNullException("User doesn't exist.");
                }

                return user;
            }
        }

        public IEnumerable<User> GetAllUsers()
        {
            using (context)
            {
                List<User> users = context.Users.ToList();
                return users;
            }
        }

        public IEnumerable<Chatroom> GetAllChatroomsForUser(int userID)
        {
            using (context)
            {
                var getChatroomsQuery = from chatrooms in context.Chatrooms
                                        join crDet in context.ChatroomDetails on chatrooms.ChatroomID equals crDet.ChatroomID
                                        where crDet.UserID == userID
                                        select chatrooms;
                return getChatroomsQuery.ToList();
            }
        }

        public void AddUser(User user)
        {
            using (context)
            {
                bool usernameExists = CheckUsernameExistence(user.Username);

                if (usernameExists)
                {
                    throw new ArgumentException("Username already exists.");
                }

                context.Users.Add(user);
                context.SaveChanges();
            }
        }


        private bool CheckUsernameExistence(string username)
        {
            if(context.Users.Any(u => u.Username == username))
            {
                return true;
            }

            return false;
        }
    }
}
