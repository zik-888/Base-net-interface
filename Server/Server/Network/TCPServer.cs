using System;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;


/// <summary>
/// BaseInterfc -> Client/Server -> UDP/TCP
/// </summary>

namespace Server.Network
{
    public class TCPServer
    {
        protected TcpListener server;
        protected InternalClient client;

        //public string IpPort { get { return server.LocalEndpoint.ToString(); } }
        public TCPServer(IPEndPoint ipPort)
        {
            StartServer(ipPort);
            WaitConnection();
        }

        protected void StartServer(IPEndPoint ipPort)
        {
            server = new TcpListener(ipPort);
            server.Start();
            Console.WriteLine("IP/Port: " + server.LocalEndpoint.ToString());
        }

        protected void WaitConnection()
        {
            client = new InternalClient(server);
        }

        public async Task<byte[]> WaitMsg()
        {
            return await client.WaitMsg();
        }

        public void SendMsg(byte[] data)
        {
            client.SendMsg(data);
        }

        public void CloseServer()
        {
            client.Close();
            server.Stop();
        }


        protected class InternalClient : TCPClient
        {
            public InternalClient(TcpListener server) => Start(server);
            private async void Start(TcpListener server)
            {
                while (true) // нужно доделать???
                {
                    Console.WriteLine("Ожидание новых подключений... ");
                    client = await server.AcceptTcpClientAsync();
                    stream = client.GetStream();
                    connect = true;
                    Console.WriteLine("Подключен клиент");
                }
            }
        }
    }
}
