using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using DLL;

namespace ChatServerInterface
{
    [ServiceContract]
    public interface IChatService
    {
        [OperationContract]
        void Connect(User user);
    }
}
