using Newtonsoft.Json;

namespace Lucraft.Database.Config
{
    public class DataOptions
    {
        [JsonProperty("allow-memory-storage")]
        public bool AllowMemoryStorage { get; init; }
        [JsonProperty("allow-async-read")]
        public bool AllowAsyncRead { get; init; }
        [JsonProperty("allow-async-write")]
        public bool AllowAsyncWrite { get; init; }
    }
}
