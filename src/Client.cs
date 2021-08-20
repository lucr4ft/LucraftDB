using System;
using System.IO;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        public Client(Socket socket)
        {
            Socket = socket;
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
            Console.WriteLine($"[{Thread.CurrentThread.Name}{DateTime.Now}] Closing connection to [{ Socket.RemoteEndPoint}]");
            Socket.Shutdown(SocketShutdown.Both);
            Socket.Close();
            Console.WriteLine($"[{Thread.CurrentThread.Name}{DateTime.Now}] Connection closed successful");
        }

        public void Dispose()
        {
            ((IDisposable)Socket).Dispose();
            ((IDisposable)_reader).Dispose();
            ((IDisposable)_writer).Dispose();
        }
    }
}
