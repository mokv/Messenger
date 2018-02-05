using MessengerClient.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MessengerClient.DataAccess
{
    public class MessengerAccess : BaseAccess
    {
        private string messengerUrl;

        public MessengerAccess() : base()
        {
            this.messengerUrl = mainUrl + "/messenger";
        }

        public async Task<IEnumerable<Message>> GetMessagesForChatroom(int chatroomId)
        {
            string url = $"{this.messengerUrl}/{chatroomId}";
            return await this.GetData<IEnumerable<Message>>(url);
        }

        public async Task<Chatroom> GetChatroomByName(string name)
        {
            string url = $"{this.messengerUrl}?name={name}";
            return await this.GetData<Chatroom>(url);
        }
        
        public async Task SendMessage(Message message)
        {
            string url = $"{this.messengerUrl}/{message.ChatroomID.ToString()}";
            await this.PostData(url, message);
        }

        public async Task CreateChatroom(Chatroom chatroom)
        {
            await this.PostData<Chatroom>(this.messengerUrl, chatroom);
        }

        public async Task AddUserToChatroom(int chatroomId, User user)
        {
            string url = $"{this.messengerUrl}/{chatroomId}";

            try
            {
                await this.PutData<User>(url, user);
            }
            catch (WebException webEx)
            {
                if (webEx.Status == WebExceptionStatus.ProtocolError)
                {
                    var response = webEx.Response as HttpWebResponse;

                    if (response != null)
                    {
                        if((int)response.StatusCode == 406)
                        {
                            throw new ArgumentException($"{user.Username} is already a member of the chatroom");
                        }
                    }
                }
            }
        }
    }
}
