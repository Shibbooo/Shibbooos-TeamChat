using TeamChatInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TeamChatClient
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class ClientCallBack : IClient
    {
        public void GetMessage(string message, string userName)
        {
            ((MainWindow)Application.Current.MainWindow).TakeMessage(message, userName);
        }

        public void GetOnOff(string userName, int status)
        {
            ((MainWindow)Application.Current.MainWindow).TakeOnOff(userName, status);
        }
        public void Typing(string userName, int type)
        {
            ((MainWindow)Application.Current.MainWindow).TakeType(userName, type);
        }
        public void GetUpdate(int value, string userName)
        {
            switch (value)
            {
                case 0:
                    {
                        ((MainWindow)Application.Current.MainWindow).AddUserToList(userName);
                        break;
                    }
                case 1:
                    {
                        ((MainWindow)Application.Current.MainWindow).RemoveUserFromList(userName);
                        break;
                    }
            }
        }

        public void GetWhisper(string message, string reciever, string sender)
        {
            ((MainWindow)Application.Current.MainWindow).TakeWhisper(message, reciever, sender);
        }
    }
}
