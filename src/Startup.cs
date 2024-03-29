using Lucraft.Database.Config;

namespace Lucraft.Database
{
    public class Startup
    {
        public Startup(Configuration configuration)
        {
            Configuration = configuration;
        }

        private Configuration Configuration { get; }

        public void Configure(IEnvironment env)
        {
            SimpleLogger.Debug = Configuration.Debug || env.IsDevelopment();
        }
    }
}