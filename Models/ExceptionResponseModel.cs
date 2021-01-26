using Newtonsoft.Json;

namespace Lucraft.Database.Models
{
    public class ExceptionResponseModel
    {
        [JsonProperty("exception")]
        public string Exception { get; init; }
        [JsonProperty("exception-message")]
        public string ExceptionMessage { get; init; }
    }
}
