using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using Server.Network;

using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

namespace Server
{
    enum NetInterface { TCP, UDP }

    class Program
    {

        static void Main(string[] args)
        {
            NetInterface netInterface = NetInterface.TCP;

            if (netInterface == NetInterface.TCP)
            {
                Stopwatch stopwatch = new Stopwatch();


                TCPServer server = new TCPServer(new IPEndPoint(IPAddress.Parse("192.168.1.5"), 1024));
                TCPClient client = new TCPClient(new IPEndPoint(IPAddress.Parse("192.168.1.5"), 1024));
                
                UpdateMsgTCPAsync(server);

                stopwatch.Start();

                while (stopwatch.ElapsedMilliseconds < 10000)
                {
                    client.SendMsg(Encoding.Default.GetBytes(Serialization.GetSerial(server)));
                }


                Console.ReadLine();

                server.SendMsg(Encoding.Default.GetBytes(Serialization.GetSerial(server)));

                Console.ReadLine();
            }

            if(netInterface == NetInterface.UDP)
            {
                //UDPNode node = new UDPNode(new IPEndPoint(IPAddress.Parse("192.168.1.6"), 1024)); 10.193.51.151
                UDPNode node1 = new UDPNode(new IPEndPoint(IPAddress.Parse("192.168.1.5"), 1024));

                UDPNode node2 = new UDPNode(new IPEndPoint(IPAddress.Parse("192.168.1.5"), 1025));


                UpdateMsgUDPAsync(node1); 
                UpdateMsgUDPAsync(node2);

                Console.ReadLine();

                //node.SendMsg(Encoding.Default.GetBytes(Serialization.GetSerial(node)), 
                  //              new IPEndPoint(IPAddress.Parse("10.193.51.133"/*"192.168.1.2"*/), 1024));

                Console.ReadLine();
            }

        }

        public static async void UpdateMsgTCPAsync(TCPServer server)
        {
            while (true)
            {
                await server.WaitMsg();
            }
        }

        public static async void UpdateMsgUDPAsync(UDPNode node)
        {
            while (true)
            {
                byte[] x = await node.WaitMsg();
                node.SendMsg(x, new IPEndPoint(IPAddress.Parse("192.168.1.5"), 1025));
            }
        }

    }
}
