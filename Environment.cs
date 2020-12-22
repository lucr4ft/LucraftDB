using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucraft.Database
{
    public class DevlopmentEnvironment : IEnvironment
    {
        public bool IsDevelopment()
        {
            return true;
        }
    }

    public class ProductionEnvironment : IEnvironment
    {
        public bool IsDevelopment()
        {
            return false;
        }
    }
}
