using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lucraft.Database
{
    public class Database
    {
        public string Id { get; init; }
        public List<Collection> Collections { get; init; }

        public Collection GetCollection(string id)
        {
            return Collections.FirstOrDefault(collection => collection.ID == id);
        }

    }
}
