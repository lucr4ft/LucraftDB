using Newtonsoft.Json;

namespace Lucraft.Database.Models
{
    public class ErrorResponseModel
    {
        [JsonProperty("error")]
        public string Error { get; init; }
    }
}
