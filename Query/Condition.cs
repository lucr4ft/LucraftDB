using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Lucraft.Database.Query
{
    public class Condition
    {
        private readonly int type;

        private readonly string field;
        private readonly object value;
        private readonly string op;
        private readonly Condition con1, con2;

        public Condition(string field, string op, object value)
        {
            type = 0;
            this.field = field;
            this.op = op;
            this.value = value;
        }

        public Condition(Condition con1, string op, Condition con2)
        {
            type = 1;
            this.con1 = con1;
            this.op = op;
            this.con2 = con2;
        }

        public bool Check(Document document)
        {
            //Console.WriteLine("Type: " + type);
            if (type == 0)
            {
                //Console.WriteLine(field + " - " + op + " - " + value);

                Dictionary<string, object> data = document.GetData();
                //try
                //{
                //    //Console.WriteLine((data.ContainsKey(field) && data[field].Equals(value)) + " - " + data[field] + " - " + value.GetType() + " - " + data[field].GetType());
                //}
                //catch (Exception)
                //{
                //}
                return op switch
                {
                    "==" => data.ContainsKey(field) && data[field].Equals(value),
                    "!=" => data.ContainsKey(field) && !data[field].Equals(value),
                    "<" => data.ContainsKey(field) && (decimal)data[field] < (decimal)value,
                    ">" => data.ContainsKey(field) && (decimal)data[field] > (decimal)value,
                    "<=" => data.ContainsKey(field) && (decimal)data[field] <= (decimal)value,
                    ">=" => data.ContainsKey(field) && (decimal)data[field] >= (decimal)value,
                    "contains" => data.ContainsKey(field) && data[field] is JArray array && Contains(array, value),
                    _ => false
                };
            }
            else
            {
                return op switch
                {
                    "||" => con1.Check(document) || con2.Check(document),
                    "&&" => con1.Check(document) && con2.Check(document),
                    _ => false,
                };
            }
        }

        private bool Contains(JArray array, object value)
        {
            foreach (JValue val in array)
            {
                if (val.Value.Equals(value)) return true;
                //Console.WriteLine(val.Value.Equals("a"));
            }
            return false;
        }

        public static Condition GetCondition(string query)
        {
            Condition condition;

            if (query.Contains("&&"))
            {
                condition = new Condition(GetCondition(query.Split("&&")[0]), "&&", GetCondition(query.Substring(query.Split("&&")[0].Length + 2)));
            }
            else if (query.Contains("||"))
            {
                condition = new Condition(GetCondition(query.Split("||")[0]), "||", GetCondition(query.Substring(query.Split("||")[0].Length + 2)));
            }
            else
            {
                object value;
                string valueStr = query.Substring(query.Split(" ")[0].Length + query.Split(" ")[1].Length + 2);

                if (long.TryParse(valueStr, out long l)) value = l;
                else if (bool.TryParse(valueStr, out bool b)) value = b;
                else if (valueStr.Equals("null")) value = null;
                else value = valueStr;

                condition = new Condition(query.Split(" ")[0], query.Split(" ")[1], value);
            }
            return condition;
        }

        public override string ToString()
        {
            if (type == 0)
            {
                return "Condition:{" + field + "," + op + "," + value + "}";
            }
            else
            {
                return "Condition:{" + con1.ToString() + "," + op + "," + con2.ToString() + "}";
            }
        }

    }
}
