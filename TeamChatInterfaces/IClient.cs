using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace TeamChatInterfaces
{
    public interface IClient
    {
        [OperationContract]
        void GetMessage(string message, string userName);
        [OperationContract]
        void GetWhisper(string message, string reciever, string sender);
        [OperationContract]
        void GetUpdate(int value, string userName);
        [OperationContract]
        void GetOnOff(string userName, int status);
        [OperationContract]
        void Typing(string userName, int type);
    }
}
