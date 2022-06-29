using System;
using System.Text;

namespace ExprEval
{
    internal class Program
    {
        public static void Main()
        {
            while (true)
            {
                Console.WriteLine("Write an infix expression, something like '(2+1)^2'");
                string s = Console.ReadLine();
                // s = "5 + ( 10 * 2 ) + 1";
                // s = "3 ^ 4 + ( 11 - ( 3 * 2 ) ) / 2";
                // s = "2 + ( 4^0.5 + 8) + 1 + 1";
                //string? result = tradizionale.EvaluateExpression(s);
                string? result = ExprEvaluator.EvaluateExpression(s);
                Console.WriteLine(result);
            }
        }



    }
}