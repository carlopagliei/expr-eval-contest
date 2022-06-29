using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExprEval
{


    internal class ExprEvaluator
    {
        public static string? EvaluateExpression(string expr)
        {
            Stack<string> rpnStack = rpn_parse_expression(expr);
            if (rpnStack.Count == 0) return null;
            return $"{rpn_evaluate(rpnStack)}";
        }


        // private 


        private static double rpn_evaluate(Stack<string> rpnStack)
        {
            string token = rpnStack.Pop();
            double x, y;
            if (!Double.TryParse(token, out x))
            {
                y = rpn_evaluate(rpnStack);
                x = rpn_evaluate(rpnStack);
                x = get_operator(token)?.Execute?.Invoke(x, y) ?? x;
            }
            return x;
        }

        private static Stack<string> rpn_parse_expression(string expression)
        {
            Stack<string> pendingOperators = new Stack<string>();
            Stack<string> rpnStack = new Stack<string>();
            string c = string.Empty;
            for (int i = 0; i < expression.Length; i++)
            {
                if ((expression[i] >= '0' && expression[i] <= '9') || (expression[i] == '.'))
                {
                    c += expression[i];
                    continue;
                }
                else
                {
                    string e = $"{expression[i]}";
                    if (double.TryParse(c.ToString(), out _))
                    {
                        rpnStack.Push(c);
                        c = string.Empty;
                    }
                    if (expression[i] == '(')
                    {
                        pendingOperators.Push(e);
                    }
                    else
                    if (expression[i] == ')')
                    {
                        while (pendingOperators.Count != 0 && pendingOperators.Peek() != "(")
                            rpnStack.Push(pendingOperators.Pop());
                        pendingOperators.Pop();
                    }
                    else
                    if (is_operator(e, out Operator? op))
                    {
                        while (pendingOperators.Count != 0 && (get_operator(pendingOperators.Peek())?.Priority ?? -1) >= op?.Priority)
                            rpnStack.Push(pendingOperators.Pop());
                        pendingOperators.Push(e);
                    }
                }
            }
            if (int.TryParse(c.ToString(), out _))
                rpnStack.Push(c);
            while (pendingOperators.Count != 0)
                rpnStack.Push(pendingOperators.Pop());
            return rpnStack;
        }

        private static bool is_operator(string c, out Operator? op)
        {
            op = operators.FirstOrDefault(o => o.Id == c);
            return op != null;
        }

        private static Operator? get_operator(string c) => operators.FirstOrDefault(o => o.Id == c);

        private static List<Operator> operators = new List<Operator>()
        {
            new Operator(){ Id = "+",  Priority = 1, Execute = (x, y) => x + y },
            new Operator(){ Id = "-",  Priority = 1, Execute = (x, y) => x - y },
            new Operator(){ Id = "*",  Priority = 2, Execute = (x, y) => x * y },
            new Operator(){ Id = "/",  Priority = 2, Execute = (x, y) => x / y },
            new Operator(){ Id = "^",  Priority = 3, Execute = (x, y) => Math.Pow(x, y) },
        };

        class Operator
        {
            public string? Id;
            public int Priority;
            public Func<double, double, double>? Execute;
        }

    }


}
