using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using DLL;

namespace ChatServerInterface
{
    [ServiceContract(CallbackContract = typeof(IChatCallback))]
    public interface IChatService
    {

        [OperationContract]
        void ConnectUser(string username);

        [OperationContract]
        void SendMessage(string username, string message);
    }
}
