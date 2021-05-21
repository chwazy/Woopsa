using System;
using System.Collections.Generic;
using System.Net.Sockets;
using Woopsa;

namespace WoopsaDemoServerWithUnity
{
    public class BooleanIndicator
    {
        public bool Value { get; set; }
    }

    [WoopsaVisibilityAttribute(WoopsaVisibility.DefaultIsVisible)]
    public class DemoMachine
    {
        public string MachineName { get; set; }

        public BooleanIndicator IsRunning { get; private set; } = new BooleanIndicator();
    }

    /// <summary>
    /// This project is used to test the notification system with Woopsa for Unity. 
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                DemoMachine root = new DemoMachine();
                bool done = false;
                using (WoopsaServer woopsaServer = new WoopsaServer(root, 80))
                {

                    Console.WriteLine("Woopsa server listening on http://localhost:{0}{1}", woopsaServer.WebServer.Port, woopsaServer.RoutePrefix);

                    Console.WriteLine();
                    Console.WriteLine("Commands : START, STOP, QUIT");
                    do
                    {
                        Console.Write(">");
                        switch (Console.ReadLine().ToUpper())
                        {
                            case "QUIT":
                                done = true;
                                break;
                            case "START":
                                root.IsRunning.Value = true;
                                Console.WriteLine("Started demo machine");
                                break;
                            case "STOP":
                                root.IsRunning.Value = false;
                                Console.WriteLine("Stopped demo machine");
                                break;
                            default:
                                Console.WriteLine("Invalid command");
                                break;
                        }
                    }
                    while (!done);
                }
            }
            catch (SocketException e)
            {
                // A SocketException is caused by an application already listening on a port in most cases
                // Applications known to use port 80:
                //  - On Windows 10, IIS is on by default on some configurations. Disable it here: 
                //    http://stackoverflow.com/questions/30758894/apache-server-xampp-doesnt-run-on-windows-10-port-80
                //  - IIS
                //  - Apache
                //  - Nginx
                //  - Skype
                Console.WriteLine("Error: Could not start Woopsa Server. Most likely because an application is already listening on port 80.");
                Console.WriteLine("Known culprits:");
                Console.WriteLine(" - On Windows 10, IIS is on by default on some configurations.");
                Console.WriteLine(" - Skype");
                Console.WriteLine(" - Apache, nginx, etc.");
                Console.WriteLine("SocketException: {0}", e.Message);
                Console.ReadLine();
            }
        }
    }
}
