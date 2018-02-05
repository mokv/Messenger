using Messenger.Data;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Messenger.WebApi.Hubs
{
    public class ChatHub : Hub
    {
        public void SendMessage(Message message)
        {
            Clients.All.ReceiveMessage(message);
        }
    }
}