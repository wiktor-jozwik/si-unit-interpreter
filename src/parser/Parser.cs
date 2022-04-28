using si_unit_interpreter.parser.expression;
using si_unit_interpreter.parser.expression.additive;
using si_unit_interpreter.parser.expression.comparison;
using si_unit_interpreter.parser.expression.multiplicative;
using si_unit_interpreter.parser.expression.negate;
using si_unit_interpreter.parser.statement;
using Expression = System.Linq.Expressions.Expression;

namespace si_unit_interpreter.parser;

public class Parser
{
    private Lexer _lexer;
    
    private readonly Dictionary<TokenType, Func<IExpression, IExpression, IExpression>> _comparisonOperatorMap;
    private readonly Dictionary<TokenType, Func<IExpression, IExpression, IExpression>> _additiveOperatorMap;
    private readonly Dictionary<TokenType, Func<IExpression, IExpression, IExpression>> _multiplicativeOperatorMap;
    private readonly Dictionary<TokenType, Func<IExpression, IExpression>> _negateOperatorMap;
    
    private IDictionary<string, IStatement> _statements = new Dictionary<string, IStatement>();

    public Parser(Lexer lexer)
    {
        _lexer = lexer;
        lexer.GetNextToken();
        
        _comparisonOperatorMap = new Dictionary<TokenType, Func<IExpression, IExpression, IExpression>>
        {
            [TokenType.GREATER_THAN_OPERATOR] = (left, right)=> new GreaterThanExpression(left, right),
            [TokenType.SMALLER_THAN_OPERATOR] = (left, right)=> new SmallerThanExpression(left, right),
            [TokenType.GREATER_EQUAL_THAN_OPERATOR] = (left, right)=> new GreaterThanExpression(left, right),
            [TokenType.SMALLER_EQUAL_THAN_OPERATOR] = (left, right)=> new SmallerThanExpression(left, right),
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
            [TokenType.MINUS_OPERATOR] = (child)=> new MinusExpression(child),
            [TokenType.NEGATE_OPERATOR] = (child)=> new NotExpression(child),
        };
    }
    
   public Program Parse()
    {

        return new Program(_statements);
    }

   private IExpression? TryParseExpression()
   {
       var leftLogicFactor = TryParseLogicFactor();

       if (leftLogicFactor == null)
       {
           return null;
       }

       while (_CheckAndConsume(TokenType.OR_OPERATOR))
       {
           var rightLogicFactor = TryParseLogicFactor();
           if (rightLogicFactor == null)
           {
               // TODO
               throw new Exception();
           }

           leftLogicFactor = new expression.Expression(leftLogicFactor, rightLogicFactor);
       }

       return leftLogicFactor;
   }

   private IExpression? TryParseLogicFactor()
   {
       var leftExpressionComparison = TryParseExpressionComparison();

       if (leftExpressionComparison == null)
       {
           return null;
       }

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
       
       if (leftAdditiveExpression == null)
       {
           return null;
       }
       
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
       
       if (leftMultiplicativeExpression == null)
       {
           return null;
       }

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
       
       if (leftUnaryExpression == null)
       {
           return null;
       }

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
           var child = TryParsePrimaryExpression();

           return child == null ? null : negateExpression(child);
       }

       return TryParseExpression();
   }

   private IExpression? TryParsePrimaryExpression()
   {
       // Literal
       if (_TokenIs(TokenType.FLOAT) || _TokenIs(TokenType.INT) || _TokenIs(TokenType.STRING)) return new Literal(_lexer.Token.Value);
       if (_TokenIs(TokenType.TRUE)) return new Literal(true);
       if (_TokenIs(TokenType.FALSE)) return new Literal(false);

       // Identifier or function call
       if (_TokenIs(TokenType.IDENTIFIER))
       {
           var identifierName = _lexer.Token.Value;
           _lexer.GetNextToken();

           if (_CheckAndConsume(TokenType.LEFT_PARENTHESES))
           {
               return TryParseFunctionCall(identifierName);
           }
           
           return new Identifier(identifierName);
       }

       if (_CheckAndConsume(TokenType.LEFT_PARENTHESES))
       {
           // Expression
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

   private IExpression TryParseFunctionCall(string functionName)
   {
       var arguments = TryParseArguments();

       if (_CheckAndConsume(TokenType.RIGHT_PARENTHESES))
       {
           return new FunctionCall(functionName, arguments);
       }

       // TODO
       throw new Exception();
   }

   private List<IExpression> TryParseArguments()
   {
       var arguments = new List<IExpression>();
       var argument = TryParseExpression();

       if (argument == null)
       {
           return arguments;
       }
       
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
   

   private bool _CheckAndConsume(TokenType type)
   {
       if (!_TokenIs(type))
       {
           return false;
       }
       _lexer.GetNextToken();
       return true;
   }

   private bool _TokenIs(TokenType type)
   {
       return _lexer.Token.Type == type;
   }
}