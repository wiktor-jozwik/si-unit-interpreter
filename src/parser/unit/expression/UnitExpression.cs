namespace si_unit_interpreter.parser.unit.expression;

public class UnitExpression: IUnitExpression
{
    public readonly IUnitExpression Left;
    public readonly IUnitExpression? Right;

    public UnitExpression(IUnitExpression left, IUnitExpression right)
    {
        Left = left;
        Right = right;
    }
}