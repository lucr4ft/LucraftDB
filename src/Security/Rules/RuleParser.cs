using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucraft.Database.Security.Rules
{
    /// <summary>
    /// 
    /// </summary>
    public static class RuleParser
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rule"></param>
        /// <returns></returns>
        public static Rule Parse(string rule)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ruleStr"></param>
        /// <param name="rule"></param>
        /// <returns></returns>
        public static bool TryParse(string ruleStr, out Rule rule)
        {
            try
            {
                rule = Parse(rule: ruleStr);
                if (rule is null)
                {
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                rule = null;
                return false;
            }
        }
    }
}
