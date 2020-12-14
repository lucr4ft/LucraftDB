using Lucraft.Database.Query;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Lucraft.Database
{
    public class RequestHandler
    {

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
                        if (requestType.Length + path.Length + 1 == request.Length)
                        {
                            Document[] documents = collection.Documents.ToArray();
                            response["documents"] = documents;
                        }
                        else
                        {
                            // handle query
                            string query = request.Split(" where ")[1];
                            Condition matchCondition = Condition.GetCondition(query);

                            Console.WriteLine(matchCondition.ToString());

                            Document[] documents = collection.Documents.ToArray();
                            List<Document> matchingDocuments = new List<Document>();

                            foreach (Document doc in documents)
                                if (matchCondition.Check(doc)) 
                                    matchingDocuments.Add(doc);

                            response["documents"] = matchingDocuments.ToArray();
                        }
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
                    string s = request[(requestType.Length + path.Length + 2)..];
                    document.SetData(JsonConvert.DeserializeObject<Dictionary<string, object>>(s));
                    break;
                case "delete":
                    collection.DeleteDocument(pathSplit[2], out string error);
                    response["error"] = error;
                    break;
                default:
                    response["error"] = requestType + " requests are currently not supported!";
                    break;
            }

            return JsonConvert.SerializeObject(response);
        }

    }
}
