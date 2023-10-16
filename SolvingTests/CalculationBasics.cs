using StringSolver;

namespace SolvingTests;

public class CalculationBasics
{
    [Fact]
    public void Addition()
    {
        var rpnExpression = ShuntingYardCalculator.InfixToPostfix("1 + 2");
        var result = RpnSolver.Solve(rpnExpression);
        Assert.Equal(3, result);
    }
    
    [Fact]
    public void Subtraction()
    {
        var rpnExpression = ShuntingYardCalculator.InfixToPostfix("1 - 2");
        var result = RpnSolver.Solve(rpnExpression);
        Assert.Equal(-1, result);
    }
    
    [Fact]
    public void Multiply()
    {
        var rpnExpression = ShuntingYardCalculator.InfixToPostfix("5 * 7");
        var result = RpnSolver.Solve(rpnExpression);
        Assert.Equal(35, result);
    }
    
    [Fact]
    public void Divide()
    {
        var rpnExpression = ShuntingYardCalculator.InfixToPostfix("42 / 6");
        var result = RpnSolver.Solve(rpnExpression);
        Assert.Equal(7, result);
    }
    
}