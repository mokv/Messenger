using MessengerClient.DataAccess;
using MessengerClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for AddMembersWindow.xaml
    /// </summary>
    public partial class AddMembersWindow : Window
    {
        private UsersAccess users;
        private MessengerAccess messenger;
        private int chatroomId;
        private IEnumerable<User> allUsers;

        public AddMembersWindow(int chatroomId)
        {
            InitializeComponent();
            this.users = new UsersAccess();
            this.messenger = new MessengerAccess();
            this.chatroomId = chatroomId;
            GetAllUsers();
        }

        private async void AddMembers_Click(object sender, RoutedEventArgs e)
        {
            bool successful = true;

            foreach(var user in this.listViewUsers.SelectedItems)
            {
                try
                {
                    await this.messenger.AddUserToChatroom(this.chatroomId, (User)user);
                }
                catch (ArgumentException ex)
                {
                    successful = false;
                    MessageBox.Show(ex.Message);
                }
            }

            if (successful)
            {
                this.Close();
            }
        }

        private async void GetAllUsers()
        {
            this.allUsers = await this.users.GetAllUsers();
            
            foreach (var user in this.allUsers)
            {
                this.listViewUsers.Items.Add(user);
            }
        }
    }
}
