using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Lucraft.Database.Query
{
    public class Condition
    {
        private readonly int _type;

        private readonly string _field;
        private readonly object _value;
        private readonly string _op;
        private readonly Condition _con1, _con2;

        public Condition(string field, string op, object value)
        {
            _type = 0;
            _field = field;
            _op = op;
            _value = value;
        }

        public Condition(Condition con1, string op, Condition con2)
        {
            _type = 1;
            _con1 = con1;
            _op = op;
            _con2 = con2;
        }

        public bool Check(Document document)
        {
            if (_type == 1)
                return _op switch
                {
                    "||" => _con1.Check(document) || _con2.Check(document),
                    "&&" => _con1.Check(document) && _con2.Check(document),
                    _ => false,
                };
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

        private static bool Contains(IEnumerable array, object value)
        {
            return value == null ? 
                array.Cast<JValue>().Any(val => val.Value == null) : 
                array.Cast<JValue>().Any(val => val.Value != null && val.Value.Equals(value));
        }

        public override string ToString()
        {
            if (_type == 0)
                return "Condition:{" + _field + "," + _op + "," + _value + "}";
            return "Condition:{" + _con1 + "," + _op + "," + _con2 + "}";
        }
    }
}
