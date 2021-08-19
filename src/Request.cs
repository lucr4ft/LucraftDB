using Lucraft.Database.Query;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Lucraft.Database
{
    /// <summary>
    /// 
    /// </summary>
    public class Request
    {
        #region private static fields
        /// <summary>
        /// This Dictionary is a lookup table for parsing RequestTypes,
        /// because Enum.TryParse would be to slow for converting string to RequestType
        /// <br />
        /// see <a href="https://stackoverflow.com/questions/16100/convert-a-string-to-an-enum-in-c-sharp/38711#38711">this</a> for reference
        /// </summary>
        private static readonly Dictionary<string, RequestType> requestTypes;
        #endregion

        static Request()
        {
            // if a new RequestType is added it also must be added here!
            requestTypes = new Dictionary<string, RequestType>
            {
                ["get"] = RequestType.Get,
                ["set"] = RequestType.Set,
                ["list"] = RequestType.List,
                ["create"] = RequestType.Create,
                ["delete"] = RequestType.Delete
            };
        }

        private Request() { }

        #region public properties
        public RequestType Type { get; init; }
        public string Path { get; init; }
        public Condition Condition { get; init; }
        public string Data { get; init; }
        #endregion

        #region parse methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public static Request Parse(string req)
        {
            string[] split = req.Split(" ");
            
            if (split.Length < 2)
                throw new ArgumentException("Malformed Request #1");

            RequestType requestType = ParseRequestType(split[0]);
            string path = split[1][1..]; // remove the first '/'
            Condition condition = null;
            string data = null;

            if (split.Length > 2)
            {
                // Data, if specified
                if (split[2].Equals("?"))
                {
                    // split the request string at '?' and continue with the second part
                    string cond = req[req.IndexOf("?")..];
                    if (!ConditionParser.TryParse(cond, out condition))
                        throw new Exception("Malformed Query Condition #1");
                }
                else
                {
                    if (split.Length > 3)
                        throw new ArgumentException("Malformed Request #3");
                    data = req[(split[0].Length + path.Length + 3)..];
                    // validate json data
                    if (JsonConvert.DeserializeObject(data) is null)
                        throw new JsonException("invalid json #1");
                }
            }
            return new Request
            {
                Type = requestType,
                Path = path,
                Condition = condition,
                Data = data
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="req"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public static bool TryParse(string req, out Request request)
        {
            try
            {
                if ((request = Parse(req)) is null)
                    return false;
                return true;
            }
            catch (Exception)
            {
                request = null;
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rt"></param>
        /// <returns></returns>
        private static RequestType ParseRequestType(string rt)
        {
            rt = rt.ToLower();
            if (!requestTypes.ContainsKey(rt))
                throw new ArgumentException("Unknown RequestType #1");
            return requestTypes[rt];
        }
        #endregion

        public override string ToString()
        {
            return Type + ":" + Path + ":" + Condition + ":" + Data;
        }
    }
}
