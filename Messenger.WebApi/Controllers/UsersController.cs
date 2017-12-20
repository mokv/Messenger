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
        public IEnumerable<User> GetAllUsers()
        {
            return service.GetAllUsers();
        }

        [HttpGet]
        public IEnumerable<Chatroom> GetAllChatroomsForUser(int id)
        {
            try
            {
                return service.GetAllChatroomsForUser(id);
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
