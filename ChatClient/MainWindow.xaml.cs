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
        private IChatService service;
        private User user;
        private static List<string> chatRoomList = new List<string>();
        private string currRoom;

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
                service = factory.CreateChannel();

                string username = UsernameTextBox.Text;

                if (string.IsNullOrWhiteSpace(username))
                {
                    throw new Exception("Username cannot be empty!");
                }

                user = new User(username);

                service.ConnectUser(user);

                Title = $"Chat Room - {username}";

                MessageTextBox.IsEnabled = true;
                SendBtn.IsEnabled = true;

                chatRoomList = service.getChatRooms();

                RoomsDDM.ItemsSource = chatRoomList;

                LoginPanel.Visibility = Visibility.Collapsed;
                ChatPanel.Visibility = Visibility.Visible;
                TextPanel.Visibility = Visibility.Collapsed;
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
            try
            {
                string to = UsersDDM.SelectedItem.ToString();
                string text = MessageTextBox.Text;

                if (!string.IsNullOrWhiteSpace(text))
                {
                    Message message = new Message(text, user.Username, to);

                    await Task.Run(() =>
                    {
                        service.SendMessage(currRoom, message);
                    });
                }

                MessageTextBox.Clear();

            } catch(Exception ex)
            {
                MessageBox.Show($"{ex.Message}");
            }
        }

        private void LogOutBtn_Click(object sender, RoutedEventArgs e)
        {
            service.DisconnectUser(user);
            LoginPanel.Visibility = Visibility.Visible;
            ChatPanel.Visibility = Visibility.Collapsed;
        }

        private void JoinBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (currRoom != null)
                {
                    service.ExitChatRoom(currRoom, user);
                }

                Object selectedRoom = RoomsDDM.SelectedItem;

                if (selectedRoom != null)
                {
                    currRoom = selectedRoom.ToString();
                    service.JoinChatRoom(currRoom, user);
                    ChatRoomLbl.Content = currRoom;

                    ChatTextBox.Clear();

                    UsersDDM.ItemsSource = service.getParticipants(currRoom).Where(p => p != user.Username).ToList();
                    UsersDDM.SelectedIndex = 0;

                    TextPanel.Visibility = Visibility.Visible;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CreatBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string newRoomName = CreateTextBox.Text;
                service.CreateChatRoom(newRoomName);

                chatRoomList = service.getChatRooms();
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
            try
            {
                RoomsDDM.Dispatcher.Invoke(new Action(() =>
                {
                    RoomsDDM.ItemsSource = service.getChatRooms();
                }));

                UsersDDM.Dispatcher.Invoke((Action)(() =>
                {
                    UsersDDM.ItemsSource = service.getParticipants(currRoom).Where(p => p != user.Username).ToList();
                    UsersDDM.SelectedIndex = 0;
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
