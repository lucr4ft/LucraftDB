using Lucraft.Database.Models;
using System.Collections.Generic;
using System.Linq;
using static Lucraft.Database.Level;

namespace Lucraft.Database.Query
{
    /// <summary>
    /// 
    /// </summary>
    public static class QueryHandler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static QueryResponseModel HandleQuery(Collection collection, Condition condition)
        {
            SimpleLogger.Log(Debug, condition.ToString());
            List<DocumentResponseModel> matchingDocs = (from document
                                                        in collection.Documents
                                                        where condition.Check(document)
                                                        select document.GetModel()).ToList();
            return new QueryResponseModel { Documents = matchingDocs };
        }
    }
}
