using Lucraft.Database.Config;

namespace Lucraft.Database
{
    public class Startup
    {
        public Startup(Configuration configuration)
        {
            Configuration = configuration;
        }

        public Configuration Configuration { get; }

        public void Configure(IEnvironment env)
        {
            if (env.IsDevelopment()) { }
            SimpleLogger.Debug = Configuration.Debug || env.IsDevelopment();
        }
    }
}
