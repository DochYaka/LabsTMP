using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alghoritms
{
    public enum TokenType
    {
        Digit, Operator, Variable, Equals, RightBracket, LeftBracket
    }

    public class Token(string value, TokenType type)
    {
        public string Value { get; } = value;
        public TokenType Type { get; } = type;
    }
}
