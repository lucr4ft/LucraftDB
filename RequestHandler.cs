using Lucraft.Database.Models;
using Lucraft.Database.Query;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Lucraft.Database
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class RequestHandler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string HandleRequest(Request request)
        {
            ResponseModel response = null;

            string[] pathSplit = request.Path[1..].Split("/");

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

            switch (request.Type)
            {
                case RequestType.Get:
                    // deprecated: use RequestType.List instead
                    // will be removed in future version
                    // see RequestType.List
                    if (pathSplit[2] == "*")
                    {
                        // see RequestType.List
                        if (request.Condition is null)
                            response = collection.GetModel();
                        // see RequestType.List
                        else
                            response = QueryHandler.HandleQuery(collection, request.Condition);
                        break;
                    }
                    else if (document == null)
                        // document does not exist 
                        // -> return empty/non-existend document
                        response = new DocumentResponseModel { Id = pathSplit[2], Exists = false, Data = null };
                    else
                        // document exists
                        // -> return document model
                        response = document.GetModel();
                    break;
                case RequestType.List:
                    if (request.Condition is null)
                        // no condition is specified
                        // return model of the collection
                        response = collection.GetModel();
                    else
                        // request contains a condition
                        // -> evalute condition
                        response = QueryHandler.HandleQuery(collection, request.Condition);
                    break;
                case RequestType.Set:
                    // if document is null
                    // then create new document
                    document ??= collection.CreateDocument(pathSplit[2]);
                    // set data of the document
                    document.SetData(JsonConvert.DeserializeObject<Dictionary<string, object>>(request.Data));
// TODO: return Success-model + write-time
                    response = new ErrorResponseModel { Error = "ResponseModel not created yet!" };
                    break;
                case RequestType.Delete:
                    // delete document
                    // if an error occures during deletion
                    // error will contain the error
                    // else error will be null
                    collection.DeleteDocument(pathSplit[2], out string error);
                    response = new ErrorResponseModel { Error = error ?? "ResponseModel not created yet!" };
                    break;
                default:
                    break;
            }
            // return response model
            // if response is null 
            // then return new ErrorResponseModel
            return JsonConvert.SerializeObject(response ?? new ErrorResponseModel { 
                Error = "lucraft.database.exception", 
                ErrorMessage = "An unknown error occured handling the request #1" 
            });
        }
    }
}
