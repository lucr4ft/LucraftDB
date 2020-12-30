using Newtonsoft.Json;
using System.Collections.Generic;

namespace Lucraft.Database.Models
{
    public class DocumentResponseModel : ResponseModel
    {
        [JsonProperty("id")]
        public string ID { get; init; }
        [JsonProperty("exists")]
        public bool Exists { get; init; }
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, object> Data { get; init; }
    }
}
