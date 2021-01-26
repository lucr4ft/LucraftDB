using Lucraft.Database.Models;
using System.Collections.Generic;
using System.Linq;
using static Lucraft.Database.Level;

namespace Lucraft.Database.Query
{
    public static class QueryHandler
    {
        public static QueryResponseModel HandleQuery(Collection collection, string query)
        {
            //Condition condition = Condition.GetCondition(query);
            Condition condition = ConditionParser.GetCondition(query);
            SimpleLogger.Log(Debug, condition.ToString());
            List<DocumentResponseModel> matchingDocs = 
                (from document 
                        in collection.Documents 
                        where condition.Check(document)
                        select document.GetModel()).ToList();

            return new QueryResponseModel { Documents = matchingDocs };
        }
    }
}
