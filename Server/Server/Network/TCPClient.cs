using System;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

namespace Server.Network
{
    public class TCPClient
    {
        protected TcpClient client;
        protected NetworkStream stream;
        protected bool connect = false; 

        protected TCPClient() { }
        public TCPClient(IPEndPoint ipPort) => Start(ipPort);
        private async void Start(IPEndPoint ipPort)
        {
            try
            {
                Console.WriteLine("Подключение к серверу... ");
                client = new TcpClient();
                await client.ConnectAsync(ipPort.Address, ipPort.Port);
                stream = client.GetStream();
                connect = true;
                Console.WriteLine("Клиент подключён");
            }
            catch (Exception e)
            {
                Console.WriteLine("Fail connection: {0} ", e.Message);
            }
        }

        public async Task<byte[]> WaitMsg()
        {
            byte[] data = null;

            while(connect == false)
            {
                Thread.Sleep(1);
            }

            try
            {
                byte[] buff = new byte[256];

                do
                {
                    int bytes = await stream.ReadAsync(buff, 0, buff.Length);


                    data = new byte[bytes];

                    for (int i = 0; i < bytes; i++)
                    {
                        data[i] = buff[i];
                    }

                    if (data.Length == 0)
                    {
                        Close();
                        return null;
                    }

                    //Console.WriteLine(Encoding.UTF8.GetString(data, 0, data.Length) + " " + data.Length.ToString() + " byte");
                }
                while (stream.DataAvailable); // пока данные есть в потоке
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0} ", e);
                Close();
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine("SocketException: {0} ", e.Message);
            }

            return data;
        }

        public async void SendMsg(byte[] data)
        {
            try
            {
                await stream.WriteAsync(data, 0, data.Length);
                //Console.WriteLine("Отправлено сообщение: {0}", Encoding.UTF8.GetString(data));
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0} ", e);
            }
            catch (Exception e)
            {
                Console.WriteLine("SocketException: {0} ", e.Message);
            }
        }

        public void Close()
        {
            connect = false;
            stream.Close();
            client.Close();
        }
    }
}
