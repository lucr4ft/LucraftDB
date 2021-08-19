using Newtonsoft.Json;
using System.Collections.Generic;

namespace Lucraft.Database.Models
{
    public class QueryResponseModel : ResponseModel
    {
        [JsonProperty("documents")]
        public List<DocumentResponseModel> Documents { get; init; }
    }
}
