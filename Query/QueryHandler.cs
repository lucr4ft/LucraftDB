using Lucraft.Database.Models;
using System.Collections.Generic;

namespace Lucraft.Database.Query
{
    public class QueryHandler
    {
        public static QueryResponseModel HandleQuery(Collection collection, string query)
        {
            Condition condition = Condition.GetCondition(query);
            SimpleLogger.Log(Level.DEBUG, condition.ToString());
            List<DocumentResponseModel> matchingDocs = new List<DocumentResponseModel>();
            collection.Documents.ForEach(delegate (Document doc)
            {
                if (condition.Check(doc)) 
                    matchingDocs.Add(doc.GetModel());
            });
            return new QueryResponseModel { Documents = matchingDocs };
        }
    }
}
