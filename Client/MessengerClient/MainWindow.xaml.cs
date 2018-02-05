using MessengerClient.Models;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using System.Net;
using System.Net.Mime;
using MessengerClient.Hubs;
using MessengerClient.DataAccess;
using Microsoft.Win32;
using System.IO;

namespace MessengerClient
{
    public partial class MainWindow : Window
    {
        private readonly User user;
        private int currentChatroomId;
        private MessengerAccess messenger;
        private UsersAccess users;
        private ChatHub chatHub;

        public MainWindow()
        {
            InitializeComponent();
            AuthenticationDialogWindow authenticationDialog = new AuthenticationDialogWindow();
            authenticationDialog.ShowDialog();
            this.user = authenticationDialog.User;

            if (this.user == null)
            {
                Environment.Exit(0);
            }

            this.messenger = new MessengerAccess();
            this.users = new UsersAccess();
            GenerateAllChatrooms();
            this.chatHub = new ChatHub();
        }

        public int CurrentChatroomId
        {
            get
            {
                return this.currentChatroomId;
            }
            set
            {
                this.currentChatroomId = value;
            }
        }

        private async void btnChatroom_Click(object sender, RoutedEventArgs e)
        {
            Button thisButton = (Button)sender;
            this.CurrentChatroomId = (int)thisButton.Tag;

            Chatroom chatroom = new Chatroom()
            {
                Name = thisButton.Content.ToString(),
                ChatroomID = (int)thisButton.Tag
            };

            await SelectChatroom(chatroom);
        }

        private async void btnSend_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.textBoxNewMessage.Text))
            {
                Message message = new Message()
                {
                    Text = this.textBoxNewMessage.Text,
                    SenderID = this.user.UserID,
                    ChatroomID = this.CurrentChatroomId
                };

                await this.messenger.SendMessage(message);
                await this.chatHub.SendMessageToHub(message);
                this.textBoxNewMessage.Text = string.Empty;
            }
        }

        public async void AddMessage(Message message)
        {
            if (message.ChatroomID == this.currentChatroomId)
            {
                TextBox textBox = await CreateMessageTextBox(message);
                this.stackPanelMessages.Children.Add(textBox);

                if (message.Image != null)
                {
                    Image img = new Image() { Source = Base64ToImage(message.Image) };
                    
                    if(message.SenderID == user.UserID)
                    {
                        img.HorizontalAlignment = HorizontalAlignment.Right;
                    }
                    else
                    {
                        img.HorizontalAlignment = HorizontalAlignment.Left;
                    }

                    this.stackPanelMessages.Children.Add(img);
                }
            }
        }

        private async void GenerateAllChatrooms()
        {
            stackPanelChatrooms.Children.Clear();
            List<Chatroom> chatrooms = (await this.users.GetChatroomsForUser(this.user)).ToList();

            foreach (Chatroom chatroom in chatrooms)
            {
                Button btnChatroom = new Button()
                {
                    Content = chatroom.Name,
                    Height = 30,
                    Tag = chatroom.ChatroomID,
                    ContentStringFormat = chatroom.Name,
                };

                btnChatroom.AddHandler(Button.ClickEvent, new RoutedEventHandler(btnChatroom_Click));
                stackPanelChatrooms.Children.Add(btnChatroom);
            }

            await SelectChatroom(chatrooms[chatrooms.Count - 1]);
        }

        private async Task<TextBox> CreateMessageTextBox(Message message)
        {
            User messageSender = null;

            if (message.SenderID == this.user.UserID)
            {
                messageSender = this.user;
            }
            else
            {
                messageSender = await this.users.GetUserByIdAsync(message.SenderID);
            }

            TextBox textBox = new TextBox();
            textBox.Text = string.Format($"|{message.SentDate}|{messageSender.Username}|: {message.Text}");
            return textBox;
        }

        private void CreateChatroom_Click(object sender, RoutedEventArgs e)
        {
            CreateChatroomWindow authenticationDialog = new CreateChatroomWindow(this.user);
            authenticationDialog.ShowDialog();
            GenerateAllChatrooms();
        }

        private void AddMembers_Click(object sender, RoutedEventArgs e)
        {
            if (this.CurrentChatroomId == 0)
            {
                MessageBox.Show("There is no chatroom selected");
                return;
            }

            AddMembersWindow addMembersWindow = new AddMembersWindow(this.CurrentChatroomId);
            addMembersWindow.ShowDialog();
        }

        private async Task SelectChatroom(Chatroom chatroom)
        {
            this.textBlockChatroomName.Text = chatroom.Name;
            this.CurrentChatroomId = chatroom.ChatroomID;
            IEnumerable<Message> messages = await this.messenger.GetMessagesForChatroom(this.CurrentChatroomId);
            this.stackPanelMessages.Children.Clear();

            foreach (Message message in messages)
            {
                AddMessage(message);
            }
        }

        private async void UploadPhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog()
            {
                Title = "Select a photo",
                Multiselect = false,
                DefaultExt = ".png",
                Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif"
            };

            bool? result = fileDialog.ShowDialog();

            if (result == true)
            {
                Message message = new Message()
                {
                    SenderID = this.user.UserID,
                    ChatroomID = this.CurrentChatroomId,
                    Image = ImageToBase64(fileDialog.FileName)
                };

                await this.messenger.SendMessage(message);
                await this.chatHub.SendMessageToHub(message);
            }
        }

        private string ImageToBase64(string imageFilePath)
        {
            byte[] imageArray = System.IO.File.ReadAllBytes(imageFilePath);
            return Convert.ToBase64String(imageArray);
        }

        private TransformedBitmap Base64ToImage(string base64Image)
        {
            double sizeToResize = 200;
            byte[] binaryData = Convert.FromBase64String(base64Image);
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new MemoryStream(binaryData);
            bitmapImage.EndInit();
            double dimension = 0;

            if(bitmapImage.Height > bitmapImage.Width)
            {
                dimension = sizeToResize / bitmapImage.Height;
            }
            else
            {
                dimension = sizeToResize / bitmapImage.Width;
            }

            var transformedBitmap = new TransformedBitmap(bitmapImage, new ScaleTransform(dimension, dimension));
            return transformedBitmap;
        }
    }
}
