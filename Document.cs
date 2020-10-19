using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Lucraft.Database
{
    public class Document
    {
        [JsonProperty("id")]
        public string ID { get; set; }
        [JsonIgnore]
        public string Filename { get; set; }

        private readonly object locker = new object();

        [JsonProperty("data")]
        private Dictionary<string, object> Data => GetData();

        public Document(string filename)
        {
            lock (locker)
            {
                Filename = filename;
                if (!File.Exists(filename))
                {
                    File.Create(filename).Close();
                }
            }
            
        }

        public Dictionary<string, object> GetData()
        {
            lock (locker)
            {
                return JsonConvert.DeserializeObject<Dictionary<string, object>>(File.ReadAllText(Filename)); // data;
            }
        }

        public void SetData(Dictionary<string, object> data) 
        {
            lock (locker)
            {
                //this.data = data;
                File.WriteAllText(Filename, JsonConvert.SerializeObject(data));
            }
        }
    }
}
