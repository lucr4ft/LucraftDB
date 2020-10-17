using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Lucraft.Database
{
    public class DatabaseServer
    {
        public static readonly string ROOT_PATH = Directory.GetCurrentDirectory();
        public static readonly Config Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(ROOT_PATH + "\\config.json"));

        public static readonly DatabaseServer Instance = new DatabaseServer();

        private readonly SocketServer SocketServer;
        public List<Database> databases;

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
