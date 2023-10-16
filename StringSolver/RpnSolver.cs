using System.Globalization;

namespace StringSolver;

public static class RpnSolver
{
    //Used a dictionary here, so that we can easily add more operations later or they can be added by users of the class library
    public static readonly Dictionary<string, Func<double, double, double>> Operations = new()
    {
        { "+", (left, right) => left + right },
        { "-", (left, right) => left - right },
        { "*", (left, right) => left * right },
        { "/", (left, right) => left / right },
        { "^", (left, right) => Math.Pow(left, right) }
    };

    //This is the actual RPN solver, it takes a queue of strings, which is the output of the Shunting Yard algorithm
    //It then iterates over the queue and applies the operations to the stack
    //The stack is used to store the intermediate results of the calculation
    //The result is the last element in the stack
    public static double Solve(Queue<string> rpnExpression)
    {
        var stack = new Stack<double>();
        while (rpnExpression.Count > 0)
        {
            var token = rpnExpression.Dequeue();
            if (double.TryParse(token, NumberStyles.Any, CultureInfo.InvariantCulture, out var number))
            {
                stack.Push(number);
            }
            else
            {
                if (!Operations.TryGetValue(token, out var operationFunc)) throw new InvalidOperationException($"Unknown operator {token}");

                var right = stack.Pop();
                var left = stack.Pop();
                var result = operationFunc(left, right);
                stack.Push(result);
            }
        }

        return stack.Pop();
    }
}