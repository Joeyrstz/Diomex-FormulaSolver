using StringSolver;

namespace SolvingTests;

public class AdvancedCalculation
{
    [Fact]
    public void FormulaWithBrackets() {
        var rpnExpression = ShuntingYardCalculator.InfixToPostfix("(1 + 2) * 3");
        var result = RpnSolver.Solve(rpnExpression);
        Assert.Equal(9, result);
    }
    
    [Fact]
    public void FormulaWithComplexBrackets() {
        var rpnExpression = ShuntingYardCalculator.InfixToPostfix("((1 + 2) * (3 + 4)) + (5.2 * 2)");
        var result = RpnSolver.Solve(rpnExpression);
        Assert.Equal(31.4, result);
    }
    
    [Fact]
    public void ComplexExpression() {
        var rpnExpression = ShuntingYardCalculator.InfixToPostfix("3 + 4 * 2 / ( 1 - 5 )");
        var result = RpnSolver.Solve(rpnExpression);
        Assert.Equal(1, result);
    }
    
    [Fact]
    public void MultiplyDoubleValue() {
        var rpnExpression = ShuntingYardCalculator.InfixToPostfix("1 + 2.5 * 3");
        var result = RpnSolver.Solve(rpnExpression);
        Assert.Equal(8.5, result);
    }

    [Fact]
    public void NegativeNumbers() {
        var rpnExpression = ShuntingYardCalculator.InfixToPostfix("-3 + 4");
        var result = RpnSolver.Solve(rpnExpression);
        Assert.Equal(1, result);
    }
    
    [Fact]
    public void ExponentiationOperator()
    {
        var rpnExpression = ShuntingYardCalculator.InfixToPostfix("2 ^ 3");
        var result = RpnSolver.Solve(rpnExpression);
        Assert.Equal(8, result);
    }
}