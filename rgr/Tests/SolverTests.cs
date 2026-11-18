using Alghoritms;
using Moq;
using System.Reflection;

namespace Tests
{
    [TestClass]
    public sealed class SolverTests 
    {
        [TestMethod]
        [DataRow("4x-5=11")]
        [DataRow("(4x+(5+22)/(34+x))+(12x-8x)=8")]
        [DataRow("2+2")]
        public void TestChecking(string expression)
        {
            var tokens = CalculatorTokenizer.Tokenize(expression);
            Assert.IsTrue(CalculatorValidator.Validate(tokens).IsValid);
        }

        [TestMethod]
        [DataRow("((4x)-1)=2")]
        [DataRow("()4x-1=2")]
        [DataRow("4x-5=11-")]
        [DataRow("4x-5+=11")]
        [DataRow("(4x-5=11")]
        [DataRow("4x-5)=11")]
        [DataRow("2+2x")]
        public void TestChecking2(string expression)
        {
            var tokens = CalculatorTokenizer.Tokenize(expression);
            Assert.IsTrue(!CalculatorValidator.Validate(tokens).IsValid);
        }

    }
}
