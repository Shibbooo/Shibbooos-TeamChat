using TeamChatInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamChatServer
{
    public class ConnectedClient
    {
        public IClient connection;
        public string UserName { get; set; }
    }
}
