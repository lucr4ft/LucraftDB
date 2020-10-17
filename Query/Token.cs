using System;
using System.Collections.Generic;
using System.Text;

namespace Lucraft.Database.Query
{
    public struct Token
    {

        public object Value { get; }
        public TokenType Type { get; }

        public Token(object value, TokenType type)
        {
            Value = value;
            Type = type;
        }

        public override string ToString()
        {
            return "TYPE: " + Type + "/VALUE: " + Value;
        }

    }

    public enum TokenType
    {
        Keyword = 0,
        Path = 1,
        EqualSign = 2,
        StringLiteral = 3,
        NumberLiteral = 4,
        Identifier = 5,
    }

}
