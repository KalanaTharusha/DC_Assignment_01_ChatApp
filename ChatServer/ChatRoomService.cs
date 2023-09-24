using ChatServerInterface;
using DLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServerInterface
{
    public class ChatRoomService
    {
        public string roomName;
        private Dictionary<User, IChatCallback> participants = new Dictionary<User, IChatCallback>();
        public ChatRoomService(string roomName)
        {
            this.roomName = roomName;
        }

        public void AddParticipant(User user, IChatCallback chatCallback)
        {
            participants.Add(user, chatCallback);
        }

        public void BroadCastMessage(Message message)
        {
            foreach (var participant in participants.Values)
            {
                participant.ReceiveMessage(message);
            }
        }
    }
}
