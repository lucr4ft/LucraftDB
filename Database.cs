using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Lucraft.Database
{
    public class Database
    {
        public string ID { get; set; }
        public List<Collection> Collections { get; set; }

        public Collection GetCollection(string id)
        {
            foreach (var collection in Collections)
            {
                if (collection.ID == id)
                    return collection;
            }
            return null;
        }

    }
}
