using System;
using System.Text.RegularExpressions;
using System.Xml.XPath;

namespace LexiconA3
{
    public class Program
    {
        static void Main(string[] args)
        {
            string input;
            while (true)
            {
                Console.WriteLine("Hello, gimme something to compute! x closes the app!");
                input = Console.ReadLine();
                if (input == "x") return;
                Console.WriteLine(input + "=" + ParseMath(input)); //manuellt löst, med mer felhanterning
                Console.WriteLine("XPath-evaled: " + input + "=" + XPathEval(input)); //min favorit för text->matte, löser allt med ordning,
                                                                                      //parenteser osv utan extra libraries, lite långsamt dock
            }
        }
        public delegate double Calculator(double x, double y);
        public static double Calculate(double x, double y, Calculator t) => t(x, y);
        /// <summary>
        /// Calculates the sum of x and y
        /// </summary>
        public static double Plus(double x, double y) => x + y;
        public static double Plus(double[] xs) => Calculate(xs, Plus);


        /// <summary>
        /// Calculates the difference of x and y
        /// </summary>    
        public static double Minus(double x, double y) => x - y;
        public static double Minus(double[] xs) => Calculate(xs, Minus);


        /// <summary>
        /// Calculates the fraction of x and y
        /// </summary> 
        /// <param name="x"></param>
        /// <param name="y">This should not be 0</param>
        /// <returns></returns>
        public static double Divide(double x, double y) => SafeDivision(x, y);

        /// <summary>
        /// Calculates the fraction of x and y
        /// </summary> 
        /// <param name="x"></param>
        /// <param name="y">This should not be 0</param>
        /// <returns></returns>
        public static double Multiply(double x, double y) => x * y;
        public static double Multiply(double[] xs) => Calculate(xs, Multiply);

        /// <summary>
        /// Calculates the fraction of x and y
        /// </summary> 
        /// <param name="x"></param>
        /// <param name="y">This should not be 0</param>
        /// <returns></returns>
        public static double Exp(double x, double y) => Math.Pow(x, y);


        /// <summary>
        /// takes an input string, splits it and calculates the result
        /// </summary>
        /// <param name="inp"></param>
        /// <returns></returns>
        public static double ParseMath(string inp)
        {
            double num1, num2, result = double.NaN;
            string[] mathstring = inp.Split('+', '-', '/', '*', '^');
            if (!(mathstring.Length == 2)) //stod inget om att kunna hantera flera operatorer, så gör det enkelt för mig själv här
            {
                Console.WriteLine("That isn't a valid input, please try again!");
                return double.NaN;
            }
            num1 = AskForDouble(mathstring[0]);
            num2 = AskForDouble(mathstring[1]); //säkerställa att vi har två st double
            Calculator calc = Plus;
            if (inp.Contains('-')) calc = Minus;
            if (inp.Contains('*')) calc = Multiply;
            if (inp.Contains('/')) calc = Divide;
            if (inp.Contains('^')) calc = Exp;
            return Calculate(num1, num2, calc);
        }

        /// <summary>
        /// Handles the multiple operations needed by arrays
        /// </summary>
        /// <param name="xs">Array of doubles</param>
        /// <param name="t">The calculator</param>
        /// <returns></returns>
        public static double Calculate(double[] xs, Calculator t)
        {
            double val = 0;
            switch (xs.Length)
            {
                case 0:
                    break;
                case 1:
                    val = xs[0];
                    break;
                default:
                    if (t == Minus) val = xs[0] * 2;
                    if (t == Multiply) val = 1;
                    foreach (double d in xs)
                    {
                        val = Calculate(val, d, t);
                    }
                    break;
            }
            return val;
        }

        /// <summary>
        /// Checks that the user isn't trying to divide by zero or googling google
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y">This may not be 0</param>
        /// <returns></returns>
        public static double SafeDivision(double x, double y)
        {
            if (y == 0)
                throw new DivideByZeroException();
            return x / y;
        }


        /// <summary>
        /// Uses XPathDocument Evaluate function to do the math stuff
        /// </summary>
        /// <param name="expression"></param>
        /// <returns>Calculated value, NaN if it can't calculate it</returns>
        public static double XPathEval(string expression)
        {
            try
            {   //hmm. /0 verkar ju ge lite underliga resultat (om den inte menar 8 = infinity). Skulle kunna fixas med en regex-matchning.
                var xsltExpression =
                    string.Format("number({0})",
                        new Regex(@"([\+\-\*])").Replace(expression, " ${1} ")
                                                .Replace("/", " div ")
                                                .Replace("%", " mod "));

                return (double)new XPathDocument
                    (new System.IO.StringReader("<r/>"))
                        .CreateNavigator()
                        .Evaluate(xsltExpression);
            }
            catch (Exception ex)
            {
                Console.WriteLine("XPath can't evaluate that string!");
                return double.NaN;
            }
        }


        /// <summary>
        /// Tests a string, and loops a prompt until the user inputs a valid double
        /// </summary>
        /// <returns></returns>
        public static double AskForDouble(string inp)
        {
            if (double.TryParse(inp, out double value))
            {
                return value;
            }
            else
            {
                Console.WriteLine(inp + " isn't a valid input, please try again!");
                return AskForDouble();
            }
        }

        /// <summary>
        /// Loops a prompt until the user inputs a valid double
        /// </summary>
        /// <returns></returns>
        public static double AskForDouble()
        {
            double value;
            while (!double.TryParse(Console.ReadLine(), out value))
            {
                Console.WriteLine("That isn't a valid input, please try again!");
            }
            return value;
        }
    }
}
