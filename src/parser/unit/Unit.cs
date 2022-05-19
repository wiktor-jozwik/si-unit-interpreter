namespace si_unit_interpreter.parser.unit;

public class Unit
{
    public readonly string Name;
    public long Power;

    public Unit(string name, long power = 1)
    {
        Name = name;
        Power = power;
    }
}