using System;

namespace Lucraft.Database.Query
{
    /// <summary>
    /// 
    /// </summary>
    public enum TokenType
    {
        Identifier,             // identifier
        StringLiteral,          // "String"
        NumberLiteral,          // 3.1415 / 9.34865E+234 / -23 / 4
        Boolean,                // true / false
        NullValue,              // null
        LeftParenthesis,        // (
        RightParenthesis,       // )
        IsEqualTo,
        IsNotEqualTo,
        IsLessThan,
        IsLessOrEqualTo,
        IsGreaterThan,
        IsGreaterOrEqualTo,
        Contains,               // Todo: add to parser
        And,
        Or
    }

    /// <summary>
    /// 
    /// </summary>
    public readonly struct Token : IEquatable<Token>
    {
        public readonly TokenType TokenType;
        public readonly object Value;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        public Token(TokenType type, object value)
        {
            TokenType = type;
            Value = value;
        }

        public bool Equals(Token other)
        {
            return (TokenType == other.TokenType) && (Value == other.Value);
        }

        public override string ToString()
        {
            return "Type: " + TokenType + "; Value: " + Value;
        }

        public override bool Equals(object obj)
        {
            return obj is Token token && Equals(token);
        }
    }
}