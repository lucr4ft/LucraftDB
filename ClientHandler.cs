using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Lucraft.Database
{
    public class ClientHandler
    {
        private readonly List<Client> ConnectedClients;

        public ClientHandler()
        {
            ConnectedClients = new List<Client>();
        }

        #region Utility Methods

        public void Add(Client client)
        {
            ConnectedClients.Add(client);
            client.Socket.BeginReceive(client.Buffer, 0, Client.BufferSize, 0, new AsyncCallback(ReadCallback), client);
        }

        public void DisconnectAll()
        {
            foreach (var client in ConnectedClients)
            {
                client.Disconnect();
            }
        }

        public void DisconnectAll(string msg)
        {
            foreach (var client in ConnectedClients)
            {
                client.Disconnect(msg);
            }
        }

        #endregion

        #region Send / Recive

        public void ReadCallback(IAsyncResult ar)
        {
            string content = string.Empty;

            // Retrieve the state object and the handler socket  
            // from the asynchronous state object.  
            Client client = (Client)ar.AsyncState;
            Socket handler = client.Socket;

            // Read data from the client socket.
            int bytesRead;
            try
            {
                bytesRead = handler.EndReceive(ar);
            }
            catch (Exception)
            {
                SimpleLogger.Log(Level.WARN, "Connection was interrupted");
                ConnectedClients.Remove(client);
                return;
            }

            if (bytesRead > 0)
            {
                client.StringBuilder.Append(Encoding.UTF8.GetString(client.Buffer, 0, bytesRead));
                content = client.StringBuilder.ToString();
                if (content.IndexOf("\u0017") > -1)
                {
                    content = content[0..^1];
                    if (content.Equals("\u0004"))
                    {
                        string remoteEndPoint = client.Socket.RemoteEndPoint.ToString();
                        SimpleLogger.Log(Level.INFO, $"Closing connetion to {remoteEndPoint}");
                        client.Socket.Shutdown(SocketShutdown.Both);
                        client.Socket.Close();
                        ConnectedClients.Remove(client);
                        SimpleLogger.Log(Level.INFO, $"Closed connetion to {remoteEndPoint}");
                        return;
                    }
                    // All the data has been read from the client. Display it on the console.  
                    SimpleLogger.Log(Level.INFO, $"Read {content.Length} bytes from {client.Socket.RemoteEndPoint}.");
                    SimpleLogger.Log(Level.DEBUG, $"Data read : {content}");
                    string response = new RequestHandler().HandleRequest(content);
                    SimpleLogger.Log(Level.DEBUG, $"Data sent : {response}");
                    // send response to client
                    Send(handler, response + "\n");

                    // remove data from cache
                    client.StringBuilder.Clear();
                }
                client.Socket.BeginReceive(client.Buffer, 0, Client.BufferSize, 0, new AsyncCallback(ReadCallback), client);
            }
        }

        private void Send(Socket handler, string data)
        {
            byte[] byteData = Encoding.UTF8.GetBytes(data + "\n");
            handler.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), handler);
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            { 
                Socket handler = (Socket)ar.AsyncState;
                int bytesSent = handler.EndSend(ar);
                SimpleLogger.Log(Level.INFO, $"Sent {bytesSent} bytes to client.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        #endregion
    }
}
