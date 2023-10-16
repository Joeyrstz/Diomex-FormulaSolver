using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;
using StringSolver;

//The Solution is overall built in .NET 7 because of the new Source Generators and the massive performance
//improvement they bring to the table. The Source Generators are used to generate the Regexes at compile time

var variables = new Dictionary<string, string>();

Console.WriteLine("Please give me a formula and I will calculate it for you.");
Console.WriteLine("For example: 1 + 2 * 3");


while (true)
{
    Console.Write(">");
    var input = Console.ReadLine();
    
    if (string.IsNullOrWhiteSpace(input)) continue;
    
    HandleInput(input);
}



void HandleInput(string value)
{
    switch (value.Trim().ToLower())
    {
        case "exit":
            return;
        case "show vars":
            Console.WriteLine("Variables:");
            foreach (var variable in variables)
            {
                Console.WriteLine($"{variable.Key} = {variable.Value}");
            }

            return;
    }

    if (value.StartsWith("var ")) // Variable assignment
    {
        var match = Regex.Match(value, @"var (\w+) = (.+)");
        if (!match.Success)
        {
            Console.WriteLine("Invalid variable assignment syntax. Should be in format: var variableName = value / formula");
            return;
        }
        
        var variableName = match.Groups[1].Value;
        var variableExpression = match.Groups[2].Value;

        // Replace any var(x) in the expression with its values.
        try
        {
            variableExpression = ReplaceVariables(variableExpression);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return;
        }
        
        if (double.TryParse(variableExpression, out _))
        {
            variables[variableName] = variableExpression;
            Console.WriteLine($"Set {variableName} = {variableExpression}");
        }
        else if (InputClassifier.IsFormula(variableExpression))
        {
            try
            {
                Console.WriteLine("Evaluating formula");
                var result = SolveFormula(variableExpression);
                variables[variableName] = result.ToString(CultureInfo.InvariantCulture);
                Console.WriteLine($"Set {variableName} = result {result} of formula {variableExpression}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        
    }
    else if(InputClassifier.IsFormula(value))
    {
        try
        {
            var result = SolveFormula(value);
            Console.WriteLine($"Result: {result}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
    else
    {
        Console.WriteLine("Invalid input");
    }
}

double SolveFormula(string input)
{
    input = ReplaceVariables(input);
    
    var postfixExpression = ShuntingYardCalculator.InfixToPostfix(input);
    var result = RpnSolver.Solve(postfixExpression);
    return result;
}

string ReplaceVariables(string input)
{
    input = variables.Aggregate(input, (current, variable) => current.Replace(variable.Key, variable.Value));

    //check which var() are still in the value by Regex (doesnt need to be a high performance regex)
    var matches = Regex.Matches(input, @"\b[a-zA-Z]+\b");

    if (matches.Count is 0) return input;
   
    //Format error message to 'Unknown variables: x, y, z
    var unknownVariablesString = string.Join(", ", matches.Select(m => m.Value));
    throw new FormatException($"Unknown variables: {unknownVariablesString}");
}