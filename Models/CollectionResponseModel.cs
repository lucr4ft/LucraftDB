using Newtonsoft.Json;
using System.Collections.Generic;

namespace Lucraft.Database.Models
{
    public class CollectionResponseModel : ResponseModel
    {
        [JsonProperty("id")]
        public string ID { get; init; }
        [JsonProperty("documents")]
        public List<DocumentResponseModel> Documents { get; init; }
    }
}
