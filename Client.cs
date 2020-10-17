using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Lucraft.Database
{

    public class Client
    {

        public Socket Socket { get; }

        public const int BufferSize = 1024;
        public byte[] Buffer = new byte[BufferSize];
        public StringBuilder StringBuilder = new StringBuilder();

        public Client(Socket handler)
        {
            Socket = handler;
        }

        public void Disconnect(string msg)
        {
            Send(msg);
            Console.WriteLine($"[{Thread.CurrentThread.Name}{DateTime.Now}] Closing connection to [{ Socket.RemoteEndPoint}]");
            Socket.Shutdown(SocketShutdown.Both);
            Socket.Close();
            Console.WriteLine($"[{Thread.CurrentThread.Name}{DateTime.Now}] Connection closed successful");
        }

        public void Disconnect()
        {
            Disconnect("connection closed by server");
        }

        private void Send(string data)
        {
            byte[] byteData = Encoding.Unicode.GetBytes(data);
            Socket.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), null);
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                int bytesSent = Socket.EndSend(ar);
                SimpleLogger.Log(Level.INFO, $"Sent {bytesSent} bytes to client.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

    }
}
