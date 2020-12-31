using Lucraft.Database.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Lucraft.Database
{
    public class Document
    {
        public string ID { get; init; }
        public string Filename { get; init; }

        private readonly object locker = new object();

        private Dictionary<string, object> Data { get; set; }

        public DocumentResponseModel GetModel() => new DocumentResponseModel { ID = ID, Exists = true, Data = GetData() };

        public Document(string filename)
        {
            lock (locker)
            {
                Filename = filename;
                if (!File.Exists(filename))
                    File.Create(filename).Close();
                else if (DatabaseServer.Config.DataOptions.AllowMemoryStorage)
                    Data = JsonConvert.DeserializeObject<Dictionary<string, object>>(File.ReadAllText(Filename));
                SimpleLogger.Log(Level.DEBUG, $"Loaded document: {filename}");
            }
        }

        public Dictionary<string, object> GetData()
        {
            lock (locker)
            {
                if (DatabaseServer.Config.DataOptions.AllowMemoryStorage)
                    return Data;
                else
                    return JsonConvert.DeserializeObject<Dictionary<string, object>>(File.ReadAllText(Filename)); // data;
            }
        }

        public void SetData(Dictionary<string, object> data) 
        {
            lock (locker)
            {
                if (DatabaseServer.Config.DataOptions.AllowMemoryStorage)
                {
                    Data = data;
                    new Task(() =>
                    {
                        File.WriteAllText(Filename, JsonConvert.SerializeObject(data));
                    }).Start();
                }
                else
                {
                    File.WriteAllText(Filename, JsonConvert.SerializeObject(data));
                }
            }
        }
    }
}
