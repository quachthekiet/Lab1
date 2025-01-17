using System.Net;
using System.Net.Sockets;

namespace ServerApp
{
    internal class Program
    {
        static void ProcessMessage(object parm)
        {
            string data;
            int count;

            try
            {
                TcpClient client = parm as TcpClient;

                // Buffer for reading data
                Byte[] bytes = new Byte[256];

                // Get a stream object for reading and writing
                NetworkStream stream = client.GetStream();

                // Loop to receive all the data sent by the client.
                while((count = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    // Translate data bytes to a ASCII string.
                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, count);
                    string clientInfo = client.Client.RemoteEndPoint.ToString();

                    Console.WriteLine($"{clientInfo}, Received: {data} at {DateTime.Now:t}");

                    // Process the data sent by the client.
                    data = data.ToUpper();
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                    // Send back a response.
                    stream.Write(msg, 0, msg.Length);
                    Console.WriteLine($"Sent: {data}");
                }

                // Shutdown and end connection
                client.Close();
            }
            catch(Exception ex)
            {
                Console.WriteLine("{0}", ex.Message);
                Console.WriteLine("Waitting message...");
            }
        }

        static void ExcuteServer(string host, int port)
        {
            int Count = 0;
            TcpListener server = null;

            try
            {
                Console.Title = "Server Application";
                IPAddress localAddr = IPAddress.Parse(host);

                server = new TcpListener(localAddr, port);

                // Start listening for client requests.
                server.Start();
                Console.WriteLine(new string('*', 40));
                Console.WriteLine("Waiting for a connection...");

                // Enter the listening loop.
                while(true)
                {
                    // Perform a blocking call to accept requests.
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine($"Number of clients connected: {++Count}");
                    Console.WriteLine(new string('*', 40));

                    // Create a thread to receive and send messages.
                    Thread thread = new Thread(new ParameterizedThreadStart(ProcessMessage));
                    thread.Start(client);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            finally
            {
                server?.Stop();
                Console.WriteLine("Servier stopped. Press any key to exit !");
            }
            Console.Read();
        }
        static void Main(string[] args)
        {
            string host = "127.0.0.1";
            int port = 8080;
            ExcuteServer(host, port);
        }
    }
}
