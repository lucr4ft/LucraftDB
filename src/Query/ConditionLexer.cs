using Lucraft.Database.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Lucraft.Database.Query
{
    /// <summary>
    /// 
    /// </summary>
    internal static class ConditionLexer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static List<Token> GetTokens(string condition)
        {
            var tokens = new List<Token>();
            string[] chars = condition.ToCharArray().Select(c => c.ToString()).ToArray();
            for (int i = 0; i < chars.Length; i++)
            {
                if (Regex.IsMatch(chars[i], "\\s"))
                {
                    continue;
                }
                else if (Regex.IsMatch(chars[i], "[a-zA-Z_]"))
                {
                    var builder = new StringBuilder();
                    while (chars.Length > i && Regex.IsMatch(chars[i], "[a-zA-Z_0-9-]"))
                    {
                        builder.Append(chars[i++]);
                    }

                    tokens.Add(builder.ToString() switch
                    {
                        "true" => new Token(TokenType.Boolean, true),
                        "false" => new Token(TokenType.Boolean, false),
                        "null" => new Token(TokenType.NullValue, null),
                        _ => new Token(TokenType.Identifier, builder.ToString())
                    });

                    i--;
                }
                else if (Regex.IsMatch(chars[i], "[0-9]") || (Regex.IsMatch(chars[i], "\\.") && chars.Length > i + 1 &&
                                                              Regex.IsMatch(chars[i + 1], "[0-9]")))
                {
                    var builder = new StringBuilder();
                    while (++i < chars.Length && Regex.IsMatch(builder.ToString() + chars[i], "^([0-9]*(\\.)?)?[0-9]*$"))
                    {
                        builder.Append(chars[i]);
                    }
                    tokens.Add(new Token(TokenType.NumberLiteral, Convert.ToDecimal(builder.ToString())));
                    i--;
                }
                else if (chars[i].Equals("\""))
                {
                    var builder = new StringBuilder();
                    while (chars.Length > i + 1 && !chars[++i].Equals("\""))
                    {
                        builder.Append(chars[i]);
                    }
                    tokens.Add(new Token(TokenType.StringLiteral, builder.ToString()));
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
                    if (chars.Length > i + 1)
                    {
                        if (chars[i + 1].Equals("="))
                        {
                            tokens.Add(new Token(TokenType.IsEqualTo, "=="));
                            i++;
                        }
                        else
                        {
                            throw new MalformedQueryException($"Unknown operator '={ chars[i + 1] }'");
                        }
                    }
                    else
                    {
                        throw new MalformedQueryException($"Unexpected character '{ chars[i] }' at { i + 1 }");
                    }
                }
                else if (chars[i].Equals("!"))
                {
                    if (chars.Length > i + 1)
                    {
                        if (chars[i + 1].Equals("="))
                        {
                            tokens.Add(new Token(TokenType.IsNotEqualTo, "!="));
                            i++;
                        }
                        else
                        {
                            throw new MalformedQueryException($"Unknown operator '!{ chars[i + 1] }'");
                        }
                    }
                    else
                    {
                        throw new MalformedQueryException($"Unexpected character '{ chars[i] }' at { i + 1 }");
                    }
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
                    SimpleLogger.Log(Level.Error, "unknown char: " + chars[i]);
                    throw new MalformedQueryException("Malformed Query #2");
                }
            }
            return tokens;
        }
    }
}