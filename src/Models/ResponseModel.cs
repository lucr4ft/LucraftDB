using Newtonsoft.Json;

namespace Lucraft.Database.Models
{
    public abstract class ResponseModel {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
