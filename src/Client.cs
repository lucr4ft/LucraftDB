﻿using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Lucraft.Database.Security.Authentication;

namespace Lucraft.Database
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class Client : IDisposable
    {
        public User User { get; init; }

        private readonly Socket Socket;
        private readonly StreamReader _reader;
        private readonly StreamWriter _writer;

        public EndPoint RemoteEndPoint { get; init; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        public Client(Socket socket)
        {
            Socket = socket;
            RemoteEndPoint = Socket.RemoteEndPoint;
            var stream = new NetworkStream(socket);
            _reader = new StreamReader(stream);
            _writer = new StreamWriter(stream) { AutoFlush = true };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ReadLine() => _reader.ReadLine();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<string> ReadLineAsync() => await _reader.ReadLineAsync().ConfigureAwait(false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        public void SendLine(string s) => _writer.Write($"{s}\n");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public async Task SendLineAsync(string s) => await _writer.WriteAsync($"{s}\n").ConfigureAwait(false);

        /// <summary>
        /// 
        /// </summary>
        public void Disconnect() => Disconnect("connection closed by server");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg">Disconnect Message</param>
        public void Disconnect(string msg)
        {
            SendLine(msg);
            SimpleLogger.Log(Level.Debug, $"Closing connection to [{ Socket.RemoteEndPoint}]");
            Socket.Shutdown(SocketShutdown.Both);
            Socket.Close();
            SimpleLogger.Log(Level.Debug, "Connection closed successful");
        }

        public void Dispose()
        {
            ((IDisposable)Socket).Dispose();
            ((IDisposable)_reader).Dispose();
            ((IDisposable)_writer).Dispose();
        }
    }
}
