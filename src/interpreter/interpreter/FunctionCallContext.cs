namespace si_unit_interpreter.interpreter.interpreter;

public class FunctionCallContext
{
    public readonly LinkedList<Scope> Scopes = new();
    public readonly LinkedList<List<dynamic>> ParameterScopes = new();
}
