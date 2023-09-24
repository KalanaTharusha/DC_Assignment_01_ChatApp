using ChatServerInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChatClient
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class ChatClientImpl
    {
        //private IChatService chatService;
        //private string username;

        //public ChatClientImpl(string username)
        //{
        //    this.username = username;

        //    InstanceContext context = new InstanceContext(this);
        //    DuplexChannelFactory<IChatService> factory = new DuplexChannelFactory<IChatService>(context, new NetTcpBinding(), "net.tcp://localhost:9000/ChatService");
        //    chatService = factory.CreateChannel();
        //    chatService.ConnectUser(username);
        //}

        //public void Start()
        //{
        //    Application.Current.MainWindow = new ChatWindow(username, this);
        //    Application.Current.MainWindow.Show();
        //}

        //public void ReceiveMessage(string username, string message)
        //{
        //    (Application.Current.MainWindow as ChatWindow)?.ReceiveMessage(username, message);
        //}

        //static void Main(string[] args)
        //{
        //    Console.Write("Enter your username: ");
        //    string username = Console.ReadLine();

        //    ChatClient client = new ChatClient(username);
        //    client.Start();

        //    // Start the WPF application
        //    Application app = new Application();
        //    app.Run();
        //}
    }
}
