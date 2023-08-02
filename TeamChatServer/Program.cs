using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TeamChatServer
{
    class Program
    {
        public static TeamChatService _server;
        static void Main(string[] args)
        {
            string _version = "1.0.0.1016";
            _server = new TeamChatService();
            using (ServiceHost host = new ServiceHost(_server))
            {
                //Thread.Sleep(1000);
                //Console.WriteLine("Loading settings");
                //Thread.Sleep(2000);
                //Console.WriteLine("Loading libraries");
                //Thread.Sleep(2000);
                //Console.WriteLine("Loading security engine");
                //Thread.Sleep(2000);
                Console.WriteLine("Starting chatserver");
                Thread.Sleep(2000);
                Console.WriteLine("[SERVER] version: "+ _version);
                Thread.Sleep(500);
                //Console.WriteLine("[SERVER] duplex channel: ENABLED");
                //Thread.Sleep(500);
                //Console.WriteLine("Type /help to see all available commands.\n");
                //Thread.Sleep(500);
                Console.WriteLine(" Team Chat Service now running! You may connect with a client now.");
                host.Open();



                string cmd = Console.ReadLine();
                while (true)
                {
                    if (cmd == "/stop")
                    {
                        Console.WriteLine("Stopping Chatserver....");
                        Thread.Sleep(4000);
                        Console.WriteLine("Saving ChatLog...");
                        Thread.Sleep(500);
                        Console.WriteLine("Saving Remote Client Serial IDs...");
                        Thread.Sleep(500);
                        Console.WriteLine("Saving your Mom from exploding...");
                        Thread.Sleep(500);
                        Console.WriteLine("Shitting into your bed...");
                        Thread.Sleep(500);
                        Console.WriteLine("Fucking your sister...");
                        Thread.Sleep(500);
                        Console.WriteLine("Eating your family...");
                        Thread.Sleep(500);
                        Console.WriteLine("Shitting out your family...");
                        Thread.Sleep(500);
                        Console.WriteLine("Stopping Chatserver....");
                        Thread.Sleep(1000);
                        Environment.Exit(0);
                    }
                    else if (cmd == "/help")
                    {
                        Console.WriteLine("\n##### COMMANDS #####\n/help - see all available commands\n/stop - stop the server immediately");
                        cmd = Console.ReadLine();
                    }
                    else if (cmd == "/say")
                    {
                        try
                        {
                            //TeamChatService.SendMessageToALL("Test", "SERVER");
                            string said = Console.ReadLine();
                            if (said != "")
                            {

                            }
                        }
                        catch
                        {
                            Console.WriteLine("Error");
                        }

                    }
                    else
                    {
                        cmd = Console.ReadLine();
                    }
                }
            }

        }
        
    }

    
}
