using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ChatServerInterface;

namespace ChatServer
{
    internal class Program
    {

        static void Main(string[] args)
        {
            //Console.WriteLine("Chat server started");

            //ServiceHost host;

            //NetTcpBinding tcp = new NetTcpBinding();

            //host = new ServiceHost(typeof(ChatService));

            //host.AddServiceEndpoint(typeof(IChatService), tcp, "net.tcp://0.0.0.0:8000/ChatService");
            //host.Open();
            //Console.WriteLine("Chat Server Online");
            //Console.ReadLine();
            //host.Close();

            Console.WriteLine("Chat server started");

            ServiceHost host = new ServiceHost(typeof(ChatService));
            host.AddServiceEndpoint(typeof(IChatService), new NetTcpBinding(), "net.tcp://localhost:8000/ChatService");
            var behavior = new ServiceMetadataBehavior { HttpGetEnabled = false };
            host.Description.Behaviors.Add(behavior);
            host.Open();

            Console.WriteLine("Chat Server Online");
            Console.ReadLine();

            host.Close();
        }
    }
}
