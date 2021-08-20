using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Lucraft.Database.Query
{
    /// <summary>
    /// 
    /// </summary>
    public class Condition
    {
        private readonly int _type;
        private readonly string _field;
        private readonly object _value;
        private readonly string _op;
        private readonly Condition _con1, _con2;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="field"></param>
        /// <param name="op"></param>
        /// <param name="value"></param>
        public Condition(string field, string op, object value)
        {
            _type = 0;
            _field = field;
            _op = op;
            _value = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="con1"></param>
        /// <param name="op"></param>
        /// <param name="con2"></param>
        public Condition(Condition con1, string op, Condition con2)
        {
            _type = 1;
            _con1 = con1;
            _op = op;
            _con2 = con2;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public bool Check(Document document)
        {
            if (_type == 1)
            {
                return _op switch
                {
                    "||" => _con1.Check(document) || _con2.Check(document),
                    "&&" => _con1.Check(document) && _con2.Check(document),
                    _ => false,
                };
            }
            Dictionary<string, object> data = document.GetData();
            return _op switch
            {
                "==" => data.ContainsKey(_field) && data[_field].Equals(_value),
                "!=" => data.ContainsKey(_field) && !data[_field].Equals(_value),
                "<" => data.ContainsKey(_field) && (decimal) data[_field] < (decimal) _value,
                ">" => data.ContainsKey(_field) && (decimal) data[_field] > (decimal) _value,
                "<=" => data.ContainsKey(_field) && (decimal) data[_field] <= (decimal) _value,
                ">=" => data.ContainsKey(_field) && (decimal) data[_field] >= (decimal) _value,
                "contains" => data.ContainsKey(_field) && data[_field] is JArray array && Contains(array, _value),
                _ => false
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static bool Contains(IEnumerable array, object value)
        {
            return value == null ? 
                array.Cast<JValue>().Any(val => val.Value == null) : 
                array.Cast<JValue>().Any(val => val.Value != null && val.Value.Equals(value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (_type == 0)
            {
                return "Condition:{" + _field + "," + _op + "," + _value + "}";
            }
            return "Condition:{" + _con1 + "," + _op + "," + _con2 + "}";
        }
    }
}
