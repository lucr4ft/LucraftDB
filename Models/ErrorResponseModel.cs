using Newtonsoft.Json;

namespace Lucraft.Database.Models
{
    public class ErrorResponseModel : ResponseModel
    {
        [JsonProperty("error")]
        public string Error { get; init; }
        [JsonProperty("error-message")]
        public string ErrorMessage { get; init; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
