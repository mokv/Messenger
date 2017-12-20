using Messenger.Data;
using Messenger.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;

namespace Messenger.WebApi.Controllers
{
    public class MessengerController : ApiController
    {
        private IMessengerService service;

        public MessengerController()
        {
            this.service = new MessengerService();
        }

        [HttpGet]
        public IEnumerable<Chatroom> GetAllChatrooms()
        {
            return service.GetAllChatrooms();
        }

        [HttpGet]
        public IEnumerable<Message> AllMessagesInChatroom(int id)
        {
            try
            {
                return service.GetAllMessagesInChatroom(id);
            }
            catch (ArgumentNullException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [HttpGet]
        public IEnumerable<Message> AllMessagesInChatroomSinceDate(int id, DateTime date)
        {
            try
            {
                return service.GetAllMessagesInChatroomSinceDate(id, date);
            }
            catch (ArgumentNullException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [HttpPost]
        public void AddMessageInChatroom(int id, [FromBody]Message message)
        {
            message.SentDate = DateTime.Now;
            message.ChatroomID = id;
            try
            {
                service.AddMessageInChatroom(message);
            }
            catch (ArgumentNullException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            catch (InvalidOperationException)
            {
                throw new HttpResponseException(HttpStatusCode.NotAcceptable);
            }
        }

        [HttpPost]
        public void AddChatroom([FromBody]Chatroom chatroom)
        {
            service.AddChatroom(chatroom);
        }

        [HttpPut]
        public void AddUserToChatroom(int id, [FromBody]User user)
        {
            try
            {
                service.AddUserToChatroom(id, user.UserID);
            }
            catch (ArgumentException)
            {
                throw new HttpResponseException(HttpStatusCode.NotAcceptable);
            }
        }
    }
}
