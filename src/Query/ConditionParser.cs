﻿using Lucraft.Database.Exceptions;
using System;
using System.Collections.Generic;

namespace Lucraft.Database.Query
{
    /// <summary>
    /// 
    /// </summary>
    public static class ConditionParser
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static Condition Parse(string condition)
        {
            List<Token> tokens = ConditionLexer.GetTokens(condition);
            return EvalParenthesis(tokens.GetRange(1, tokens.Count - 2));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="conditionString"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static bool TryParse(string conditionString, out Condition condition)
        {
            try
            {
                condition = Parse(conditionString);
                if (condition is null)
                {
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                condition = null;
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        /// Todo: implement syntax checking
        private static Condition EvalParenthesis(IReadOnlyList<Token> tokens)
        {
            var list = new List<object>();
            int c1 = tokens.Count;
            int c2 = tokens.Count - 1;

            for (int i = 0; i < c1; i++)
            {
                if (tokens[i].TokenType == TokenType.LeftParenthesis)
                {
                    i++;
                    int open = 1, close = 0;
                    var temp = new List<Token>();
                    do
                    {
                        temp.Add(tokens[i++]);
                        if (i == c2)
                        {
                            throw new MalformedQueryException();
                        }
                        else if (tokens[i].TokenType == TokenType.LeftParenthesis)
                        {
                            open++;
                        }
                        else if (tokens[i].TokenType == TokenType.RightParenthesis)
                        {
                            close++;
                        }
                    } while (open > close);
                    list.Add(EvalParenthesis(temp));
                }
                else
                {
                    list.Add(tokens[i]);
                }
            }

            if (list.Count > 1)
            {
                if (list[0] is Condition con1 && list[2] is Condition con2)
                {
                    return new Condition(con1, ((Token)list[1]).Value as string, con2);
                }
                return new Condition((string)((Token)list[0]).Value, (string)((Token)list[1]).Value, (string)((Token)list[2]).Value);
            }
            else if (list.Count == 0 || list[0] is not Condition)
            {
                throw new MalformedQueryException("Malformed Query #1");
            }
            return list[0] as Condition;
        }
    }
}
