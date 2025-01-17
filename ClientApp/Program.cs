using System.Net.Sockets;

namespace ClientApp
{
    internal class Program
    {
        static void ConnectServer(string server, int port)
        {
            string message, responseData;
            int bytes;
            try
            {
                TcpClient client = new TcpClient(server, port);
                Console.Title = "Client Application";
                Console.WriteLine($"Connected to server: {client.Client.RemoteEndPoint.ToString()}");
                NetworkStream stream = client.GetStream();
                while(true)
                {
                    Console.WriteLine("Input message <press enter to exite>:");
                    message = Console.ReadLine();
                    if(message == string.Empty)
                    {
                        break;
                    }
                    Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
                    stream = client.GetStream();
                    stream.Write(data, 0, data.Length);
                    Console.WriteLine("Sent: {0}", message);
                    data = new byte[256];
                    bytes = stream.Read(data, 0, data.Length);
                    responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                    Console.WriteLine("Received: {0}", responseData);
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception: {0}", ex.Message);
            }
        }
        static void Main(string[] args)
        {
            string server = "127.0.0.1";
            int port = 8080;
            ConnectServer(server, port);
        }
    }
}
