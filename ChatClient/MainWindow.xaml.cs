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
        private List<string> currUsers;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                InstanceContext context = new InstanceContext(this);
                DuplexChannelFactory<IChatService> factory = new DuplexChannelFactory<IChatService>(context, new NetTcpBinding(), "net.tcp://localhost:8000/ChatService");
                foob = factory.CreateChannel();

                string username = UsernameTextBox.Text;

                user = new User(username);

                foob.ConnectUser(user);

                Title = $"Chat Room - {username}";

                MessageTextBox.IsEnabled = true;
                SendBtn.IsEnabled = true;

                chatRoomList = foob.getChatRooms();

                RoomsDDM.ItemsSource = chatRoomList;



                LoginPanel.Visibility = Visibility.Collapsed;
                ChatPanel.Visibility = Visibility.Visible;
            }
            catch (FaultException<ServerFault> ex)
            {
                MessageBox.Show(ex.Detail.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void ReceiveMessage(Message message)
        {
            ChatTextBox.AppendText($"{message.Time.TimeOfDay.Hours}:{message.Time.TimeOfDay.Minutes} {message.From}: {message.Text}\n");
        }

        private async void SendBtn_Click(object sender, RoutedEventArgs e)
        {
            string to = UsersDDM.SelectedItem.ToString();
            string text = MessageTextBox.Text;
            Message message = new Message(text, user.Username, to);

            await Task.Run(() =>
            {
                foob.SendMessage(currRoom, message);
            });
            MessageTextBox.Clear();
        }

        private void LogOutBtn_Click(object sender, RoutedEventArgs e)
        {
            foob.DisconnectUser(user);
            LoginPanel.Visibility = Visibility.Visible;
            ChatPanel.Visibility = Visibility.Collapsed;
        }

        private void JoinBtn_Click(object sender, RoutedEventArgs e)
        {
            if (currRoom != null)
            {
                foob.ExitChatRoom(currRoom, user);
            }

            Object selectedRoom = RoomsDDM.SelectedItem;

            if (selectedRoom != null)
            {
                currRoom = selectedRoom.ToString();
                foob.JoinChatRoom(currRoom, user);
                ChatRoomLbl.Content = currRoom;

                ChatTextBox.Clear();
                currUsers = foob.getParticipants(currRoom);
                UsersDDM.ItemsSource = currUsers;
                UsersDDM.SelectedIndex = 0;

            }
        }

        private void CreatBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string newRoomName = CreateTextBox.Text;
                foob.CreateChatRoom(newRoomName);

                chatRoomList = foob.getChatRooms();
                RoomsDDM.ItemsSource = chatRoomList;
                CreateTextBox.Clear();
            }
            catch (FaultException<ServerFault> ex)
            {
                MessageBox.Show(ex.Detail.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void RefreashBtn_Click(object sender, RoutedEventArgs e)
        {
            RoomsDDM.Dispatcher.Invoke(new Action(() =>
            {
                RoomsDDM.ItemsSource = foob.getChatRooms();
            }));

            UsersDDM.Dispatcher.Invoke((Action)(() =>
            {
                currUsers = foob.getParticipants(currRoom);
                UsersDDM.ItemsSource = currUsers;
                UsersDDM.SelectedIndex = 0;
            }));
        }

    }
}
