using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using DLL;

namespace ChatServerInterface
{
    public interface IChatCallback
    {

        [OperationContract(IsOneWay = true)]
        void ReceiveMessage(Message message);

        [OperationContract(IsOneWay = true)]
        void UpdateChatRoomInfo(string chatRoomName);

        [OperationContract(IsOneWay = true)]
        void OnUserJoinedChatRoom(string username);

        [OperationContract(IsOneWay = true)]
        void OnUserLeaveChatRoom(string username);
    }
}
