using System;
using System.Collections.Generic;
using System.Text;

namespace Lucraft.Database
{
    public class Startup
    {
        public Startup(Config config)
        {
            Configuration = config;
        }

        public Config Configuration { get; }

        public void Configure(IEnvironment env)
        {
            if (env.IsDevelopment()) { }
            SimpleLogger.Debug = Configuration.Debug || env.IsDevelopment();
        }
    }
}
