using NuGet.Versioning;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Lucraft.Database
{
    public class SocketServer
    {
        private readonly ManualResetEvent allDone = new(false);
        private readonly ClientHandler ClientHandler = new();

        public void Start(int port)
        {
            SimpleLogger.Log(Level.Info, "Starting server....");
            IPEndPoint localEndPoint = new(IPAddress.Any, port);
            // Create a TCP/IP socket.  
            Socket listener = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // Bind the socket to the local endpoint and listen for incoming connections.  
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);
                SimpleLogger.Log(Level.Info, "Server started successful on port " + port);
                while (true)
                {
                    // Set the event to nonsignaled state.  
                    allDone.Reset();
                    // Start an asynchronous socket to listen for connections.  
                    SimpleLogger.Log(Level.Info, "Waiting for a client to connect...");
                    listener.BeginAccept(new AsyncCallback(AcceptClient), listener);
                    // Wait until a connection is made before continuing.  
                    allDone.WaitOne();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.Read();
        }

        public void Shutdown()
        {
            ClientHandler.DisconnectAll("Shutting down server");
        }

        public void AcceptClient(IAsyncResult ar)
        {
            // Signal the main thread to continue.  
            allDone.Set();
            Thread.CurrentThread.Name = "ClientThread#" + DateTimeOffset.Now.ToUnixTimeMilliseconds();
            // Get the socket that handles the client request.  
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);
            SimpleLogger.Log(Level.Info, $"Client connected from {handler.RemoteEndPoint}");
            ClientHandler.Add(new Client(handler));
        }
    }
}
