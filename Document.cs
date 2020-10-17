using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        private Dictionary<string, object> data;

        public Document(string filename)
        {
            lock (locker)
            {
                if (!File.Exists(filename))
                {
                    File.Create(filename).Close();
                } 
                else
                {
                    data = JsonConvert.DeserializeObject<Dictionary<string, object>>(File.ReadAllText(filename));
                }
            }
            Filename = filename;
        }

        public Dictionary<string, object> GetData()
        {
            lock (locker)
            {
                return data;//JsonConvert.DeserializeObject<Dictionary<string, object>>(File.ReadAllText(Filename));
            }
        }

        public void SetData(Dictionary<string, object> data) 
        {
            lock (locker)
            {
                this.data = data;
                File.WriteAllText(Filename, JsonConvert.SerializeObject(data));
            }
        }

    }
}
