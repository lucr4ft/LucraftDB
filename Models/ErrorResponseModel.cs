using Newtonsoft.Json;

namespace Lucraft.Database.Models
{
    public class ErrorResponseModel : ResponseModel
    {
        [JsonProperty("error")]
        public string Error { get; init; }
    }
}
