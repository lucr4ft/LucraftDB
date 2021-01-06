using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Lucraft.Database.Query
{
    class ConditionLexer
    {
        public static List<Token> GetTokens(string condition)
        {
            List<Token> tokens = new List<Token>();
            string[] chars = condition.ToCharArray().Select(c => c.ToString()).ToArray();
            for (int i = 0; i < chars.Length; i++)
            {
                if (Regex.IsMatch(chars[i], "\\s")) { continue; }
                else if (Regex.IsMatch(chars[i], "[a-zA-Z_]"))
                {
                    string s = "";
                    while (chars.Length > i && Regex.IsMatch(chars[i], "[a-zA-Z_0-9-]"))
                        s += chars[i++];
                    if (s.Equals("true") || s.Equals("false"))
                        tokens.Add(new Token(TokenType.Boolean, bool.Parse(s)));
                    else if (s.Equals("null"))
                        tokens.Add(new Token(TokenType.NullValue, null));
                    else
                        tokens.Add(new Token(TokenType.Identifier, s));
                    if (chars.Length > i && !Regex.IsMatch(chars[i], "[a-zA-Z_0-9]"))
                        i--;
                }
                else if (Regex.IsMatch(chars[i], "[0-9]") || (Regex.IsMatch(chars[i], "\\.") && chars.Length > i + 1 && Regex.IsMatch(chars[i + 1], "[0-9]")))
                {
                    string s = chars[i];
                    string temp = s;
                    while (++i < chars.Length && Regex.IsMatch(temp += chars[i], "^([0-9]*(\\.)?)?[0-9]*$"))
                        s += chars[i];
                    tokens.Add(new Token(TokenType.NumberLiteral, Convert.ToDouble(s)));
                    if (chars.Length > i && !Regex.IsMatch(temp, "^[0-9]*(\\.)?[0-9]*$"))
                        i--;
                }
                else if (chars[i].Equals("\""))
                {
                    string s = "";
                    while (chars.Length > i + 1 && !chars[++i].Equals("\""))
                        s += chars[i];
                    tokens.Add(new Token(TokenType.StringLiteral, s));
                }
                else if (chars[i].Equals("("))
                {
                    tokens.Add(new Token(TokenType.LeftParenthesis, chars[i]));
                }
                else if (chars[i].Equals(")"))
                {
                    tokens.Add(new Token(TokenType.RightParenthesis, chars[i]));
                }
                else if (chars[i].Equals("="))
                {
                    if (chars.Length > i + 1 && chars[i + 1].Equals("="))
                    {
                        tokens.Add(new Token(TokenType.IsEqualTo, "=="));
                        i++;
                    }
                    else throw new Exception();
                }
                else if (chars[i].Equals("!"))
                {
                    if (chars.Length > i + 1 && chars[i + 1].Equals("="))
                    {
                        tokens.Add(new Token(TokenType.IsNotEqualTo, "!="));
                        i++;
                    }
                    else throw new Exception();
                }
                else if (chars[i].Equals("<"))
                {
                    if (chars.Length > i + 1 && chars[i + 1].Equals("="))
                    {
                        tokens.Add(new Token(TokenType.IsLessOrEqualTo, "<="));
                        i++;
                    }
                    else
                    {
                        tokens.Add(new Token(TokenType.IsLessThan, chars[i]));
                    }
                }
                else if (chars[i].Equals(">"))
                {
                    if (chars.Length > i + 1 && chars[i + 1].Equals("="))
                    {
                        tokens.Add(new Token(TokenType.IsGreatherOrEqualTo, ">="));
                        i++;
                    }
                    else
                    {
                        tokens.Add(new Token(TokenType.IsGreatherThan, chars[i]));
                    }
                }
                else if (chars[i].Equals("&") && chars.Length > i + 1 && chars[i + 1].Equals("&"))
                {
                    tokens.Add(new Token(TokenType.And, "&&"));
                    i++;
                }
                else if (chars[i].Equals("|") && chars.Length > i + 1 && chars[i + 1].Equals("|"))
                {
                    tokens.Add(new Token(TokenType.Or, "||"));
                    i++;
                }
                else
                {
                    Console.Error.WriteLine("unknown char: " + chars[i]);
                }
            }
            return tokens;
        }
    }

    public class ConditionParser
    {
        public static Condition GetCondition(string condition)
        {
            List<Token> tokens = ConditionLexer.GetTokens(condition);
            return EvalParenthesis(tokens: tokens.GetRange(1, tokens.Count - 2));
        }

        private static Condition EvalParenthesis(List<Token> tokens)
        {
            List<object> list = new List<object>();
            for (int i = 0; i < tokens.Count; i++)
            {
                if (tokens[i].TokenType == TokenType.LeftParenthesis)
                {
                    int open = 1, close = 0;
                    List<Token> temp = new List<Token> { tokens[i++] };
                    do
                    {
                        temp.Add(tokens[i]);
                        if (i > tokens.Count - 1) throw new Exception();
                        if (tokens[i].TokenType == TokenType.LeftParenthesis) open++;
                        else if (tokens[i].TokenType == TokenType.RightParenthesis) close++;
                        i++;
                    } while (open > close);
                    i--;
                    Condition tempCondition = EvalParenthesis(temp.GetRange(1, temp.Count - 2));
                    list.Add(tempCondition);
                }
                else
                {
                    list.Add(tokens[i]);
                }
            }
            if (list.Count > 1)
            {
                if (list[0] is Condition && list[2] is Condition)
                    return new Condition(list[0] as Condition, ((Token)list[1]).Value as string, list[2] as Condition);
                else
                    return new Condition(((Token)list[0]).Value as string, ((Token)list[1]).Value as string, ((Token)list[2]).Value as string);
            }
            else if (list.Count == 0 || !(list[0] is Condition))
                throw new Exception();
            return list[0] as Condition;
        }
    }
}
