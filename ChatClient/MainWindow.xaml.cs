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
        private string username;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            //User newUser = new User();
            //newUser.UserName = usernameTextBox.Text;
            username = usernameTextBox.Text;

            InstanceContext context = new InstanceContext(this);
            DuplexChannelFactory<IChatService> factory = new DuplexChannelFactory<IChatService>(context, new NetTcpBinding(), "net.tcp://localhost:8000/ChatService");
            foob = factory.CreateChannel();
            foob.ConnectUser(username);

            Title = $"Chat Room - {username}";

            messageTextBox.IsEnabled = true;
            sendButton.IsEnabled = true;

            LoginPanel.Visibility = Visibility.Collapsed;
            ChatPanel.Visibility = Visibility.Visible;
        }

        public void ReceiveMessage(string username, string message)
        {
            chatTextBox.AppendText($"{username}: {message}\n");
        }

        private async void sendButton_Click(object sender, RoutedEventArgs e)
        {
            string message = messageTextBox.Text;
            await Task.Run(() =>
            {
                foob.SendMessage(username, message);
            });
            messageTextBox.Clear();
        }
    }
}
