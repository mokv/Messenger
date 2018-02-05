using MessengerClient.Models;
using MessengerClient;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Net;

namespace MessengerClient.Hubs
{
    public class ChatHub
    {
        private HubConnection hubConnection;
        private IHubProxy chatHubProxy;
        private MainWindow mainWindow;

        public ChatHub()
        {
            this.mainWindow = (MainWindow)Application.Current.MainWindow;
            this.hubConnection = new HubConnection(ConfigurationManager.AppSettings.Get("serverMainUrl"));
            this.hubConnection.Headers.Add(HttpRequestHeader.ContentType.ToString(), "application/json");
            this.chatHubProxy = hubConnection.CreateHubProxy("ChatHub");
            this.chatHubProxy.On<Message>("ReceiveMessage", message =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Debug.WriteLine("Receive message called on client.");
                    mainWindow.AddMessage(message);
                });
            });

            ConnectSignalR(hubConnection);
        }

        private async void ConnectSignalR(HubConnection hubConnection)
        {
            await hubConnection.Start();
        }

        public async Task SendMessageToHub(Message message)
        {
            Debug.WriteLine("Send message to hub called on client.");
            await chatHubProxy.Invoke<Message>("SendMessage", message);
        }
    }
}
