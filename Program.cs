using Lucraft.Database.Config;
using Newtonsoft.Json;
using System.IO;

namespace Lucraft.Database
{
    public static class Program
    {
        private static void Main(string[] args)
        {

            IEnvironment env = new ProductionEnvironment();

            if (args.Length > 0)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i].Equals("-e"))
                    {
                        if (i + 1 < args.Length && args[i+1].Equals("development"))
                        {
                            env = new DevelopmentEnvironment();
                        }
                    }
                }
            }

            // load configuration
            Startup startup = new Startup(JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(Directory.GetCurrentDirectory() + "/config.json")));
            startup.Configure(env: env);

            DatabaseServer.Instance.Start();
        }
    }
}
