using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Lucraft.Database.Query
{
    internal static class ConditionLexer
    {
        public static List<Token> GetTokens(string condition)
        {
            List<Token> tokens = new();
            string[] chars = condition.ToCharArray().Select(c => c.ToString()).ToArray();
            for (int i = 0; i < chars.Length; i++)
            {
                if (Regex.IsMatch(chars[i], "\\s")) { } // continue 
                else if (Regex.IsMatch(chars[i], "[a-zA-Z_]"))
                {
                    string s = "";
                    while (chars.Length > i && Regex.IsMatch(chars[i], "[a-zA-Z_0-9-]"))
                        s += chars[i++];
                    switch (s)
                    {
                        case "true":
                        case "false":
                            tokens.Add(new Token(TokenType.Boolean, bool.Parse(s)));
                            break;
                        case "null":
                            tokens.Add(new Token(TokenType.NullValue, null));
                            break;
                        default:
                            tokens.Add(new Token(TokenType.Identifier, s));
                            break;
                    }

                    if (chars.Length > i && !Regex.IsMatch(chars[i], "[a-zA-Z_0-9]"))
                        i--;
                }
                else if (Regex.IsMatch(chars[i], "[0-9]") || (Regex.IsMatch(chars[i], "\\.") && chars.Length > i + 1 &&
                                                              Regex.IsMatch(chars[i + 1], "[0-9]")))
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
                        tokens.Add(new Token(TokenType.IsGreaterOrEqualTo, ">="));
                        i++;
                    }
                    else
                    {
                        tokens.Add(new Token(TokenType.IsGreaterThan, chars[i]));
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
}