using Messenger.Data;
using Messenger.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;

namespace Messenger.WebApi.Controllers
{
    public class UsersController : ApiController
    {
        private IUsersService service;

        public UsersController()
        {
            this.service = new UsersService();
        }

        [HttpGet]
        public User GetUserById(int id)
        {
            try
            {
                return service.GetUserById(id);
            }
            catch (ArgumentNullException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [HttpGet]
        public User GetUserByUsername(string username)
        {
            try
            {
                return service.GetUserByUsername(username);
            }
            catch (ArgumentNullException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [HttpGet]
        public IEnumerable<User> GetAllUsers()
        {
            return service.GetAllUsers();
        }

        [HttpGet]
        [Route("api/users/{userId}/chatrooms")]
        public IEnumerable<Chatroom> GetAllChatroomsForUser(int userId)
        {
            try
            {
                return service.GetAllChatroomsForUser(userId);
            }
            catch (ArgumentNullException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [HttpPost]
        public void AddUser([FromBody]User user)
        {
            try
            {
                service.AddUser(user);
            }
            catch (ArgumentException)
            {
                throw new HttpResponseException(HttpStatusCode.NotAcceptable);
            }
        }
    }
}
