using MessengerClient.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MessengerClient.DataAccess
{
    public class UsersAccess : BaseAccess
    {
        private string usersUrl;

        public UsersAccess() : base()
        {
            this.usersUrl = base.mainUrl + "/users";
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            string url = $"{this.usersUrl}/{userId}";
            return await this.GetData<User>(url);
        }

        public async Task<User> GetUserByUsername(string username)
        {
            string url = $"{this.usersUrl}?username={username}";

            try
            {
                return await this.GetData<User>(url);
            }
            catch (WebException webEx)
            {
                if (webEx.Status == WebExceptionStatus.ProtocolError)
                {
                    var response = webEx.Response as HttpWebResponse;

                    if (response != null)
                    {
                        if ((int)response.StatusCode == 404)
                        {
                            throw new ArgumentException($"Invalid username");
                        }
                    }
                }

                throw new InvalidOperationException("Unindentified problem. Please restart the application.");
            }
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await this.GetData<IEnumerable<User>>(this.usersUrl);
        }
       
        public async Task<IEnumerable<Chatroom>> GetChatroomsForUser(User user)
        {
            string url = $"{this.usersUrl}/{user.UserID}/chatrooms";
            return await this.GetData<IEnumerable<Chatroom>>(url);
        }

        public async Task RegisterUser(User user)
        {
            await this.PostData<User>(this.usersUrl, user);
        }
    }
}
