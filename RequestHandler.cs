using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Lucraft.Database
{
    public class RequestHandler
    {

        //public string HandleRequest(Dictionary<string, object> request)
        //{
        //    var response = new Dictionary<string, object>();
        //    Dictionary<string, object> options = JsonConvert.DeserializeObject<Dictionary<string, object>>(request["options"].ToString());
        //    string requestType = (string)options["request-type"];

        //    Database db = Databases.GetDatabase(request["database"].ToString());
        //    if (db == null)
        //    {
        //        response["error"] = "Database with ID " + request["database"].ToString() + " does not exist";
        //        return JsonConvert.SerializeObject(response);
        //    }
        //    Collection collection = db.GetCollection(request["collection"].ToString());
        //    if (db == null)
        //    {
        //        response["error"] = "Collection with ID " + request["collection"].ToString() + " does not exist";
        //        return JsonConvert.SerializeObject(response);
        //    }
        //    Document document = collection.GetDocument(request["document"].ToString());

        //    if (requestType == "get")
        //    {
        //        if (request["document"].ToString() == "*")
        //        {
        //            Document[] documents = collection.Documents.ToArray();
        //            response["documents"] = documents;
        //        }
        //        else if (document == null)
        //        {
        //            response["exists"] = false;
        //            response["data"] = null;
        //        } else
        //        {
        //            response["exists"] = true;
        //            response["data"] = document.GetData();
        //        }
        //    }
        //    else if (requestType == "set")
        //    {
        //        if (document == null)
        //        {
        //            document = collection.CreateDocument(request["document"].ToString());
        //        }
        //        document.SetData(JsonConvert.DeserializeObject<Dictionary<string, object>>(request["data"].ToString()));
        //    }
        //    else if (requestType == "delete")
        //    {
        //        Console.WriteLine("delete");
        //        if (!collection.DeleteDocument(request["document"].ToString(), out string error))
        //        {
        //            response["error"] = error;
        //        }
        //    }
        //    else if (requestType == "query")
        //    {
        //        string query = (string)options["query-string"];
        //        Document[] documents = collection.Documents.ToArray();
        //        List<Document> docs = new List<Document>();
        //        foreach (var doc in documents)
        //        {
        //            var data = doc.GetData();
        //            if (data.ContainsKey("name") && (string) data["name"] == "Daytimer")
        //            {
        //                docs.Add(doc);
        //            }
        //        }
        //        response["documents"] = docs.ToArray();
        //    }
        //    else
        //    {
        //        throw new NotSupportedException(requestType + " request are currenty not supported!");
        //    }
        //    return JsonConvert.SerializeObject(response);
        //}

        public string HandleRequest(string request)
        {
            string requestType = request.Split(" ")[0];
            Dictionary<string, object> response = new Dictionary<string, object>();

            string path = request.Split(" ")[1];
            string[] pathSplit = path.Substring(1).Split("/");

            Database db = Databases.GetDatabase(pathSplit[0]);
            if (db == null)
            {
                response["error"] = "Database with ID " + pathSplit[0] + " does not exist";
                return JsonConvert.SerializeObject(response);
            }
            Collection collection = db.GetCollection(pathSplit[1]);
            if (collection == null)
            {
                response["error"] = "Collection with ID " + pathSplit[1] + " does not exist";
                return JsonConvert.SerializeObject(response);
            }
            Document document = collection.GetDocument(pathSplit[2]);

            switch (requestType)
            {
                case "get":
                    if (pathSplit[2] == "*")
                    {
                        Document[] documents = collection.Documents.ToArray();
                        response["documents"] = documents;
                    }
                    else if (document == null)
                    {
                        response["exists"] = false;
                        response["data"] = null;
                    }
                    else
                    {
                        response["id"] = document.ID;
                        response["exists"] = true;
                        response["data"] = document.GetData();
                    }
                    break;
                case "set":
                    if (document == null) 
                        document = collection.CreateDocument(pathSplit[2]);
                    string s = request.Substring(requestType.Length + path.Length + 2);
                    document.SetData(JsonConvert.DeserializeObject<Dictionary<string, object>>(s));
                    break;
                case "delete":
                    collection.DeleteDocument(pathSplit[2], out string error);
                    response["error"] = error;
                    break;
                default:
                    response["error"] = requestType + " requests are currently not supported!";
                    break;
                    //throw new NotSupportedException(requestType + " requests are currently not supported!");
            }

            return JsonConvert.SerializeObject(response);
        }

        private string HandleQuery(string query)
        {

            return "";
        }

    }
}
