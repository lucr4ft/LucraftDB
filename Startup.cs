using System;
using System.Collections.Generic;
using System.Text;

namespace Lucraft.Database
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

    }
}
