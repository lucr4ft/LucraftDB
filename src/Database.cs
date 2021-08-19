using System.Collections.Generic;
using System.Linq;

namespace Lucraft.Database
{
    public class Database
    {
        public string Id { get; init; }
        public List<Collection> Collections { get; init; }

        public Collection GetCollection(string id) => Collections.FirstOrDefault(collection => collection.ID == id);
    }
}
