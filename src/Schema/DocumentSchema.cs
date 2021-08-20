using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucraft.Database.Schema
{
    public class DocumentSchema
    {
        public FileInfo SchemaFile { get; set; }
        public JSchema Schema
        {
            get
            {
                return JSchema.Parse(File.ReadAllText(SchemaFile.FullName));
            }
        }
    }
}
