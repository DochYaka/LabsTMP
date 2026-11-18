using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alghoritms
{
    public interface IAlghoritm
    {
        public string Solve(List<Token> tokens);
    }

    public class RecursiveAlghoritm : IAlghoritm
    {
        private List<Token> _tokens;
        private int _position;

        public string Solve(List<Token> tokens)
        {
            var equationParts = SplitEquation(tokens);
            var leftTokens = equationParts.left;
            var rightTokens = equationParts.right;

            var allTokens = MoveToLeftSide(leftTokens, rightTokens);

            Console.WriteLine("Выражение после переноса: " + string.Join(" ", allTokens.Select(t => t.Value)));

            double a = 0, b = 0, c = 0;
            _position = 0;
            _tokens = allTokens;

            ParseExpression(ref a, ref b, ref c);

            Console.WriteLine($"Найденные коэффициенты: a={a}, b={b}, c={c}");

            return SolveQuadraticEquation(a, b, c);
        }

        private (List<Token> left, List<Token> right) SplitEquation(List<Token> tokens)
        {
            int equalsIndex = tokens.FindIndex(t => t.Type == TokenType.Equals);
            if (equalsIndex == -1)
                throw new ArgumentException("Уравнение должно содержать знак равенства");

            var left = tokens.Take(equalsIndex).ToList();
            var right = tokens.Skip(equalsIndex + 1).ToList();

            return (left, right);
        }

        private List<Token> MoveToLeftSide(List<Token> left, List<Token> right)
        {
            var result = new List<Token>(left);

            if (right.Count == 0 || (right.Count == 1 && right[0].Value == "0" && right[0].Type == TokenType.Digit))
                return result;

            List<Token> processedRight = new List<Token>(right);

            if (processedRight.Count > 0 && processedRight[0].Type != TokenType.Operator)
            {
                processedRight.Insert(0, new Token("+", TokenType.Operator));
            }

            for (int i = 0; i < processedRight.Count; i++)
            {
                if (processedRight[i].Type == TokenType.Operator)
                {
                    string op = processedRight[i].Value;
                    string newOp = (op == "+") ? "-" : "+";
                    result.Add(new Token(newOp, TokenType.Operator));
                }
                else
                {
                    result.Add(processedRight[i]);
                }
            }

            return result;
        }

        private void ParseExpression(ref double a, ref double b, ref double c)
        {
            double currentA = 0, currentB = 0, currentC = 0;

            ParseTerm(ref currentA, ref currentB, ref currentC);

            while (_position < _tokens.Count && _tokens[_position].Type == TokenType.Operator)
            {
                string op = _tokens[_position].Value;
                _position++;

                double termA = 0, termB = 0, termC = 0;
                ParseTerm(ref termA, ref termB, ref termC);

                if (op == "+")
                {
                    currentA += termA;
                    currentB += termB;
                    currentC += termC;
                }
                else if (op == "-")
                {
                    currentA -= termA;
                    currentB -= termB;
                    currentC -= termC;
                }
            }

            a = currentA;
            b = currentB;
            c = currentC;
        }

        private void ParseTerm(ref double a, ref double b, ref double c)
        {
            if (_position >= _tokens.Count)
                return;

            double coefficient = 1;
            int sign = 1;

            if (_tokens[_position].Type == TokenType.Operator)
            {
                if (_tokens[_position].Value == "-")
                    sign = -1;
                _position++;
            }

            if (_position < _tokens.Count && _tokens[_position].Type == TokenType.LeftBracket)
            {
                _position++;

                double bracketA = 0, bracketB = 0, bracketC = 0;
                ParseExpression(ref bracketA, ref bracketB, ref bracketC);

                if (_position < _tokens.Count && _tokens[_position].Type == TokenType.RightBracket)
                {
                    _position++;
                }
                else
                {
                    throw new ArgumentException("Ожидалась закрывающая скобка ')'");
                }

                a += bracketA * coefficient * sign;
                b += bracketB * coefficient * sign;
                c += bracketC * coefficient * sign;

                if (_position < _tokens.Count && (_tokens[_position].Type == TokenType.Digit ||
                    (_tokens[_position].Type == TokenType.Variable)))
                {
                    double mulA = 0, mulB = 0, mulC = 0;
                    ParseTerm(ref mulA, ref mulB, ref mulC);

                    if (Math.Abs(mulA) > 1e-10 || Math.Abs(mulB) > 1e-10 || Math.Abs(mulC) > 1e-10)
                    {
                        double oldA = a, oldB = b, oldC = c;
                        a = oldA * mulA + oldB * mulA + oldC * mulA;
                    }
                }

                return;
            }

            if (_position < _tokens.Count && _tokens[_position].Type == TokenType.Digit)
            {
                coefficient = double.Parse(_tokens[_position].Value);
                _position++;
            }

            if (_position < _tokens.Count && _tokens[_position].Type == TokenType.Variable)
            {
                _position++;

                if (_position < _tokens.Count && _tokens[_position].Type == TokenType.Operator &&
                    _tokens[_position].Value == "^")
                {
                    _position++;
                    if (_position < _tokens.Count && _tokens[_position].Type == TokenType.Digit)
                    {
                        int power = int.Parse(_tokens[_position].Value);
                        _position++;

                        if (power == 2)
                            a += coefficient * sign;
                        else if (power == 1)
                            b += coefficient * sign;
                        else
                            throw new ArgumentException($"Степень {power} не поддерживается (максимум 2)");
                    }
                }
                else
                {
                    b += coefficient * sign;
                }
            }
            else
            {
                c += coefficient * sign;
            }
        }

        private string SolveQuadraticEquation(double a, double b, double c)
        {
            if (Math.Abs(a) < 1e-10)
            {
                if (Math.Abs(b) < 1e-10)
                {
                    if (Math.Abs(c) < 1e-10)
                        throw new InvalidOperationException("Бесконечно много решений");
                    else
                        throw new InvalidOperationException("Нет решения");
                }

                double x = -c / b;
                return $"x = {x}";
            }

            double discriminant = b * b - 4 * a * c;

            if (Math.Abs(discriminant) < 1e-10)
            {
                double x = -b / (2 * a);
                return $"x = {x}";
            }
            else if (discriminant > 0)
            {
                double sqrtD = Math.Sqrt(discriminant);
                double x1 = (-b + sqrtD) / (2 * a);
                double x2 = (-b - sqrtD) / (2 * a);
                return $"x1 = {x1}, x2 = {x2}";
            }
            else
            {
                throw new InvalidOperationException("Нет действительных корней (дискриминант < 0)");
            }
        }
    }
}
