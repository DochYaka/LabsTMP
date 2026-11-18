using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Alghoritms.CalculatorSolver;

namespace Alghoritms
{
    public class ExpressionInfo(bool isValid, bool isRecursive)
    {
        public bool IsValid { get; } = isValid;
        public bool IsRecursive { get; } = isRecursive;
    }

    public static class CalculatorValidator
    {
        public static ExpressionInfo Validate(List<Token> tokens)
        {
            if (tokens.Count == 0)
                return new ExpressionInfo(false, false);

            int bracketBalance = 0;
            bool hasOperator = false;
            bool lastWasOperator = false;
            bool equalsFlag = false;
            bool isRecursive = false;
            bool isValid = true;

            foreach (var item in tokens)
            {
                if (item.Type == TokenType.Operator)
                {
                    if (lastWasOperator)
                    {
                        isValid = false;
                        break;
                    }

                    if (!hasOperator)
                        hasOperator = true;

                    lastWasOperator = true;
                }
                else if (item.Type == TokenType.Equals)
                {
                    if ((equalsFlag && !isRecursive) || lastWasOperator)
                    {
                        isValid = false;
                        break;
                    }
                    equalsFlag = true;
                    lastWasOperator = false;
                }
                else if (item.Type == TokenType.LeftBracket)
                {
                    lastWasOperator = false;
                    bracketBalance++;
                    hasOperator = false;
                }
                else if (item.Type == TokenType.RightBracket)
                {
                    lastWasOperator = false;
                    bracketBalance--;
                    if (bracketBalance < 0 || !hasOperator)
                    {
                        isValid = false;
                        break;
                    }
                }
                else if (item.Type == TokenType.Variable)
                {
                    isRecursive = true;
                    lastWasOperator = false;
                }
                else
                {
                    lastWasOperator = false;
                }
            }

            if(isValid)
                isValid = !((!equalsFlag && isRecursive) || lastWasOperator || !hasOperator || bracketBalance != 0);

            return new ExpressionInfo(isValid, isRecursive);
        }
    }
}
