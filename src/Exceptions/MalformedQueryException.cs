using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lucraft.Database.Exceptions
{
    public class MalformedQueryException : Exception
    {
        public MalformedQueryException() { }

        public MalformedQueryException(string message) : base(message) { }

        public MalformedQueryException(string message, Exception innerException) : base(message, innerException) { }
    }
}
