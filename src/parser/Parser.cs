using si_unit_interpreter.parser.expression;
using si_unit_interpreter.parser.expression.additive;
using si_unit_interpreter.parser.expression.comparison;
using si_unit_interpreter.parser.expression.literal;
using si_unit_interpreter.parser.expression.multiplicative;
using si_unit_interpreter.parser.expression.negate;
using si_unit_interpreter.parser.statement;
using si_unit_interpreter.parser.type;
using si_unit_interpreter.parser.unit.expression;
using si_unit_interpreter.parser.unit.expression.power;

namespace si_unit_interpreter.parser;

public class Parser
{
    private readonly Lexer _lexer;
    
    private readonly IList<IStatement> _statements = new List<IStatement>();
    private readonly IDictionary<string, IList<IStatement>> _functions = new Dictionary<string, IList<IStatement>>();
    private readonly IDictionary<string, UnitType> _units = new Dictionary<string, UnitType>();

    private readonly Dictionary<TokenType, Func<IExpression, IExpression, IExpression>> _comparisonOperatorMap;
    private readonly Dictionary<TokenType, Func<IExpression, IExpression, IExpression>> _additiveOperatorMap;
    private readonly Dictionary<TokenType, Func<IExpression, IExpression, IExpression>> _multiplicativeOperatorMap;
    private readonly Dictionary<TokenType, Func<IExpression, IExpression>> _negateOperatorMap;

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
    }
    
   public Program Parse()
   {
       ParseStatements();
       
       return new Program(_statements, _functions, _units);
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
               // TODO
               throw new Exception();
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
               // TODO
               throw new Exception();
           }

           leftExpressionComparison = new LogicFactor(leftExpressionComparison, rightExpressionComparison);
       }

       return leftExpressionComparison;
   }

   private IExpression? TryParseExpressionComparison()
   {
       var leftAdditiveExpression = TryParseAdditiveExpression();
       
       if (leftAdditiveExpression == null) return null;
       
       while (_comparisonOperatorMap.TryGetValue(_lexer.Token.Type, out var comparisonExpression))
       {
           _lexer.GetNextToken();
           
           var rightAdditiveExpression = TryParseAdditiveExpression();
           if (rightAdditiveExpression == null)
           {
               // TODO
               throw new Exception();
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
               // TODO
               throw new Exception();
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
               // TODO
               throw new Exception();
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

           return child == null ? null : negateExpression(child);
       }

       return TryParsePrimaryExpression();
   }

   private IExpression? TryParsePrimaryExpression()
   {
       var literal = TryParseLiteral();

       if (literal != null) return literal;

       var identifierOrFunctionCall = TryParseIdentifierOrFunctionCall();

       if (identifierOrFunctionCall != null) return identifierOrFunctionCall;
       
       if (_CheckAndConsume(TokenType.LEFT_PARENTHESES))
       {
           var expression = TryParseExpression();

           if (!_CheckAndConsume(TokenType.RIGHT_PARENTHESES))
           {
               // TODO
               throw new Exception();
           }

           return expression;
       }

       return null;
   }

   private IExpression? TryParseLiteral()
   {
       if (_TokenIs(TokenType.TRUE)) return new BoolLiteral(true);
       if (_TokenIs(TokenType.FALSE)) return new BoolLiteral(false);
       if (_TokenIs(TokenType.STRING)) return new StringLiteral(_lexer.Token.Value);

       if (_TokenIs(TokenType.FLOAT) || _TokenIs(TokenType.INT))
       {
           var value = _GetValueOfTokenAndPrepareNext();
           UnitType? unitType = null;
           
           if (_CheckAndConsume(TokenType.COLON))
           {
               unitType = (UnitType?) TryParseUnitType();

               if (unitType == null)
               {
                   // TODO
                   throw new Exception();
               }
           }

           if (value?.GetType().Equals(typeof(long))) return new IntLiteral(value, unitType);

           if (value?.GetType().Equals(typeof(double))) return new FloatLiteral(value, unitType);
       }

       return null;
   }

   private IExpression? TryParseIdentifierOrFunctionCall()
   {
       if (!_TokenIs(TokenType.IDENTIFIER)) return null;

       var value = _GetValueOfTokenAndPrepareNext();
       return TryParseFunctionCall() ?? new Identifier(value);
   }

   // FunctionCall should match both IStatement and IExpression I guess
   private dynamic? TryParseFunctionCall()
   {
       if (!_TokenIs(TokenType.IDENTIFIER)) return null;
       
       var identifier = _GetValueOfTokenAndPrepareNext();

       if (!_CheckAndConsume(TokenType.LEFT_PARENTHESES)) return null;
       
       var arguments = TryParseArguments();

       if (!_CheckAndConsume(TokenType.RIGHT_PARENTHESES))
       {
           // TODO
           throw new Exception();
       }

       return new FunctionCall(identifier, arguments);
   }

   private List<IExpression> TryParseArguments()
   {
       var arguments = new List<IExpression>();
       var argument = TryParseExpression();

       if (argument == null) return arguments;
       
       arguments.Add(argument);

       while (_CheckAndConsume(TokenType.COMMA))
       {
           var nextArgument = TryParseExpression();
           if (nextArgument == null)
           {
               // TODO
               throw new Exception();
           } 
           arguments.Add(argument);
       }

       return arguments;
   }

   private FunctionStatement? TryParseFunctionStatement()
   {
       if (!_CheckAndConsume(TokenType.FUNCTION)) return null;
       

       if (!_TokenIs(TokenType.IDENTIFIER))
       {
           // TODO
           throw new Exception();
       }
       var functionName = _GetValueOfTokenAndPrepareNext();

       if (!_CheckAndConsume(TokenType.LEFT_PARENTHESES))
       {
           // TODO
           throw new Exception();
       }

       var parameters = TryParseParameters();
       
       if (!_CheckAndConsume(TokenType.RIGHT_PARENTHESES))
       {
           // TODO
           throw new Exception();
       }
       
       if (!_CheckAndConsume(TokenType.RETURN_ARROW))
       {
           // TODO
           throw new Exception();
       }

       var returnType = TryParseReturnType();
       var statements = TryParseBlock();

       return new FunctionStatement(functionName, parameters, returnType, statements);
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
               // TODO
               throw new Exception();
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
           // TODO
           throw new Exception();
       }

       var variableType = TryParseVariableType();

       return new Parameter(identifier, variableType);
   }

   private IType? TryParseReturnType()
   {
       return _TokenIs(TokenType.VOID_TYPE) ? new VoidType() : TryParseVariableType();
   }

   private IType? TryParseVariableType()
   {
       if (_CheckAndConsume(TokenType.STRING_TYPE)) return new StringType();
       if (_CheckAndConsume(TokenType.BOOL_TYPE)) return new BoolType();

       return TryParseUnitType();
   }

   private IType TryParseUnitType()
   {
       if (!_CheckAndConsume(TokenType.LEFT_SQUARE_BRACKET))
       {
           // TODO
           throw new Exception();
       }

       var unitExpression = TryParseUnitExpression();
       
       if (!_CheckAndConsume(TokenType.RIGHT_SQUARE_BRACKET))
       {
           // TODO
           throw new Exception();
       }

       return new UnitType(unitExpression);
   }

   private IUnitExpression? TryParseUnitExpression()
   {
       var leftUnitUnaryExpression = TryParseUnitUnaryExpression();

       if (leftUnitUnaryExpression == null) return null;
       
       while (_CheckAndConsume(TokenType.MULTIPLICATION_OPERATOR))
       {
           var rightUnitUnaryExpression = TryParseUnitUnaryExpression();
           if (rightUnitUnaryExpression == null)
           {
               // TODO
               throw new Exception();
           }
           
           leftUnitUnaryExpression = new UnitExpression(leftUnitUnaryExpression, rightUnitUnaryExpression);
       }

       return leftUnitUnaryExpression;
   }

   private IUnitExpression? TryParseUnitUnaryExpression()
   {
       if (!_TokenIs(TokenType.IDENTIFIER)) return null;

       var identifier = _GetValueOfTokenAndPrepareNext();
       var unitPower = TryParseUnitPower();

       return new UnitUnaryExpression(identifier, unitPower);
   }

   private IUnitPower? TryParseUnitPower()
   {
       if (!_CheckAndConsume(TokenType.POWER_OPERATOR)) return null;

       if (_CheckAndConsume(TokenType.MINUS_OPERATOR))
       {
           long minusValue = _GetValueOfTokenAndPrepareNext();
           return new UnitMinusPower(minusValue);
       }

       long value = _GetValueOfTokenAndPrepareNext();
       return new UnitPower(value);
   }

   private List<IStatement> TryParseBlock()
   {
       if (!_CheckAndConsume(TokenType.LEFT_CURLY_BRACE))
       {
           // TODO
           throw new Exception();
       }

       var block = new List<IStatement>();
       var statement = TryParseStatement();

       while (statement != null)
       {
           block.Add(statement);
           statement = TryParseStatement();
       }
       
       if (!_CheckAndConsume(TokenType.RIGHT_CURLY_BRACE))
       {
           // TODO
           throw new Exception();
       }

       return block;
   }

   private IStatement? TryParseStatement()
   {
       return TryParseFunctionCall() ?? 
              TryParseFunctionStatement() ?? 
              TryParseAssignStatement() ?? 
              TryParseUnitDeclaration() ?? 
              TryParseReturnStatement() ?? 
              TryParseIfStatement() ??
              TryParseWhileStatement();
   }

   private void ParseStatements()
   {
       var functionStatement = TryParseFunctionStatement();
       var unitDeclaration = TryParseUnitDeclaration();
       var statement = 
           TryParseAssignStatement() ??
           TryParseFunctionCall() ??
           TryParseReturnStatement() ?? 
           TryParseIfStatement() ??
           TryParseWhileStatement();

       while (functionStatement != null || statement != null || unitDeclaration != null)
       {
           if (functionStatement != null) _functions[functionStatement.Name] = functionStatement.Statements;

           if (unitDeclaration != null) _units[unitDeclaration.Identifier] = unitDeclaration.Type;
           
           if (statement != null) _statements.Add(statement);
           
           functionStatement = TryParseFunctionStatement();
           unitDeclaration = TryParseUnitDeclaration();
           statement = 
               TryParseAssignStatement() ??
               TryParseFunctionCall() ??
               TryParseUnitDeclaration() ?? 
               TryParseReturnStatement() ?? 
               TryParseIfStatement() ??
               TryParseWhileStatement();
       }
   }
   
   private AssignStatement? TryParseAssignStatement()
   {
       if (!_CheckAndConsume(TokenType.LET)) return null;

       var parameter = TryParseParameter();

       if (parameter == null)
       {
           // TODO
           throw new Exception();
       }

       if (!_CheckAndConsume(TokenType.ASSIGNMENT_OPERATOR))
       {
           // TODO
           throw new Exception();
       }

       var expression = TryParseExpression();
       
       if (expression == null)
       {
           // TODO
           throw new Exception();
       }

       return new AssignStatement(parameter, expression);
   }
   
   private UnitDeclaration? TryParseUnitDeclaration()
   {
       if (!_CheckAndConsume(TokenType.UNIT)) return null;

       if (!_TokenIs(TokenType.IDENTIFIER))
       {
           // TODO
           throw new Exception();
       }
       var identifier = _GetValueOfTokenAndPrepareNext();

       if (!_CheckAndConsume(TokenType.COLON))
       {
           // TODO
           throw new Exception();
       }

       var unitType = TryParseUnitType();

       if (unitType == null)
       {
           // TODO
           throw new Exception();
       }

       return new UnitDeclaration(identifier, (UnitType) unitType);
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
       var ifElseStatement = new List<IStatement>();

       while (_CheckAndConsume(TokenType.ELSE))
       {
           if (_CheckAndConsume(TokenType.IF))
           {
               var (elseIfCondition, elseIfStatements) = TryParseIfConditionAndBlock();

               if (elseIfCondition == null || elseIfStatements == null)
               {
                   // TODO
                   throw new Exception();
               }
               
               elseIfStatements.Add(new ElseIfStatement(elseIfCondition, elseIfStatements));
           }

           var block = TryParseBlock();

           if (block == null)
           {
               // TODO
               throw new Exception();   
           }
       }

       return new IfStatement(condition, statements, ifElseIfStatements, ifElseStatement);
   }

   private (IExpression, IList<IStatement>) TryParseIfConditionAndBlock()
   {
       if (!_CheckAndConsume(TokenType.LEFT_PARENTHESES))
       {
           // TODO
           throw new Exception();
       }
       
       var condition = TryParseExpression();

       if (condition == null)
       {
           // TODO
           throw new Exception();
       }
       
       if (!_CheckAndConsume(TokenType.RIGHT_PARENTHESES))
       {
           // TODO
           throw new Exception();
       }

       var statements = TryParseBlock();

       return (condition, statements);
   }
   
   

   private IStatement? TryParseWhileStatement()
   {
       if (!_CheckAndConsume(TokenType.WHILE)) return null;

       if (!_CheckAndConsume(TokenType.LEFT_PARENTHESES))
       {
           // TODO
           throw new Exception();
       }

       var condition = TryParseExpression();

       if (condition == null)
       {
           // TODO
           throw new Exception();
       }
       
       if (!_CheckAndConsume(TokenType.RIGHT_PARENTHESES))
       {
           // TODO
           throw new Exception();
       }

       var statements = TryParseBlock();
       
       return new WhileStatement(condition, statements);
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