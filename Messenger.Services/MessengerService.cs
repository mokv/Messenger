using Messenger.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Messenger.Services
{
    public class MessengerService : BaseService, IMessengerService
    {
        public IEnumerable<Chatroom> GetAllChatrooms()
        {
            List<Chatroom> chatrooms = context.Chatrooms.ToList();
            return chatrooms;
        }

        public IEnumerable<Message> GetAllMessagesInChatroom(int chatroomID)
        {
            Chatroom chatroom = GetChatroom(chatroomID);
            List<Message> messages = chatroom.Messages.ToList();
            return messages;
        }

        public IEnumerable<Message> GetAllMessagesInChatroomSinceDate(int chatroomID, DateTime date)
        {
            List<Message> messages = new List<Message>();
            Chatroom chatroom = GetChatroom(chatroomID);
            messages = chatroom.Messages.Where(m => (m.SentDate.CompareTo(date) >= 0)).ToList();
            return messages;
        }

        public void AddMessageInChatroom(Message message)
        {
            Chatroom chatroom = GetChatroom(message.ChatroomID);
            bool userInChatroom = CheckIfUserIsMember(chatroom, message.SenderID);

            if (!userInChatroom)
            {
                throw new InvalidOperationException("User is not a member of this chatroom.");
            }

            context.Messages.Add(message);
            context.SaveChanges();
        }

        public void AddChatroom(Chatroom chatroom)
        {
            context.Chatrooms.Add(chatroom);
            context.SaveChanges();
        }

        public void AddUserToChatroom(int chatroomId, int userId)
        {
            Chatroom chatroom = GetChatroom(chatroomId);
            User user = GetUser(userId);

            if (chatroom.Users.Contains(user))
            {
                throw new ArgumentException("User is already a member of the chatroom.");
            }

            chatroom.Users.Add(user);
            context.SaveChanges();
        }

        private Chatroom GetChatroom(int chatroomID)
        {
            Chatroom chatroom = new Chatroom();
            chatroom = context.Chatrooms.FirstOrDefault(cr => cr.ChatroomID == chatroomID);

            if (chatroom == null)
            {
                throw new ArgumentNullException("Chatroom doesn't exist.");
            }

            return chatroom;
        }

        private bool CheckIfUserIsMember(Chatroom chatroom, int userID)
        {
            foreach (User user in chatroom.Users)
            {
                if (user.UserID == userID)
                {
                    return true;
                }
            }

            return false;
        }

        private User GetUser(int userId)
        {
            User user = context.Users.FirstOrDefault(u => u.UserID == userId);

            if (user == null)
            {
                throw new ArgumentNullException("User doesn't exist.");
            }

            return user;
        }
    }
}
