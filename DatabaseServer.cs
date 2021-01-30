using Lucraft.Database.Config;
using Newtonsoft.Json;
using NuGet.Versioning;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Lucraft.Database
{
    public class DatabaseServer
    {
        public static readonly string ROOT_PATH = Directory.GetCurrentDirectory();
        public static readonly Configuration Config = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(ROOT_PATH + "/config.json"));

        public static readonly SemanticVersion MinimumClientVersion = SemanticVersion.Parse("1.1.1");

        public static readonly DatabaseServer Instance = new();

        private readonly SocketServer SocketServer;

        private DatabaseServer()
        {
            Databases.Load();
            SocketServer = new SocketServer();
        }

        public void Start()
        {
            new Thread(() =>
            {
                Thread.CurrentThread.Name = "SocketServer-Thread";
                SocketServer.Start(Config.Port);
            }).Start();
        }

        public void Shutdown()
        {
            SocketServer.Shutdown();
        }
    }
}
