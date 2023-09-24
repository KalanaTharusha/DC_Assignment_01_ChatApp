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
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    internal class ChatService : IChatService
    {
        private List<User> _users;
        public ChatService()
        {
            _users = new List<User>();
        }

        public void Connect(User user)
        {
            _users.Add(user);
            Console.WriteLine(user.UserName + " Connected");
        }
    }
}
