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
using ChatServerInterface;
using System.ServiceModel;
using DLL;

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IChatCallback
    {
        private IChatService foob;
        private User user;
        private static List<string> chatRoomList = new List<string>();
        private string currRoom;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            //User newUser = new User();
            //newUser.UserName = usernameTextBox.Text;
            string username = usernameTextBox.Text;
            user = new User(username);

            InstanceContext context = new InstanceContext(this);
            DuplexChannelFactory<IChatService> factory = new DuplexChannelFactory<IChatService>(context, new NetTcpBinding(), "net.tcp://localhost:8000/ChatService");
            foob = factory.CreateChannel();
            foob.ConnectUser(user);

            Title = $"Chat Room - {username}";

            messageTextBox.IsEnabled = true;
            sendButton.IsEnabled = true;

            chatRoomList = foob.getChatRooms();

            RoomsDDM.ItemsSource = chatRoomList;

            LoginPanel.Visibility = Visibility.Collapsed;
            ChatPanel.Visibility = Visibility.Visible;
        }

        public void ReceiveMessage(Message message)
        {
            chatTextBox.AppendText($"{message.Time} {message.From}: {message.Text}\n");
        }

        private async void sendButton_Click(object sender, RoutedEventArgs e)
        {
            string text = messageTextBox.Text;
            Message message = new Message(text, user.Username, "all");

            await Task.Run(() =>
            {
                foob.SendMessage(currRoom, message);
            });
            messageTextBox.Clear();
        }

        private void LogOutBtn_Click(object sender, RoutedEventArgs e)
        {
            foob.DisconnectUser(user);
            LoginPanel.Visibility = Visibility.Visible;
            ChatPanel.Visibility = Visibility.Collapsed;
        }

        private void JoinBtn_Click(object sender, RoutedEventArgs e)
        {
            Object selectedRoom = RoomsDDM.SelectedItem;

            if (selectedRoom != null)
            {
                currRoom = selectedRoom.ToString();
                foob.JoinChatRoom(currRoom, user);
                ChatRoomLbl.Content = currRoom;
            }
        }
    }
}
