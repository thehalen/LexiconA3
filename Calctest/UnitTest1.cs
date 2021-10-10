using System;
using Xunit;
using LexiconA3;

namespace CalcTest
{
    public class CalculatorShould
    {

        [Fact]
        [Trait("Cat", "Operators")]
        public void AddStuff()
        {
            Assert.Equal(10, Program.Plus(5, 5));
            Assert.Equal(10, Program.Calculate(5, 5, Program.Plus));
        }

        [Fact]
        [Trait("Cat", "Array operators")]
        public void ArrayAddStuff()
        {
            Assert.Equal(0, Program.Calculate(new double[0], Program.Plus));
            Assert.Equal(15, Program.Calculate(new double[] { 10, 5 }, Program.Plus));
            Assert.Equal(17, Program.Calculate(new double[] { 10, 5, 2 }, Program.Plus));
            Assert.Equal(27, Program.Calculate(new double[] { 5, 10, 12 }, Program.Plus));
            Assert.Equal(27, Program.Plus(new double[] { 5, 10, 12 }));
        }


        [Fact]
        [Trait("Cat", "Operators")]
        public void SubtractStuff()
        {
            Assert.Equal(10, Program.Minus(15, 5));
            Assert.Equal(10, Program.Calculate(15, 5, Program.Minus));
        }

        [Fact]
        [Trait("Cat", "Array operators")]
        public void ArraySubtractStuff()
        {
            Assert.Equal(0, Program.Calculate(new double[0], Program.Minus));
            Assert.Equal(5, Program.Calculate(new double[] { 10, 5 }, Program.Minus));
            Assert.Equal(3, Program.Calculate(new double[] { 10, 5, 2 }, Program.Minus));
            Assert.Equal(-7, Program.Calculate(new double[] { 5, 10, 2 }, Program.Minus));
            Assert.Equal(-7, Program.Minus(new double[] { 5, 10, 2 }));
        }


        [Fact]
        [Trait("Cat", "Operators")]
        public void DivideStuff()
        {
            Assert.Equal(10, Program.Divide(50, 5));
            Assert.Equal(10, Program.Calculate(50, 5, Program.Divide));
        }

        [Fact]
        [Trait("Cat", "Operators")]
        public void DivideByZero()
        {

            Exception exception = Assert.Throws<DivideByZeroException>(() =>
            {
                var result = Program.Calculate(50, 0, Program.Divide);

                Assert.IsType<decimal>(result);
            });

            Assert.IsType<DivideByZeroException>(exception);
        }


        [Fact]
        [Trait("Cat", "Operators")]
        public void MultiplyStuff()
        {
            Assert.Equal(10, Program.Multiply(2, 5));
            Assert.Equal(10, Program.Calculate(2, 5, Program.Multiply));
        }

        [Fact]
        [Trait("Cat", "Array operators")]
        public void ArrayMultiplyStuff()
        {
            Assert.Equal(0, Program.Calculate(new double[0], Program.Multiply));
            Assert.Equal(50, Program.Calculate(new double[] { 10, 5 }, Program.Multiply));
            Assert.Equal(100, Program.Calculate(new double[] { 10, 5, 2 }, Program.Multiply));
            Assert.Equal(6000, Program.Calculate(new double[] { 600, 5, 2 }, Program.Multiply));
            Assert.Equal(6000, Program.Multiply(new double[] { 600, 5, 2 }));
        }

        [Fact]
        [Trait("Cat", "Operators")]
        public void ExpStuff()
        {
            Assert.Equal(125, Program.Exp(5, 3));
            Assert.Equal(625, Program.Calculate(5, 4, Program.Exp));
        }

    }
}
