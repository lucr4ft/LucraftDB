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
            return JsonConvert.SerializeObject(request.Type switch
            {
                RequestType.Get => HandleGetRequest(request),
                RequestType.Set => HandleSetRequest(request),
                RequestType.List => HandleListRequest(request),
                RequestType.Create => HandleCreateRequest(request),
                RequestType.Delete => HandleDeleteRequest(request),
                _ => new ErrorResponseModel
                {
                    Error = "lucraft.database.exception",
                    ErrorMessage = "An unknown error occured handling the request #1"
                }
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private static ResponseModel HandleGetRequest(Request request)
        {
            string[] pathSplit = request.Path.Split("/");

            // get database
            Database db = Databases.GetDatabase(pathSplit[0]);
            if (db == null)
                return new ErrorResponseModel { Error = $"Database with ID {pathSplit[0]} does not exist" };

            // get collection
            Collection collection = db.GetCollection(pathSplit[1]);
            if (collection == null)
                return new ErrorResponseModel { Error = $"Collection with ID {pathSplit[1]} does not exist" };

            Document document;
            if ((document = collection.GetDocument(pathSplit[2])) != null)
            {
                return document.GetModel();
            } 
            else
            {
                // document does not exist 
                // -> return empty/non-existend document
                return new DocumentResponseModel { Id = pathSplit[2], Exists = false, Data = null };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private static ResponseModel HandleSetRequest(Request request)
        {
            string[] pathSplit = request.Path.Split("/");

            // get database
            Database db = Databases.GetDatabase(pathSplit[0]);
            if (db == null)
                return new ErrorResponseModel { Error = $"Database with ID {pathSplit[0]} does not exist" };

            // get collection
            Collection collection = db.GetCollection(pathSplit[1]);
            if (collection == null)
                return new ErrorResponseModel { Error = $"Collection with ID {pathSplit[1]} does not exist" };

            // get document
            Document document = collection.GetDocument(pathSplit[2]);

            // if document is null
            // then create new document
            document ??= collection.CreateDocument(pathSplit[2]);
            // set data of the document
            document.SetData(JsonConvert.DeserializeObject<Dictionary<string, object>>(request.Data));
            // TODO: return Success-model + write-time
            return new ErrorResponseModel { Error = "ResponseModel not created yet!" };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private static ResponseModel HandleListRequest(Request request)
        {
            string[] pathSplit = request.Path.Split("/");

            Database db = Databases.GetDatabase(pathSplit[0]);
            if (db == null)
                return new ErrorResponseModel { Error = $"Database with ID {pathSplit[0]} does not exist" };

            // get collection
            Collection collection = db.GetCollection(pathSplit[1]);
            if (collection == null)
                return new ErrorResponseModel { Error = $"Collection with ID {pathSplit[1]} does not exist" };

            if (request.Condition is null)
                // no condition is specified
                // return model of the collection
                return collection.GetModel();
            else
                // request contains a condition
                // -> evalute condition
                return QueryHandler.HandleQuery(collection, request.Condition);
        }

        private static ResponseModel HandleCreateRequest(Request request)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private static ResponseModel HandleDeleteRequest(Request request)
        {
            string[] pathSplit = request.Path.Split("/");

            // get database
            Database db = Databases.GetDatabase(pathSplit[0]);
            if (db == null)
                return new ErrorResponseModel { Error = $"Database with ID {pathSplit[0]} does not exist" };

            // get collection
            Collection collection = db.GetCollection(pathSplit[1]);
            if (collection == null)
                return new ErrorResponseModel { Error = $"Collection with ID {pathSplit[1]} does not exist" };

            // delete document
            // if an error occures during deletion
            // error will contain the error
            // else error will be null
            collection.DeleteDocument(pathSplit[2], out string error);
            return new ErrorResponseModel { Error = error ?? "ResponseModel not created yet!" };
        }
    }
}
