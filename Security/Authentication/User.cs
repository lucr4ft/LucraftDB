using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucraft.Database.Security.Authentication
{
    public sealed class User
    {
        public string ID { get; init; }
        public Credentials Credentials { get; init; }
    }
}
