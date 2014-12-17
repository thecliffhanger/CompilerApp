using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProgram;
using NUnit.Framework;

namespace TestProgram.Tests
{
    [TestFixture()]
    public class CalculatorTests
    {
        [Test()]
        public void PerformAddTest()
        {
            int expected = 30;
            Calculator calc = new Calculator();
            int actual = calc.Perform("+", 20, 10);
            Assert.AreEqual(expected, actual);
        }

        [Test()]
        public void PerformSubstractTest()
        {
            int expected = 10;
            Calculator calc = new Calculator();
            int actual = calc.Perform("-", 20, 10);
            Assert.AreEqual(expected, actual);
        }

        [Test()]
        public void PerformMultiplyTest()
        {
            int expected = 200;
            Calculator calc = new Calculator();
            int actual = calc.Perform("*", 20, 10);
            Assert.AreEqual(expected, actual);
        }

        [Test()]
        public void PerformDivideTest()
        {
            int expected = 2;
            Calculator calc = new Calculator();
            int actual = calc.Perform("/", 20, 10);
            Assert.AreEqual(expected, actual);
        }
    }
}
