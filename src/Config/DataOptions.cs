using Newtonsoft.Json;

namespace Lucraft.Database.Config
{
    public class DataOptions
    {
        [JsonProperty("allow-memory-storage")]
        public bool AllowMemoryStorage { get; init; }
    }
}
