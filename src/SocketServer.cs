using NuGet.Versioning;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lucraft.Database
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SocketServer
    {
        private bool running = false;

        /// <summary>
        /// 
        /// </summary>
        private readonly IPEndPoint localEndPoint = new(IPAddress.Any, DatabaseServer.Config.Port);

        /// <summary>
        /// 
        /// </summary>
        private readonly Socket listener = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            if (running)
                throw new Exception("Server is already runnig!");
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);

                running = true;

                SimpleLogger.Log(Level.Info, "Server started successful on port " + DatabaseServer.Config.Port);

                while (true)
                {
                    SimpleLogger.Log(Level.Info, "Waiting for a client to connect...");
                    // Program is suspended while waiting for an incoming connection.  
                    Socket handler = listener.Accept();

                    var client = new Client(handler);
                    Task.Run(async () => await ClientHandler.Add(client));
                }
            }
            catch (Exception e)
            {
                SimpleLogger.Log(Level.Error, e.ToString());
            }
        }

        public void Shutdown()
        {
            if (!running)
                throw new Exception("Server is not runnig");
            SimpleLogger.Log(Level.Info, "Stopping server...");
            ClientHandler.DisconnectAll("Stopping server");
            listener.Shutdown(SocketShutdown.Both);
            listener.Close();
            SimpleLogger.Log(Level.Info, "Stopped server");
            Environment.Exit(0);
        }
    }
}
