using MessengerClient.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
using MessengerClient.DataAccess;

namespace MessengerClient
{
    public partial class AuthenticationDialogWindow : Window
    {
        private User user;
        private UsersAccess users;

        public AuthenticationDialogWindow()
        {
            InitializeComponent();
            this.users = new UsersAccess();
        }

        public User User
        {
            get
            {
                return this.user;
            }
            set
            {
                this.user = value;
            }
        }

        private async void btnSubmit_Click(object sender, RoutedEventArgs args)
        {
            bool successful = true;

            try
            {
                this.User = await users.GetUserByUsername(textBoxUsername.Text);
            }
            catch (ArgumentException ex)
            {
                successful = false;
                MessageBox.Show(ex.Message);
            }

            if (successful)
            {
                this.Close();
            }
        }

        private async void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            string username = this.textBoxUsername.Text;

            if (!ValidateUsername(username))
            {
                MessageBox.Show("Incorrect username.");
                return;
            }

            await this.users.RegisterUser(new User() { Username = username });
            this.user = await users.GetUserByUsername(textBoxUsername.Text);
            this.Close();
        }

        private bool ValidateUsername(string username)
        {
            if (username.Length < 3 || string.IsNullOrEmpty(username) || username.Length > 50)
            {
                return false;
            }

            return true;
        }
    }
}
