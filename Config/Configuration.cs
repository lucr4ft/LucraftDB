using Newtonsoft.Json;

namespace Lucraft.Database.Config
{
    public class Configuration
    {
        [JsonProperty("port")]
        public int Port { get; init; }
        [JsonProperty("max-connections")]
        public int MaxConnections { get; init; }
        [JsonProperty("debug")]
        public bool Debug { get; init; }
        [JsonProperty("data-options")]
        public DataOptions DataOptions { get; init; }
    }
}
