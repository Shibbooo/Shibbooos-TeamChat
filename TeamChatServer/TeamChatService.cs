using TeamChatInterfaces;
using System;
using System.Windows;
using System.Windows.Forms;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Threading;
using System.Net;
using System.Net.NetworkInformation;

namespace TeamChatServer
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single)]
    public class TeamChatService : ITeamChatService
    {
        public ConcurrentDictionary<string, ConnectedClient> _connectedClients = new ConcurrentDictionary<string, ConnectedClient>();

        public int Login(string userName, string passWord)
        {
                foreach (var client in _connectedClients)    //Avoid duplicate usernames - 1=User already online
                {
                    if (client.Key.ToLower() == userName.ToLower())
                    {
                        return 1;
                    }
                }
                string connString = Properties.Settings.Default.ConStr;


            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                SqlCommand getDetails = new SqlCommand("select Account, Password FROM Login WHERE Account = '" + userName + "'", conn);

                SqlDataReader read = getDetails.ExecuteReader();

                if (read.HasRows)
                {
                    while (read.Read())
                    {

                        if (passWord == read.GetString(1))
                        {
                            var establishedUserConnection = OperationContext.Current.GetCallbackChannel<IClient>();

                            var macAddr =
                            (
                                from nic in NetworkInterface.GetAllNetworkInterfaces()
                                where nic.OperationalStatus == OperationalStatus.Up
                                select nic.GetPhysicalAddress().ToString()
                            ).FirstOrDefault();

                            // Retrive the Name of HOST
                            string hostName = Dns.GetHostName();
                            // Get the IP
#pragma warning disable CS0618 // Typ oder Element ist veraltet
                            string myIP = Dns.GetHostByName(hostName).AddressList[0].ToString();
#pragma warning restore CS0618 // Typ oder Element ist veraltet

                            ConnectedClient newClient = new ConnectedClient();
                            newClient.connection = establishedUserConnection;
                            newClient.UserName = userName;

                            _connectedClients.TryAdd(userName, newClient);

                            updateHelper(0, userName);

                            using (SqlConnection conn01 = new SqlConnection(connString))
                            {
                                conn01.Open();

                                SqlCommand SetLogin = new SqlCommand("INSERT INTO LoginLog (Account, IpAddress, MacAddress, Type) VALUES ('" + userName + "', '" + myIP + "', '" + macAddr + "', 1)", conn01);
                                SetLogin.ExecuteNonQuery();
                            }

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("[CONNECT] Client: {0} at {1} - IP: {2} - MAC: {3} - Password: OK", newClient.UserName, System.DateTime.Now, myIP, macAddr);
                            Console.ResetColor();

                            return 0;
                        }
                        else
                        {
                            return 2;
                        }
                    }
                }
                else
                {
                    return 3;
                }
                return 0;
            }
        }

        public void Logout()
        {
            ConnectedClient client = GetMyClient();
            if (client != null)
            {
                ConnectedClient removedclient;
                _connectedClients.TryRemove(client.UserName, out removedclient);

                updateHelper(1, removedclient.UserName);
               
                var macAddr =
                           (
                               from nic in NetworkInterface.GetAllNetworkInterfaces()
                               where nic.OperationalStatus == OperationalStatus.Up
                               select nic.GetPhysicalAddress().ToString()
                           ).FirstOrDefault();

                // Retrive the Name of HOST
                string hostName = Dns.GetHostName();
                // Get the IP
#pragma warning disable CS0618 // Typ oder Element ist veraltet
                string myIP = Dns.GetHostByName(hostName).AddressList[0].ToString();
#pragma warning restore CS0618 // Typ oder Element ist veraltet
                string connString = Properties.Settings.Default.ConStr;
                using (SqlConnection conn01 = new SqlConnection(connString))
                {
                    conn01.Open();

                    SqlCommand SetLogin = new SqlCommand("INSERT INTO LoginLog (Account, IpAddress, MacAddress, Type) VALUES ('" + removedclient.UserName + "', '" + myIP + "', '" + macAddr + "', 0)", conn01);
                    SetLogin.ExecuteNonQuery();
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[DISCONNECT] Client: {0} at {1}", removedclient.UserName, DateTime.Now);
                Console.ResetColor();
            }
        }
        public ConnectedClient GetMyClient()
        {
            var establishedUserConnection = OperationContext.Current.GetCallbackChannel<IClient>();
            foreach (var client in _connectedClients)
            {
                if (client.Value.connection == establishedUserConnection)
                {
                    return client.Value;
                }
            }
            return null;
        }
   
        public void SendMessageToALL(string message, string userName)    // send message to everyone (not yourself)
        {
            string newmessage = message.Replace("'", "''");
            foreach (var client in _connectedClients)
            {
                if (client.Key.ToLower() != userName.ToLower())
                {
                    client.Value.connection.GetMessage(message, userName);
                }
            }
            string connString = Properties.Settings.Default.ConStr;
            using (SqlConnection conn01 = new SqlConnection(connString))
            {
                conn01.Open();

                SqlCommand SetLogin = new SqlCommand("INSERT INTO TalkLog ([From], [To], Message) VALUES ('" + userName + "', 'All', '" + newmessage + "')", conn01);
                SetLogin.ExecuteNonQuery();
            }
            Console.WriteLine("[SEND TO ALL] "+userName+": "+message);
        }

        private void updateHelper(int value, string userName)
        {
            foreach (var client in _connectedClients)
            {
                if (client.Value.UserName.ToLower() != userName.ToLower())
                {
                    client.Value.connection.GetUpdate(value, userName);
                }
            }

        }

        public List<string> getCurrentUsers()
        {
            List<string> listOfUsers = new List<string>();
            foreach(var client in _connectedClients)
            {
                listOfUsers.Add(client.Value.UserName);
            }
            return listOfUsers;
        }

        public void SendOnOff(string userName, int status)
        {
            foreach (var client in _connectedClients)
            {
                 if (client.Key.ToLower() != userName.ToLower())
                 {
                     client.Value.connection.GetOnOff(userName, status);
                 }
            }
        }
        public void SendPoke(string userName, int status, string reciever)
        {
            foreach (var client in _connectedClients)
            {
                if (client.Key.ToLower() == reciever.ToLower())
                {
                    client.Value.connection.GetOnOff(userName, status);
                }
            }
        }

        public void IsTyping(string userName, int type)
        {
            foreach (var client in _connectedClients)
            {
                if (client.Key.ToLower() != userName.ToLower())
                {
                    client.Value.connection.Typing(userName, type);
                }
            }
        }

        public void WhisperToUser(string message, string reciever, string sender)    // send message to everyone (not yourself)
        {
            foreach (var client in _connectedClients)
            {
                if (client.Key.ToLower() == reciever.ToLower())
                {
                    client.Value.connection.GetWhisper(message, reciever, sender);
                }
            }
            string connString = Properties.Settings.Default.ConStr;
            using (SqlConnection conn01 = new SqlConnection(connString))
            {
                conn01.Open();

                SqlCommand SetLogin = new SqlCommand("INSERT INTO TalkLog ([From], [To], Message) VALUES ('" + sender + "', '" + reciever + "', '" + message + "')", conn01);
                SetLogin.ExecuteNonQuery();
            }

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("[WHISPER] " + sender + " TO " + reciever + ": " + message);
            Console.ResetColor();
        }   
    }
}
