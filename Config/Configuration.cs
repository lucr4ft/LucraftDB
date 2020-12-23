using System.Text.Json.Serialization;

namespace Lucraft.Database.Config
{
    public class Configuration
    {
        public int Port { get; init; }
        [JsonPropertyName("max-connections")]
        public int MaxConnections { get; init; }
        public bool Debug { get; init; }
        [JsonPropertyName("data-options")]
        public DataOptions DataOptions { get; init; }
    }
}
