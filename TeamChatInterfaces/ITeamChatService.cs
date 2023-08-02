using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace TeamChatInterfaces
{
    [ServiceContract(CallbackContract = typeof(IClient))]
    public interface ITeamChatService
    {
        [OperationContract]
        int Login(string userName, string passWord);
        [OperationContract]
        void Logout();
        [OperationContract]
        void SendMessageToALL(string message, string userName);
        [OperationContract]
        void WhisperToUser(string message, string reciever, string sender);
        [OperationContract]
        void SendOnOff(string userName, int status);
        [OperationContract]
        void SendPoke(string userName, int status, string reciever);
        [OperationContract]
        List<string> getCurrentUsers();
        [OperationContract]
        void IsTyping(string userName, int type);
    }
}
