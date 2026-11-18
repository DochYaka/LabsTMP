using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Alghoritms.CalculatorSolver;

namespace Alghoritms
{
    public static class CalculatorTokenizer
    {
        private static readonly HashSet<string> _operators = ["+", "-", "*", "/", "^"];

        //Разбивает выражение на токены
        public static List<Token> Tokenize(string expression)
        {
            List<Token> tokens = new();
            StringBuilder currentNumber = new StringBuilder();

            bool isDouble = false;

            for (int i = 0; i < expression.Length; i++)
            {
                var ch = expression[i];

                if (char.IsDigit(ch) || ch == ',')
                {
                    if (ch == ',')
                    {
                        if (!isDouble)
                            isDouble = true;
                        else
                            throw new Exception("Неверная запись дробного числа!");
                    }

                    currentNumber.Append(ch);
                }
                else
                {
                    if (currentNumber.Length > 0)
                    {
                        tokens.Add(new Token(currentNumber.ToString(), TokenType.Digit));
                        currentNumber.Clear();
                    }

                    if (_operators.Contains(ch.ToString()))
                    {
                        tokens.Add(new Token(ch.ToString(), TokenType.Operator));
                    }
                    else
                    {

                        switch (ch.ToString().ToLower())
                        {
                            case "=":
                                tokens.Add(new Token(ch.ToString(), TokenType.Equals));
                                break;

                            case "(":
                                tokens.Add(new Token(ch.ToString(), TokenType.LeftBracket));
                                break;

                            case ")":
                                tokens.Add(new Token(ch.ToString(), TokenType.RightBracket));
                                break;

                            case "x":
                                tokens.Add(new Token(ch.ToString(), TokenType.Variable));
                                break;

                            default:
                                throw new Exception($"Неподдерживаемый символ \'{ch}\'!");
                        }
                    }
                }
            }

            if (currentNumber.Length > 0)
            {
                tokens.Add(new Token(currentNumber.ToString(), TokenType.Digit));
            }

            return tokens;
        }
    }
}
