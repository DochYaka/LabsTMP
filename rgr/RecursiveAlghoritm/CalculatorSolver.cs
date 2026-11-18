using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Alghoritms
{
    public static class CalculatorSolver
    {
        public static string Solve(string expression, IAlghoritm alghoritm)
        {
            if (string.IsNullOrWhiteSpace(expression))
                throw new ArgumentException("Выражение пустое!");

            List<Token> tokens = CalculatorTokenizer.Tokenize(expression);

            var info = CalculatorValidator.Validate(tokens);
            if (!info.IsValid)
                throw new Exception("Выражение записано неверно!");

            return alghoritm.Solve(tokens);
        }       
    }
}