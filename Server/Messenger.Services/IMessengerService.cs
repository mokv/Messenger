using Messenger.Data;
using System;
using System.Collections.Generic;

namespace Messenger.Services
{
    public interface IMessengerService
    {
        IEnumerable<Chatroom> GetAllChatrooms();

        Chatroom GetChatroomByName(string name);

        IEnumerable<Message> GetAllMessagesInChatroom(int chatroomID);

        IEnumerable<Message> GetAllMessagesInChatroomSinceDate(int chatroomID, DateTime date);

        void AddMessageInChatroom(Message message);

        void AddChatroom(Chatroom chatroom);

        void AddUserToChatroom(int chatroomID, int userID);
    }
}
