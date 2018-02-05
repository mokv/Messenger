using MessengerClient.DataAccess;
using MessengerClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MessengerClient
{
    /// <summary>
    /// Interaction logic for CreateChatroomWindow.xaml
    /// </summary>
    public partial class CreateChatroomWindow : Window
    {
        private MessengerAccess messenger;
        private User user;

        public CreateChatroomWindow(User user)
        {
            InitializeComponent();
            this.messenger = new MessengerAccess();
            this.user = user;
        }

        private async void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            string newName = textBoxChatroomName.Text;
            await this.messenger.CreateChatroom(new Chatroom() { Name = newName });
            Chatroom chatroom = await this.messenger.GetChatroomByName(newName);
            await this.messenger.AddUserToChatroom(chatroom.ChatroomID, this.user);
            this.Close();
        }
    }
}
