using Lucraft.Database.Config;
using Newtonsoft.Json;
using NuGet.Versioning;
using System.IO;
using System.Threading;

namespace Lucraft.Database
{
    /// <summary>
    /// 
    /// </summary>
    public class DatabaseServer
    {
        public static readonly string ROOT_PATH = Directory.GetCurrentDirectory();
        public static readonly Configuration Config = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(ROOT_PATH + "/config.json"));
        public static readonly SemanticVersion MinimumClientVersion = SemanticVersion.Parse("2.0.0-rc.1");
        public static readonly DatabaseServer Instance = new();

        private readonly SocketServer socketServer = new();

        /// <summary>
        /// 
        /// </summary>
        private DatabaseServer()
        {
            Databases.Load();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            new Thread(() =>
            {
                Thread.CurrentThread.Name = "SocketServer-Thread";
                socketServer.Start();
            }).Start();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Shutdown()
        {
            socketServer.Shutdown();
        }
    }
}
