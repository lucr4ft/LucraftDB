using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Lucraft.Database.Query
{
    public class Parser
    {

        

        public static void GetInstructions(string query)
        {
            List<Token> tokens = new Lexer().Tokenize(query);
            foreach (var token in tokens)
            {
                Console.WriteLine(token);
            }
        }

    }

    class Lexer
    {

        private readonly string[] keywords =
        {
            "get", "set", "where", "true", "false", "null"
        };

        public List<Token> Tokenize(string query)
        {
            List<Token> tokens = new List<Token>();
            string[] words = query.Split(" ");
            foreach (var word in words)
            {
                if (keywords.Contains(word))
                {
                    tokens.Add(new Token(word, TokenType.Keyword));
                }
                else if (decimal.TryParse(word, out decimal number))
                {
                    tokens.Add(new Token(number, TokenType.NumberLiteral));
                }
                else if (word.StartsWith("'") && word.EndsWith("'"))
                {
                    tokens.Add(new Token(word[1..^1], TokenType.StringLiteral));
                }
                else if (word == "=")
                {
                    tokens.Add(new Token(word, TokenType.EqualSign));
                }
                else if (word.StartsWith("/"))
                {
                    tokens.Add(new Token(word, TokenType.Path));
                }
                else if (Regex.IsMatch(word, "^[a-zA-Z_]"))
                {
                    tokens.Add(new Token(word, TokenType.Identifier));
                }
                else
                {
                    throw new ArgumentException();
                }
            }
            return tokens;
        }

    }

}
