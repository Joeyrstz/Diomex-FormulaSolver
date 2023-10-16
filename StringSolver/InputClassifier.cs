using System.Text.RegularExpressions;

namespace StringSolver;

public static partial class InputClassifier
{
    //Also generated regex for performance reasons
    [GeneratedRegex(@"^([a-zA-Z\d+\-*/()\s]+)(var\(\w+\))?[\d+\-*/()\s\a-zA-Z]*$")]
    private static partial Regex FormulaCharactersRegex();

    public static bool IsFormula(string input) => FormulaCharactersRegex().IsMatch(input);
}
