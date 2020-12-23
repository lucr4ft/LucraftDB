using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Lucraft.Database.Config
{
    public class DataOptions
    {
        [JsonPropertyName("allow-memory-storage")]
        public bool AllowMemoryStorage { get; init; }
        [JsonPropertyName("allow-async-read")]
        public bool AllowAsyncRead { get; init; }
        [JsonPropertyName("allow-async-write")]
        public bool AllowAsyncWrite { get; init; }
    }
}
