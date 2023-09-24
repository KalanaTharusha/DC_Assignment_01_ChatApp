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

        //private static readonly List<IChatCallback> clients = new List<IChatCallback>();
        private static readonly Dictionary<User, IChatCallback> clients = new Dictionary<User, IChatCallback>();
        private static readonly List<ChatRoomService> chatRooms = new List<ChatRoomService>();

        public ChatService()
        {
            if (chatRooms.Count == 0)
            {
                ChatRoomService cr1 = new ChatRoomService("chat room 1");
                ChatRoomService cr2 = new ChatRoomService("chat room 2");
                chatRooms.Add(cr1);
                chatRooms.Add(cr2);
            }
        }

        public void ConnectUser(User user)
        {
            var callback = OperationContext.Current.GetCallbackChannel<IChatCallback>();

            if (!clients.ContainsKey(user) && !clients.ContainsValue(callback))
            {
                clients.Add(user, callback);
                Console.WriteLine(user.Username + " connected");
            }
        }

        public void JoinChatRoom(string roomName, User user)
        {
            ChatRoomService chatRoom = chatRooms.FirstOrDefault(cr =>  cr.roomName == roomName);

            if(chatRoom != null)
            {
                foreach (var client in clients)
                {
                    if(client.Key.Username == user.Username)
                    {
                        chatRoom.AddParticipant(client.Key, client.Value);
                    }
                }
            }

        }

        public void SendMessage(string roomName, Message message)
        {
            ChatRoomService chatRoom = chatRooms.FirstOrDefault(cr => cr.roomName == roomName);

            if (chatRoom != null)
            {
                chatRoom.BroadCastMessage(message);
            }

        }

        public List<string> getChatRooms()
        {
            List<String> roomList = new List<String>();
            foreach (var room in chatRooms)
            {
                roomList.Add(room.roomName);
            }

            return roomList;
        }

        public void DisconnectUser(User user)
        {
            //IChatCallback key = null;

            //foreach (var client in clients)
            //{
            //    if(client.Value == user)
            //    {
            //        key = client.Key;
            //    }
            //}

            //clients.Remove(key);
        }
    }
}
