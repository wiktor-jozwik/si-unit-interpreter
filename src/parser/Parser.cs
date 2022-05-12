using si_unit_interpreter.lexer;
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
    
    private readonly IDictionary<string, FunctionStatement> _functions = new Dictionary<string, FunctionStatement>();
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
    
   public TopLevel Parse()
   {
       ParseProgramStatements();
       
       return new TopLevel(_functions, _units);
   }
   
   private void ParseProgramStatements()
   {
       var functionStatement = TryParseFunctionStatement();
       var unitDeclaration = TryParseUnitDeclaration();
       
       while (functionStatement != null || unitDeclaration != null)
       {
           if (functionStatement != null) _functions[functionStatement.Name] = functionStatement;
           if (unitDeclaration != null) _units[unitDeclaration.Identifier] = unitDeclaration.Type;
           
           functionStatement = TryParseFunctionStatement();
           unitDeclaration = TryParseUnitDeclaration();
       }
   }
   
   private FunctionStatement? TryParseFunctionStatement()
   {
       // if (!_CheckAndConsume(TokenType.FUNCTION)) return null; // mozna by wyrzucic

       if (!_TokenIs(TokenType.IDENTIFIER)) return null;
       
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
       if (returnType == null)
       {
           // TODO
           throw new Exception();
       }
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
           var minusValue = _GetValueOfTokenAndPrepareNext();
           if (minusValue == null) throw new Exception();
           if (minusValue.GetType() != typeof(long)) throw new Exception();
           
           return new UnitMinusPower(minusValue);
       }

       long value = _GetValueOfTokenAndPrepareNext();
       return new UnitPower(value);
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
           // TODO
           throw new Exception();
       }

       return new Block(statements);
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

       return new UnitDeclaration(identifier, unitType);
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
           // TODO
           throw new Exception();
       }

       return new FunctionCall(identifier, arguments);
   }

   private AssignStatement? TryParseAssignStatement(string identifier)
   {
       if (!_CheckAndConsume(TokenType.ASSIGNMENT_OPERATOR)) return null;
       
       var expression = TryParseExpression();

       if (expression == null)
       {
           // TODO
           throw new Exception();
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
               // TODO
               throw new Exception();
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

               if (elseIfCondition == null || elseIfStatements == null)
               {
                   // TODO
                   throw new Exception();
               }
               
               ifElseIfStatements.Add(new ElseIfStatement(elseIfCondition, elseIfStatements));
           }
           else
           {
               var block = TryParseBlock();

               ifElseStatement = block ?? throw new Exception();
           }
       }
       
       return new IfStatement(condition, statements, ifElseIfStatements, ifElseStatement);
   }

   private (IExpression, Block) TryParseIfConditionAndBlock()
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

       if (statements == null)
       {
           // TODO
           throw new Exception();
       }

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
       
       if (statements == null)
       {
           // TODO
           throw new Exception();
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
       
       if (_comparisonOperatorMap.TryGetValue(_lexer.Token.Type, out var comparisonExpression))
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

           if (child == null)
           {
               // TODO
               throw new Exception();
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
           
       var unitType = TryParseUnitType();

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
           // TODO
           throw new Exception();
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