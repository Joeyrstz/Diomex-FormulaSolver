using System.Globalization;
using System.Text.RegularExpressions;

namespace StringSolver;

public static partial class ShuntingYardCalculator
{
    //Can also be extended by adding more operators
    public static readonly Dictionary<string, int> OperatorPrecedence = new()
    {
        {"+", 1},
        {"-", 1},
        {"*", 2},
        {"/", 2},
        {"^", 3}
    };
    
    //To detect all the numbers, operators and brackets in the formula
    // GeneratedRegex is faster than a runtime Regex, because it is compiled at compile time instead of runtime
    // Also improves readability because a DataAttribute is nice and clean
    // (has to be replaced with a runtime Regex, if extention for more operators is needed)
    // (Dynamically generated Data Attributes are not supported in .NET yet)
    [GeneratedRegex(@"(-?\d+\.?\d*)|([()])|([+\-*/^])")]
    private static partial Regex ExpressionRegex();
    
    //To Detect negative numbers in the formula
    [GeneratedRegex(@"(?<=^|-|\+|\*|/|\()\s*-(?=\s*\d)")]
    private static partial Regex NegativeNumbersRegex();

    
    //Converts a infix formula to a postfix formula
    //For example: 1 + 2 * 3 -> 1 2 3 * +
    //Purpose of this is to make it easier to calculate the formula
    public static Queue<string> InfixToPostfix(string infix)
    {
        //override the parameter to save memory
        infix = ProcessNegativeNumbers(infix);
        
        var outputQueue = new Queue<string>();
        var opStack = new Stack<string>();

        var tokens = new List<string>();
        var matches = ExpressionRegex().Matches(infix);
        foreach (Match match in matches)
        {
            tokens.Add(match.Value);
        }

        foreach (var token in tokens)
        {
            var formattedToken = token.Replace(',', '.'); // replace ',' with '.'
            if (double.TryParse(formattedToken, NumberStyles.Any, CultureInfo.InvariantCulture, out _)) // Number
            {
                outputQueue.Enqueue(token);
            }
            else if (OperatorPrecedence.TryGetValue(token, out var value)) // Operator
            {
                while (opStack.Count > 0 &&
                       OperatorPrecedence.ContainsKey(opStack.Peek()) &&
                       OperatorPrecedence[opStack.Peek()] >= value)
                {
                    outputQueue.Enqueue(opStack.Pop());
                }

                opStack.Push(token);
            }
            else switch (token)
            {
                // Left Parenthesis
                case "(":
                    opStack.Push(token);
                    break;
                // Right Parenthesis
                case ")":
                {
                    string topToken;
                    while ((topToken = opStack.Pop()) != "(")
                    {
                        outputQueue.Enqueue(topToken);
                    }
                    break;
                }
            }
        }

        while (opStack.Count > 0)
        {
            outputQueue.Enqueue(opStack.Pop());
        }

        return outputQueue;
    }
    
    //We replace every negative number with (-1 * number)
    //This makes it easier for the syntax analyzer to parse the formula
    //it clarifies that it is not a operator but a negative number
    private static string ProcessNegativeNumbers(string infix)
    {
        infix = NegativeNumbersRegex().Replace(infix, "(-1 *");
        var openBracesCount = infix.Count(x => x == '(');
        var closeBracesCount = infix.Count(x => x == ')');
        var difference = openBracesCount - closeBracesCount;

        for (var i = 1; i <= difference; i++)
        {
            infix += ")";
        }

        infix = infix.Replace("--", "+");
        return infix;
    }
}