using ChatServerInterface;
using DLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    internal class ChatService : IChatService
    {
        private List<User> _users;
        private static readonly List<IChatCallback> clients = new List<IChatCallback>();

        public ChatService()
        {
            _users = new List<User>();
        }

        public void ConnectUser(string username)
        {
            var callback = OperationContext.Current.GetCallbackChannel<IChatCallback>();

            if (!clients.Contains(callback))
            {
                clients.Add(callback);
                Console.WriteLine(username + " connected");
            }
        }

        public void SendMessage(string username, string message)
        {
            Console.WriteLine(message);

            foreach (var client in clients)
            {
                client.ReceiveMessage(username, message);
                Console.WriteLine(message + " sent to " + client);
            }
        }





        private bool _IsUserConnected(string username)
        {
            foreach (var user in _users)
            {
                if (user.UserName == username)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
