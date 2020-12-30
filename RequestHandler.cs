using Lucraft.Database.Models;
using Lucraft.Database.Query;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Lucraft.Database
{
    public class RequestHandler
    {
        public static string HandleRequest(string request)
        {
            string requestType = request.Split(" ")[0];
            ResponseModel response = new ErrorResponseModel { Error = "An error occurred handling the request!" };

            string path = request.Split(" ")[1];
            string[] pathSplit = path[1..].Split("/");

            // get database
            Database db = Databases.GetDatabase(pathSplit[0]);
            if (db == null) 
                return JsonConvert.SerializeObject(new ErrorResponseModel { Error = $"Database with ID {pathSplit[0]} does not exist" });

            // get collection
            Collection collection = db.GetCollection(pathSplit[1]);
            if (collection == null) 
                return JsonConvert.SerializeObject(new ErrorResponseModel { Error = $"Collection with ID {pathSplit[1]} does not exist" });

            // get document
            Document document = collection.GetDocument(pathSplit[2]);

            switch (requestType)
            {
                case "get":
                    if (pathSplit[2] == "*")
                    {
                        if (requestType.Length + path.Length + 1 == request.Length)
                            response = collection.GetModel();
                        else
                            response = QueryHandler.HandleQuery(
                                    collection: collection,
                                    query: request.Split(" where ")[1]);
                        break;
                    }
                    else if (document == null)
                        response = new DocumentResponseModel { ID = pathSplit[2], Exists = false, Data = null };
                    else
                        response = document.GetModel();//new DocumentModel { ID = document.ID, Exists = true, Data = document.GetData() };
                    break;
                case "set":
                    if (document == null) 
                        document = collection.CreateDocument(pathSplit[2]);
                    string data = request[(requestType.Length + path.Length + 2)..];
                    document.SetData(JsonConvert.DeserializeObject<Dictionary<string, object>>(data));
                    // Todo: return Success-model + write-time
                    response = new ErrorResponseModel { Error = "ResponseModel not created yet!" };
                    break;
                case "delete":
                    collection.DeleteDocument(pathSplit[2], out string error);
                    if (error != null)
                        response = new ErrorResponseModel { Error = error };
                    else
                        // TODO: return delete-time/success-model
                        response = new ErrorResponseModel { Error = "ResponseModel not created yet!" };
                    break;
                default:
                    response = new ErrorResponseModel { Error = $"{requestType} requests are currently not supported!" };
                    break;
            }
            return JsonConvert.SerializeObject(response);
        }
    }
}
