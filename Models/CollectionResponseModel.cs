using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucraft.Database.Model
{
    public class CollectionResponseModel
    {
        [JsonProperty("documents")]
        public List<DocumentResponseModel> Documents { get; init; }
    }
}
