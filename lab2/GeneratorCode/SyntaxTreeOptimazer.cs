using lab2.LexicalAnalisator;
using lab2.SyntaxAnalisator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2.GeneratorCode
{
  internal class SyntaxTreeOptimazer
  {
    public static SyntaxTree OptimizeTree(SyntaxTree tree)
    {
      OptimizeNode(tree?.Root);
      return tree;
    }

    private static void OptimizeNode(SyntaxNode node)
    {
      if (node == null) return;

      foreach (var child in node.Children)
      {
        OptimizeNode(child);
      }

      OptimizeExpression(node);
    }

    private static void OptimizeExpression(SyntaxNode node)
    {
      if (IsOperator(node.Token) && node.Children.Count == 2)
      {
        var leftChild = node.Children[0];
        var rightChild = node.Children[1];

        //Если константы считаем выражение сразу
        if (IsConstant(leftChild.Token) && IsConstant(rightChild.Token))
        {
          double leftValue = Convert.ToDouble(leftChild.Token.Value, System.Globalization.CultureInfo.InvariantCulture);
          double rightValue = Convert.ToDouble(rightChild.Token.Value, System.Globalization.CultureInfo.InvariantCulture);
          double result = 0;

          LexicalEnumType resType = LexicalEnumType.IntegerConstant;

          if (leftChild.Token.Type == LexicalEnumType.FloatConstant || rightChild.Token.Type == LexicalEnumType.FloatConstant)
          {
            resType = LexicalEnumType.FloatConstant;
          }

          switch (node.Token.Type)
          {
            case LexicalEnumType.AdditionOperation:
              result = leftValue + rightValue;
              break;
            case LexicalEnumType.SubtractionOperation:
              result = leftValue - rightValue; break;
            case LexicalEnumType.MultiplicationOperation:
              result = leftValue * rightValue; break;
            case LexicalEnumType.DivisionOperation:
              if (rightValue != 0)
              {
                result = leftValue / rightValue;
                if (result.GetType() == typeof(double))
                {
                  resType = LexicalEnumType.FloatConstant;
                }
              }
              else
              {
                // Обработка деления на 0 (можно, например, выбросить ошибку или заменить на индикацию ошибки)
                result = double.NaN;
                resType = LexicalEnumType.Unknown;
              }
              break;
            default:
              break;
          }

          if (resType == LexicalEnumType.FloatConstant)
          {
            node.Token = new Token(LexicalEnumType.FloatConstant, result, -1);
          }
          else
          {
            node.Token = new Token(LexicalEnumType.IntegerConstant, (int)result, -1);
          }

          node.Children.Clear();
        }
        else if (leftChild.Token.Type == LexicalEnumType.Coercion && leftChild.Children.Count == 1)
        {
          leftChild = leftChild.Children[0];
        }
        else if (rightChild.Token.Type == LexicalEnumType.Coercion && rightChild.Children.Count == 1)
        {
          rightChild = rightChild.Children[0];
        }
      }


      // Оптимизация преобразований int2float
      if (node.Token.Type == LexicalEnumType.Coercion && node.Children.Count == 1)
      {
        var child = node.Children[0];
        if (IsConstant(child.Token) && child.Token.Type == LexicalEnumType.IntegerConstant)
        {
          double floatValue = Convert.ToDouble(child.Token.Value);
          node.Token = new Token(LexicalEnumType.FloatConstant, floatValue, -1);
          node.Children = child.Children;
        }
      }

      if ((node.Token.Type == LexicalEnumType.AdditionOperation && node.Children.Count == 2) || (node.Token.Type == LexicalEnumType.SubtractionOperation && node.Children.Count == 2))
      {
        var leftChild = node.Children[0];
        var rightChild = node.Children[1];

        // A +/- 0 -> A
        if (IsConstant(rightChild.Token) && Convert.ToDouble(rightChild.Token.Value) == 0)
        {
          node.Token = leftChild.Token;
          node.Children = leftChild.Children;
        }
        else if (IsConstant(leftChild.Token) && Convert.ToDouble(leftChild.Token.Value) == 0)
        {
          node.Token = rightChild.Token;
          node.Children = rightChild.Children;
        }
      }
      else if (node.Token.Type == LexicalEnumType.MultiplicationOperation && node.Children.Count == 2)
      {
        var leftChild = node.Children[0];
        var rightChild = node.Children[1];

        // A * 0 -> 0
        if (IsConstant(rightChild.Token) && Convert.ToDouble(rightChild.Token.Value, System.Globalization.CultureInfo.InvariantCulture) == 0)
        {
          node.Token = new Token(rightChild.Token.Type, 0, -1);
          node.Children.Clear();
        }
        else if (IsConstant(leftChild.Token) && Convert.ToDouble(leftChild.Token.Value, System.Globalization.CultureInfo.InvariantCulture) == 0)
        {
          node.Token = new Token(leftChild.Token.Type, 0, -1);
          node.Children.Clear();
        }
        // A * 1 -> A
        else if (IsConstant(rightChild.Token) && Convert.ToDouble(rightChild.Token.Value, System.Globalization.CultureInfo.InvariantCulture) == 1)
        {
          node.Token = leftChild.Token;
          node.Children = leftChild.Children;
        }
        else if (IsConstant(leftChild.Token) && Convert.ToDouble(leftChild.Token.Value, System.Globalization.CultureInfo.InvariantCulture) == 1)
        {
          node.Token = rightChild.Token;
          node.Children = rightChild.Children;
        }
      }
      else if (node.Token.Type == LexicalEnumType.DivisionOperation && node.Children.Count == 2)
      {
        var leftChild = node.Children[0];
        var rightChild = node.Children[1];

        // A / 1 -> A
        if (IsConstant(rightChild.Token) && Convert.ToDouble(rightChild.Token.Value, System.Globalization.CultureInfo.InvariantCulture) == 1)
        {
          node.Token = leftChild.Token;
          node.Children = leftChild.Children;
        }
        //0 / A -> 0
        else if (IsConstant(leftChild.Token) && Convert.ToDouble(leftChild.Token.Value, System.Globalization.CultureInfo.InvariantCulture) == 0)
        {
          node.Token = new Token(leftChild.Token.Type, 0, -1);
          node.Children.Clear();
        }
      }
    }



    private static bool IsOperator(Token token)
    {
      return token.Type == LexicalEnumType.AdditionOperation ||
            token.Type == LexicalEnumType.SubtractionOperation ||
            token.Type == LexicalEnumType.MultiplicationOperation ||
            token.Type == LexicalEnumType.DivisionOperation ||
            token.Type == LexicalEnumType.Coercion;
    }

    private static bool IsConstant(Token token)
    {
      return token.Type == LexicalEnumType.IntegerConstant || token.Type == LexicalEnumType.FloatConstant;
    }
  }
}
