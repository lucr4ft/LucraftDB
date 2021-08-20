using Lucraft.Database.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

        private readonly object locker = new();

        private Dictionary<string, object> Data { get; set; }

        public Collection Parent { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        public Document(string filename)
        {
            lock (locker)
            {
                Filename = filename;
                if (!File.Exists(filename))
                {
                    File.Create(filename).Close();
                }
                else if (DatabaseServer.Config.DataOptions.AllowMemoryStorage)
                {
                    Data = JsonConvert.DeserializeObject<Dictionary<string, object>>(File.ReadAllText(Filename));
                }
                SimpleLogger.Log(Level.Debug, $"Loaded document: {filename}");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> GetData()
        {
            lock (locker)
            {
                if (DatabaseServer.Config.DataOptions.AllowMemoryStorage)
                {
                    return Data;
                }
                else
                {
                    return JsonConvert.DeserializeObject<Dictionary<string, object>>(File.ReadAllText(Filename));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DocumentResponseModel GetModel() => new() { Id = ID, Exists = true, Data = GetData() };
    }
}
