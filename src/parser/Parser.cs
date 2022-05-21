using si_unit_interpreter.exceptions.parser;
using si_unit_interpreter.lexer;
using si_unit_interpreter.parser.expression;
using si_unit_interpreter.parser.expression.additive;
using si_unit_interpreter.parser.expression.comparison;
using si_unit_interpreter.parser.expression.literal;
using si_unit_interpreter.parser.expression.multiplicative;
using si_unit_interpreter.parser.expression.negate;
using si_unit_interpreter.parser.statement;
using si_unit_interpreter.parser.type;
using si_unit_interpreter.parser.unit;

namespace si_unit_interpreter.parser;

public class Parser
{
    private readonly Lexer _lexer;
    
    private readonly IDictionary<string, FunctionStatement> _functions = new Dictionary<string, FunctionStatement>();
    private readonly IDictionary<string, UnitType> _units = new Dictionary<string, UnitType>();

    private readonly Dictionary<TokenType, Func<IExpression, IExpression, IExpression>> _comparisonOperatorMap;
    private readonly Dictionary<TokenType, Func<IExpression, IExpression, IExpression>> _additiveOperatorMap;
    private readonly Dictionary<TokenType, Func<IExpression, IExpression, IExpression>> _multiplicativeOperatorMap;
    private readonly Dictionary<TokenType, Func<IExpression, IExpression>> _negateOperatorMap;
    private readonly HashSet<TokenType> _expressionTokenSet;
    private readonly HashSet<TokenType> _returnTypeTokenSet;
    public Parser(Lexer lexer)
    {
        _lexer = lexer;
        _lexer.GetNextToken();
        
        _comparisonOperatorMap = new Dictionary<TokenType, Func<IExpression, IExpression, IExpression>>
        {
            [TokenType.GREATER_THAN_OPERATOR] = (left, right)=> new GreaterThanExpression(left, right),
            [TokenType.SMALLER_THAN_OPERATOR] = (left, right)=> new SmallerThanExpression(left, right),
            [TokenType.GREATER_EQUAL_THAN_OPERATOR] = (left, right)=> new GreaterEqualThanExpression(left, right),
            [TokenType.SMALLER_EQUAL_THAN_OPERATOR] = (left, right)=> new SmallerEqualThanExpression(left, right),
            [TokenType.EQUAL_OPERATOR] = (left, right)=> new EqualExpression(left, right),
            [TokenType.NOT_EQUAL_OPERATOR] = (left, right)=> new NotEqualExpression(left, right),
        };
        
        _additiveOperatorMap = new Dictionary<TokenType, Func<IExpression, IExpression, IExpression>>
        {
            [TokenType.PLUS_OPERATOR] = (left, right)=> new AddExpression(left, right),
            [TokenType.MINUS_OPERATOR] = (left, right)=> new SubtractExpression(left, right),
        };
        
        _multiplicativeOperatorMap = new Dictionary<TokenType, Func<IExpression, IExpression, IExpression>>
        {
            [TokenType.MULTIPLICATION_OPERATOR] = (left, right)=> new MultiplicateExpression(left, right),
            [TokenType.DIVISION_OPERATOR] = (left, right)=> new DivideExpression(left, right),
        };
        
        _negateOperatorMap = new Dictionary<TokenType, Func<IExpression, IExpression>>
        {
            [TokenType.MINUS_OPERATOR] = child=> new MinusExpression(child),
            [TokenType.NEGATE_OPERATOR] = child=> new NotExpression(child),
        };
        
        _expressionTokenSet = new HashSet<TokenType>
        {
            TokenType.TRUE,
            TokenType.FALSE,
            TokenType.INT,
            TokenType.FLOAT,
            TokenType.STRING,
            TokenType.IDENTIFIER,
            TokenType.LEFT_PARENTHESES
        };

        _returnTypeTokenSet = new HashSet<TokenType>
        {
            TokenType.STRING_TYPE,
            TokenType.BOOL_TYPE,
            TokenType.VOID_TYPE,
            TokenType.LEFT_SQUARE_BRACKET,
        };
    }
    
   public TopLevel Parse()
   {
       ParseTopLevelStatements();
       
       return new TopLevel(_functions, _units);
   }
   
   private void ParseTopLevelStatements()
   {
       while(TryParseFunctionStatement() || TryParseUnitDeclaration()){}
   }
   
   private bool TryParseFunctionStatement()
   {
       if (!_TokenIs(TokenType.IDENTIFIER)) return false;
       
       var functionName = _GetValueOfTokenAndPrepareNext();

       if (!_CheckAndConsume(TokenType.LEFT_PARENTHESES))
       {
           throw new ParserException(new HashSet<TokenType>{ TokenType.LEFT_PARENTHESES }, _lexer.Token.Type, _lexer.Token.Position);
       }

       var parameters = TryParseParameters();
       
       if (!_CheckAndConsume(TokenType.RIGHT_PARENTHESES))
       {
           throw new ParserException(new HashSet<TokenType>{ TokenType.RIGHT_PARENTHESES }, _lexer.Token.Type, _lexer.Token.Position);
       }
       
       if (!_CheckAndConsume(TokenType.RETURN_ARROW))
       {
           throw new ParserException(new HashSet<TokenType>{ TokenType.RETURN_ARROW }, _lexer.Token.Type, _lexer.Token.Position);
       }

       var returnType = TryParseReturnType();
       if (returnType == null)
       {
           throw new ParserException(_returnTypeTokenSet, _lexer.Token.Type, _lexer.Token.Position);
       }
       var statements = TryParseBlock();

       if (statements == null)
       {
           throw new ParserException(new HashSet<TokenType>{ TokenType.LEFT_CURLY_BRACE }, _lexer.Token.Type, _lexer.Token.Position);
       }

       _functions[functionName] = new FunctionStatement(parameters, returnType, statements);

       return true;
   }

   private List<Parameter> TryParseParameters()
   {
       var parameters = new List<Parameter>();
       var parameter = TryParseParameter();

       if (parameter != null) parameters.Add(parameter);

       while (_CheckAndConsume(TokenType.COMMA))
       {
           var nextParameter = TryParseParameter();

           if (nextParameter == null)
           {
               throw new ParserException(new HashSet<TokenType>{ TokenType.IDENTIFIER }, _lexer.Token.Type, _lexer.Token.Position);
           }
           parameters.Add(nextParameter);
       }

       return parameters;
   }

   private Parameter? TryParseParameter()
   {
       if (!_TokenIs(TokenType.IDENTIFIER)) return null;
       
       var identifier = _GetValueOfTokenAndPrepareNext();

       if (!_CheckAndConsume(TokenType.COLON))
       {
           throw new ParserException(new HashSet<TokenType>{ TokenType.COLON }, _lexer.Token.Type, _lexer.Token.Position);
       }

       var variableType = TryParseVariableType();

       return new Parameter(identifier, variableType);
   }

   private IType? TryParseReturnType()
   {
       return _CheckAndConsume(TokenType.VOID_TYPE) ? new VoidType() : TryParseVariableType();
   }

   private IType? TryParseVariableType()
   {
       if (_CheckAndConsume(TokenType.STRING_TYPE)) return new StringType();
       if (_CheckAndConsume(TokenType.BOOL_TYPE)) return new BoolType();

       return TryParseUnitType();
   }

   private UnitType? TryParseUnitType()
   {
       if (!_CheckAndConsume(TokenType.LEFT_SQUARE_BRACKET)) return null;

       var units = TryParseUnits();
       
       if (!_CheckAndConsume(TokenType.RIGHT_SQUARE_BRACKET))
       {
           throw new ParserException(new HashSet<TokenType>{ TokenType.RIGHT_SQUARE_BRACKET }, _lexer.Token.Type, _lexer.Token.Position);
       }

       return new UnitType(units);
   }

   private IList<Unit> TryParseUnits()
   {
       var units = new List<Unit>();
       if (!_TokenIs(TokenType.IDENTIFIER)) return units;

       var identifier = _GetValueOfTokenAndPrepareNext();
       var power = TryParsePower();
       units.Add(new Unit(identifier, power));
       
       while (_CheckAndConsume(TokenType.MULTIPLICATION_OPERATOR))
       {
           if (!_TokenIs(TokenType.IDENTIFIER))
           {
               throw new ParserException(new HashSet<TokenType>{ TokenType.IDENTIFIER }, _lexer.Token.Type, _lexer.Token.Position);
           } 
           identifier = _GetValueOfTokenAndPrepareNext();
           power = TryParsePower();
           
           units.Add(new Unit(identifier, power));
       }

       return units;
   }

   private long TryParsePower()
   {
       if (!_CheckAndConsume(TokenType.POWER_OPERATOR)) return 1;

       long value;
       if (_CheckAndConsume(TokenType.MINUS_OPERATOR))
       {
           value = _CheckForIntValueAndGetIt();
           return -1 * value;
       }

       value = _CheckForIntValueAndGetIt();
       return value;
   }

   private long _CheckForIntValueAndGetIt()
   {
       var value = _lexer.Token.Value;
       if (value == null)
       {
           throw new ParserException(new HashSet<TokenType>{ TokenType.INT }, _lexer.Token.Type, _lexer.Token.Position);
       }

       if (value.GetType() != typeof(long))
       {
           throw new ParserException(new HashSet<TokenType>{ TokenType.INT }, _lexer.Token.Type, _lexer.Token.Position);
       }
           
       _lexer.GetNextToken();
       return value;
   }

   private Block? TryParseBlock()
   {
       if (!_CheckAndConsume(TokenType.LEFT_CURLY_BRACE)) return null;

       var statements = new List<IStatement>();
       var statement = TryParseStatement();

       while (statement != null)
       {
           statements.Add(statement);
           statement = TryParseStatement();
       }
       
       if (!_CheckAndConsume(TokenType.RIGHT_CURLY_BRACE))
       {
           throw new ParserException(new HashSet<TokenType>{ TokenType.RIGHT_CURLY_BRACE }, _lexer.Token.Type, _lexer.Token.Position);
       }

       return new Block(statements);
   }

   private bool TryParseUnitDeclaration()
   {
       if (!_CheckAndConsume(TokenType.UNIT)) return false;

       if (!_TokenIs(TokenType.IDENTIFIER))
       {
           throw new ParserException(new HashSet<TokenType>{ TokenType.IDENTIFIER }, _lexer.Token.Type, _lexer.Token.Position);
       }

       var unitName = _GetValueOfTokenAndPrepareNext();

       if (!_CheckAndConsume(TokenType.COLON))
       {
           throw new ParserException(new HashSet<TokenType>{ TokenType.COLON }, _lexer.Token.Type, _lexer.Token.Position);
       }

       var unitType = TryParseUnitType();

       if (unitType == null)
       {
           throw new ParserException(new HashSet<TokenType>{ TokenType.LEFT_SQUARE_BRACKET }, _lexer.Token.Type, _lexer.Token.Position);
       }

       _units[unitName] = unitType;

       return true;
   }

   private IStatement? TryParseStatement()
   {
       return TryParseVariableDeclaration() ?? 
              TryParseAssignStatementOrFunctionCall() ??
              TryParseReturnStatement() ??
              TryParseIfStatement() ??
              TryParseWhileStatement();
   }
   
   private IStatement? TryParseAssignStatementOrFunctionCall()
   {
       if (!_TokenIs(TokenType.IDENTIFIER)) return null;

       var identifier = _GetValueOfTokenAndPrepareNext();

       return TryParseRestOfFunctionCall(identifier) ?? TryParseAssignStatement(identifier);
   }

   private FunctionCall? TryParseRestOfFunctionCall(string identifier)
   {
       if (!_CheckAndConsume(TokenType.LEFT_PARENTHESES)) return null;
       
       var arguments = TryParseArguments();

       if (!_CheckAndConsume(TokenType.RIGHT_PARENTHESES))
       {
           throw new ParserException(new HashSet<TokenType>{ TokenType.RIGHT_PARENTHESES }, _lexer.Token.Type, _lexer.Token.Position);
       }

       return new FunctionCall(identifier, arguments);
   }

   private AssignStatement? TryParseAssignStatement(string identifier)
   {
       if (!_CheckAndConsume(TokenType.ASSIGNMENT_OPERATOR)) return null;
       
       var expression = TryParseExpression();

       if (expression == null)
       {
           throw new ParserException(_expressionTokenSet, _lexer.Token.Type, _lexer.Token.Position);
       }

       return new AssignStatement(identifier, expression);
   }
   
   private List<IExpression> TryParseArguments()
   {
       var arguments = new List<IExpression>();
       var argument = TryParseExpression();

       if (argument == null) return arguments;
       
       arguments.Add(argument);

       while (_CheckAndConsume(TokenType.COMMA))
       {
           argument = TryParseExpression();
           if (argument == null)
           {
               throw new ParserException(_expressionTokenSet, _lexer.Token.Type, _lexer.Token.Position);
           } 
           arguments.Add(argument);
       }

       return arguments;
   }

   private VariableDeclaration? TryParseVariableDeclaration()
   {
       if (!_CheckAndConsume(TokenType.LET)) return null;

       var parameter = TryParseParameter();

       if (parameter == null)
       {
           throw new ParserException(new HashSet<TokenType>{ TokenType.IDENTIFIER }, _lexer.Token.Type, _lexer.Token.Position);
       }

       if (!_CheckAndConsume(TokenType.ASSIGNMENT_OPERATOR))
       {
           throw new ParserException(new HashSet<TokenType>{ TokenType.ASSIGNMENT_OPERATOR }, _lexer.Token.Type, _lexer.Token.Position);
       }

       var expression = TryParseExpression();
       
       if (expression == null)
       {
           throw new ParserException(_expressionTokenSet, _lexer.Token.Type, _lexer.Token.Position);
       }

       return new VariableDeclaration(parameter, expression);
   }

   private IStatement? TryParseReturnStatement()
   {
       if (!_CheckAndConsume(TokenType.RETURN)) return null;

       var expression = TryParseExpression();

       return new ReturnStatement(expression);
   }

   private IStatement? TryParseIfStatement()
   {
       if (!_CheckAndConsume(TokenType.IF)) return null;
       
       var (condition, statements) = TryParseIfConditionAndBlock();

       var ifElseIfStatements = new List<ElseIfStatement>();
       var ifElseStatement = new Block();

       while (_CheckAndConsume(TokenType.ELSE))
       {
           if (_CheckAndConsume(TokenType.IF))
           {
               var (elseIfCondition, elseIfStatements) = TryParseIfConditionAndBlock();
               
               ifElseIfStatements.Add(new ElseIfStatement(elseIfCondition, elseIfStatements));
           }
           else
           {
               var block = TryParseBlock();

               ifElseStatement = block ?? throw new ParserException(new HashSet<TokenType>{ TokenType.LEFT_CURLY_BRACE }, _lexer.Token.Type, _lexer.Token.Position);
           }
       }
       
       return new IfStatement(condition, statements, ifElseIfStatements, ifElseStatement);
   }

   private (IExpression, Block) TryParseIfConditionAndBlock()
   {
       if (!_CheckAndConsume(TokenType.LEFT_PARENTHESES))
       {
           throw new ParserException(new HashSet<TokenType>{ TokenType.LEFT_PARENTHESES }, _lexer.Token.Type, _lexer.Token.Position);
       }
       
       var condition = TryParseExpression();

       if (condition == null)
       {
           throw new ParserException(_expressionTokenSet, _lexer.Token.Type, _lexer.Token.Position);
       }
       
       if (!_CheckAndConsume(TokenType.RIGHT_PARENTHESES))
       {
           throw new ParserException(new HashSet<TokenType>{ TokenType.RIGHT_PARENTHESES }, _lexer.Token.Type, _lexer.Token.Position);
       }

       var statements = TryParseBlock();

       if (statements == null)
       {
           throw new ParserException(new HashSet<TokenType>{ TokenType.LEFT_CURLY_BRACE }, _lexer.Token.Type, _lexer.Token.Position);
       }

       return (condition, statements);
   }
   
   private IStatement? TryParseWhileStatement()
   {
       if (!_CheckAndConsume(TokenType.WHILE)) return null;

       if (!_CheckAndConsume(TokenType.LEFT_PARENTHESES))
       {
           throw new ParserException(new HashSet<TokenType>{ TokenType.LEFT_PARENTHESES }, _lexer.Token.Type, _lexer.Token.Position);
       }

       var condition = TryParseExpression();

       if (condition == null)
       {
           throw new ParserException(_expressionTokenSet, _lexer.Token.Type, _lexer.Token.Position);
       }
       
       if (!_CheckAndConsume(TokenType.RIGHT_PARENTHESES))
       {
           throw new ParserException(new HashSet<TokenType>{ TokenType.RIGHT_PARENTHESES }, _lexer.Token.Type, _lexer.Token.Position);
       }

       var statements = TryParseBlock();
       
       if (statements == null)
       {
           throw new ParserException(new HashSet<TokenType>{ TokenType.LEFT_CURLY_BRACE }, _lexer.Token.Type, _lexer.Token.Position);
       }
       
       return new WhileStatement(condition, statements);
   }
   
   
   private IExpression? TryParseExpression()
   {
       var leftLogicFactor = TryParseLogicFactor();
       
       if (leftLogicFactor == null) return null;
   
       while (_CheckAndConsume(TokenType.OR_OPERATOR))
       {

           var rightLogicFactor = TryParseLogicFactor();
           if (rightLogicFactor == null)
           {
               throw new ParserException(_expressionTokenSet, _lexer.Token.Type, _lexer.Token.Position);
           }

           leftLogicFactor = new Expression(leftLogicFactor, rightLogicFactor);
       }

       return leftLogicFactor;
   }

   private IExpression? TryParseLogicFactor()
   {
       var leftExpressionComparison = TryParseExpressionComparison();

       if (leftExpressionComparison == null) return null;
       
       while (_CheckAndConsume(TokenType.AND_OPERATOR))
       {
           var rightExpressionComparison = TryParseExpressionComparison();
           if (rightExpressionComparison == null)
           {
               throw new ParserException(_expressionTokenSet, _lexer.Token.Type, _lexer.Token.Position);
           }

           leftExpressionComparison = new LogicFactor(leftExpressionComparison, rightExpressionComparison);
       }

       return leftExpressionComparison;
   }

   private IExpression? TryParseExpressionComparison()
   {
       var leftAdditiveExpression = TryParseAdditiveExpression();
       
       if (leftAdditiveExpression == null) return null;
       
       if (_comparisonOperatorMap.TryGetValue(_lexer.Token.Type, out var comparisonExpression))
       {
           _lexer.GetNextToken();
           
           var rightAdditiveExpression = TryParseAdditiveExpression();
           if (rightAdditiveExpression == null)
           {
               throw new ParserException(_expressionTokenSet, _lexer.Token.Type, _lexer.Token.Position);
           }
           
           leftAdditiveExpression = comparisonExpression(leftAdditiveExpression, rightAdditiveExpression);
       }

       return leftAdditiveExpression;
   }

   private IExpression? TryParseAdditiveExpression()
   {
       var leftMultiplicativeExpression = TryParseMultiplicativeExpression();
       
       if (leftMultiplicativeExpression == null) return null;

       while (_additiveOperatorMap.TryGetValue(_lexer.Token.Type, out var additiveExpression))
       {
           _lexer.GetNextToken();
           
           var rightMultiplicativeExpression = TryParseMultiplicativeExpression();
           if (rightMultiplicativeExpression == null)
           {
               throw new ParserException(_expressionTokenSet, _lexer.Token.Type, _lexer.Token.Position);
           }
           
           leftMultiplicativeExpression = additiveExpression(leftMultiplicativeExpression, rightMultiplicativeExpression);
       }

       return leftMultiplicativeExpression;
   }

   private IExpression? TryParseMultiplicativeExpression()
   {
       var leftUnaryExpression = TryParseUnaryExpression();
       
       if (leftUnaryExpression == null) return null;

       while (_multiplicativeOperatorMap.TryGetValue(_lexer.Token.Type, out var multiplicativeExpression))
       {
           _lexer.GetNextToken();
           
           var rightUnaryExpression = TryParseUnaryExpression();
           if (rightUnaryExpression == null)
           {
               throw new ParserException(_expressionTokenSet, _lexer.Token.Type, _lexer.Token.Position);
           }
           
           leftUnaryExpression = multiplicativeExpression(leftUnaryExpression, rightUnaryExpression);
       }

       return leftUnaryExpression;
   }

   private IExpression? TryParseUnaryExpression()
   {
       if (_negateOperatorMap.TryGetValue(_lexer.Token.Type, out var negateExpression))
       {
           _lexer.GetNextToken();

           var child = TryParsePrimaryExpression();

           if (child == null)
           {
               throw new ParserException(_expressionTokenSet, _lexer.Token.Type, _lexer.Token.Position);
           }

           return negateExpression(child);
       }

       return TryParsePrimaryExpression();
   }

   private IExpression? TryParsePrimaryExpression()
   {
       return TryParseLiteral() ?? TryParseIdentifierOrFunctionCallOrParentheses();
   }

   private IExpression? TryParseLiteral()
   {
       return TryParseBoolLiteral() ?? TryParseStringLiteral() ?? TryParseNumLiteral();
   }

   private IExpression? TryParseBoolLiteral()
   {
       bool value;
       if (_TokenIs(TokenType.FALSE)) value = false;
       else if (_TokenIs(TokenType.TRUE)) value = true;
       else return null;
       
       _lexer.GetNextToken();
       return new BoolLiteral(value);
   }

   private IExpression? TryParseStringLiteral()
   {
       return _TokenIs(TokenType.STRING) ? new StringLiteral(_GetValueOfTokenAndPrepareNext()) : null;
   }

   private IExpression? TryParseNumLiteral()
   {
       if (!_TokenIs(TokenType.FLOAT) && !_TokenIs(TokenType.INT)) return null;
       
       var value = _GetValueOfTokenAndPrepareNext();
           
       var unitType = TryParseUnitType() ?? new UnitType(new List<Unit>());

       if (value?.GetType().Equals(typeof(long))) return new IntLiteral(value, unitType);
       if (value?.GetType().Equals(typeof(double))) return new FloatLiteral(value, unitType);

       return null;
   }

   private IExpression? TryParseIdentifierOrFunctionCallOrParentheses()
   {
       return TryParseIdentifierOrFunctionCall() ?? TryParseParenthesesExpression();
   }

   private IExpression? TryParseIdentifierOrFunctionCall()
   {
       if (!_TokenIs(TokenType.IDENTIFIER)) return null;

       var identifier = _GetValueOfTokenAndPrepareNext();
       
       var functionCall = TryParseRestOfFunctionCall(identifier);
       if (functionCall != null) return functionCall;
       
       return new Identifier(identifier);
   }

   private IExpression? TryParseParenthesesExpression()
   {
       if (!_CheckAndConsume(TokenType.LEFT_PARENTHESES)) return null;
       
       var expression = TryParseExpression();

       if (!_CheckAndConsume(TokenType.RIGHT_PARENTHESES))
       {
           throw new ParserException(new HashSet<TokenType>{ TokenType.RIGHT_PARENTHESES }, _lexer.Token.Type, _lexer.Token.Position);
       }

       return expression;
   }
   
   private bool _CheckAndConsume(TokenType type)
   {
       if (!_TokenIs(type)) return false;
       
       _lexer.GetNextToken();
       return true;
   }

   private bool _TokenIs(TokenType type)
   {
       return _lexer.Token.Type == type;
   }

   private dynamic? _GetValueOfTokenAndPrepareNext()
   {
       var value = _lexer.Token.Value;
       _lexer.GetNextToken();
       return value;
   }
}