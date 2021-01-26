namespace Lucraft.Database.Query
{
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

    public readonly struct Token
    {
        public readonly TokenType TokenType;
        public readonly object Value;

        public Token(TokenType type, object value)
        {
            TokenType = type;
            Value = value;
        }

        public override string ToString()
        {
            return "Type: " + TokenType + "; Value: " + Value;
        }
    }
}