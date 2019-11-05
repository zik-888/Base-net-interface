using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server.Network
{
    class UDPNode
    {
        protected UdpClient client;

        

        public UDPNode(IPEndPoint ipPort)
        {
            client = new UdpClient(ipPort);
            Console.WriteLine("IP/Port: " + ipPort.Address.ToString() + "/" + ipPort.Port.ToString());
        }

        public async Task<byte[]> WaitMsg()
        {
            UdpReceiveResult data = await client.ReceiveAsync();
            Console.WriteLine(data.RemoteEndPoint.ToString() + ": " + 
                              Encoding.UTF8.GetString(data.Buffer, 0, data.Buffer.Length));
            return data.Buffer;
        }

        public async void SendMsg(byte[] data, IPEndPoint ipPort)
        {
            await client.SendAsync(data, data.Length, ipPort);
        }

        public void CloseServer()
        {
            client.Close();
        }

    }
}
