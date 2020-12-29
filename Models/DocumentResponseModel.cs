using Newtonsoft.Json;
using System.Collections.Generic;

namespace Lucraft.Database.Models
{
    public class DocumentResponseModel
    {
        [JsonProperty("id")]
        public string ID { get; init; }
        [JsonProperty("exists")]
        public bool Exists { get; init; }
        [JsonProperty("data")]
        public Dictionary<string, object> Data { get; init; }
    }
}
