using Lucraft.Database.Models;
using NuGet.Versioning;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Lucraft.Database
{
    /// <summary>
    /// 
    /// </summary>
    public static class ClientHandler
    {
        private static readonly List<Client> Clients = new();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static async Task Add(Client client)
        {
            Thread.CurrentThread.Name = "ClientThread#" + DateTimeOffset.Now.ToUnixTimeMilliseconds();
            if (await Authenticate(client).ConfigureAwait(false))
            {
                Clients.Add(client);
                _ = Task.Run(() => StartListening(client).ConfigureAwait(false));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        private static async Task<bool> Authenticate(Client client)
        {
            string clientData = await client.ReadLineAsync();
            var metadata = new Dictionary<string, string>();

            foreach (string line in clientData.Split(";"))
            {
                metadata.Add(line.Split("=")[0], line.Split("=")[1]);
            }

            SemanticVersion clientVersion = SemanticVersion.Parse(metadata["version"]);

            if (clientVersion < DatabaseServer.MinimumClientVersion)
            {
                client.Disconnect(new ErrorResponseModel
                {
                    Error = "lucraft.database.exception.outdated_client",
                    ErrorMessage = $"client needs AT LEAST version {DatabaseServer.MinimumClientVersion} to connect to this server"
                }.ToString());
                client.Dispose();
                return false;
            }
            else
            {
                await client.SendLineAsync("{}");
                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        private static async Task StartListening(Client client)
        {
            var stopwatch = new Stopwatch();
            while (true)
            {
                string req = await client.ReadLineAsync();
                SimpleLogger.Log(Level.Info, $"Read {req.Length} bytes from {client.RemoteEndPoint}.");
                SimpleLogger.Log(Level.Debug, $"Data read : {req}");
                stopwatch.Start();
                string res = Request.TryParse(req, out Request request)
                    ? RequestHandler.HandleRequest(request)
                    : new ErrorResponseModel { Error = "lucraft.database.exception.malformed_request", ErrorMessage = "Malformed Request #1" }.ToString();
                stopwatch.Stop();
                SimpleLogger.Log(Level.Debug, $"Elapsed Time: {(decimal)stopwatch.ElapsedTicks / 10_000} milliseconds");
                await client.SendLineAsync(res);
                SimpleLogger.Log(Level.Debug, $"Data sent : {res}");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void DisconnectAll() => Clients.ForEach(c =>
        {
            c.Disconnect();
            c.Dispose();
        });

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public static void DisconnectAll(string msg) => Clients.ForEach(c =>
        {
            c.Disconnect(msg);
            c.Dispose();
        });
    }
}
