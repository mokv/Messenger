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
            using (context)
            {
                List<Chatroom> chatrooms = context.Chatrooms.ToList();
                return chatrooms;
            }
        }

        public Chatroom GetChatroomByName(string name)
        {
            using (context)
            {
                Chatroom chatroom = context.Chatrooms.FirstOrDefault(cr => cr.Name.Contains(name));

                if (chatroom == null)
                {
                    throw new ArgumentNullException("Chatroom doesn't exist.");
                }

                return chatroom;
            }
        }

        public IEnumerable<Message> GetAllMessagesInChatroom(int chatroomID)
        {
            using (context)
            {
                Chatroom chatroom = GetChatroom(chatroomID);
                List<Message> messages = chatroom.Messages.ToList();
                return messages;
            }
        }

        public IEnumerable<Message> GetAllMessagesInChatroomSinceDate(int chatroomID, DateTime date)
        {
            using (context)
            {
                List<Message> messages = new List<Message>();
                Chatroom chatroom = GetChatroom(chatroomID);
                messages = chatroom.Messages.Where(m => (m.SentDate.CompareTo(date) >= 0)).ToList();
                return messages;
            }
        }

        public void AddMessageInChatroom(Message message)
        {
            using (context)
            {
                if (!CheckIfUserIsMember(message.ChatroomID, message.SenderID))
                {
                    throw new InvalidOperationException("User is not a member of this chatroom.");
                }

                context.Messages.Add(message);
                context.SaveChanges();
            }
        }

        public void AddChatroom(Chatroom chatroom)
        {
            using (context)
            {
                context.Chatrooms.Add(chatroom);
                context.SaveChanges();
            }
        }

        public void AddUserToChatroom(int chatroomId, int userId)
        {
            using (context)
            {
                if (CheckIfUserIsMember(chatroomId, userId))
                {
                    throw new ArgumentException("User is already a member of the chatroom.");
                }

                ChatroomDetail crDetail = new ChatroomDetail() { ChatroomID = chatroomId, UserID = userId };
                context.ChatroomDetails.Add(crDetail);
                context.SaveChanges();
            }
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

        private bool CheckIfUserIsMember(int chatroomId, int userId)
        {
            return context.ChatroomDetails.Where(cd => cd.ChatroomID == chatroomId)
                                          .Any(cd => cd.UserID == userId);
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
