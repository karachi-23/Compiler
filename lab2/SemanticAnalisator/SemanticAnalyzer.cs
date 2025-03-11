using lab2.LexicalAnalisator;
using lab2.SyntaxAnalisator;
using System;
using System.Collections.Generic;

namespace lab2.SemanticAnalisator
{
  internal class SemanticAnalyzer
  {
    private readonly SymbolsTable _symbolsTable;

    public SemanticAnalyzer(SyntaxTree tree, SymbolsTable symbolsTable)
    {
      _symbolsTable = symbolsTable;
      AnalyzeNode(tree.Root);
    }

    private void AnalyzeNode(SyntaxNode node)
    {
      if(node == null) return;
      //Проверка типов и при необходимости приведение
      EnsureTypeCompatibility(node);

      foreach(var child in node.Children)
      {
        AnalyzeNode(child);
      }
    }

    private void EnsureTypeCompatibility(SyntaxNode node)
    {
      if(node == null || node.Children.Count<2) return;

      var leftNode = node.Children[0];
      var rightNode = node.Children[1];

      if (node.Token.Type == LexicalEnumType.DivisionOperation && rightNode != null)
      {
        if (rightNode.Token.Type == LexicalEnumType.IntegerConstant && rightNode.Token.Value.Equals("0"))
        {
          throw new SemanticException("Семантическая ошибка. Деление на ноль");
        }
        else if (rightNode.Token.Type == LexicalEnumType.FloatConstant && rightNode.Token.Value.Equals("0.0"))
        {
          throw new SemanticException("Семантическая ошибка. Деление на ноль");
        }
      }

      EnsureTypeCompatibility(leftNode);
      EnsureTypeCompatibility(rightNode);
      var leftType = LexicalEnumType.IntegerConstant;
      var rightType = LexicalEnumType.IntegerConstant;

      if (leftNode.Token.Type == LexicalEnumType.Identifier)
      {
        leftType = _symbolsTable[leftNode.Token.IdentificatorID].Type;
      } else if (leftNode.Token.Type == LexicalEnumType.IntegerConstant)
      {
        leftType = LexicalEnumType.IntegerVariable;
      } else if (leftNode.Token.Type == LexicalEnumType.FloatConstant)
      {
        leftType = LexicalEnumType.FloatVariable;
      }
      else
      {
        leftType = leftNode.Type;
      }

      if (rightNode.Token.Type == LexicalEnumType.Identifier)
      {
        rightType = _symbolsTable[rightNode.Token.IdentificatorID].Type;
      }
      else if (rightNode.Token.Type == LexicalEnumType.IntegerConstant)
      {
        rightType = LexicalEnumType.IntegerVariable;
      }
      else if (rightNode.Token.Type == LexicalEnumType.FloatConstant)
      {
        rightType = LexicalEnumType.FloatVariable;
      }
      else
      {
        rightType = rightNode.Type;
      }


      /* var leftType = leftNode.Token.Type == LexicalEnumType.Identifier
         ? _symbolsTable[leftNode.Token.IdentificatorID].Type
         : leftNode.Token.Type;

       var rightType = rightNode.Token.Type == LexicalEnumType.Identifier
         ? _symbolsTable[rightNode.Token.IdentificatorID].Type
         : rightNode.Token.Type;
*/
      if (leftType != rightType)
      {
        SyntaxNode coercionNode = new SyntaxNode(new Token(LexicalEnumType.Coercion, LexicalEnumType.Coercion.ToDescribString()));
        if (leftType == LexicalEnumType.IntegerVariable && rightType == LexicalEnumType.FloatVariable)
        {
          coercionNode.Children.Add(leftNode);
          node.Children[0] = coercionNode;
        }
        else if (rightType == LexicalEnumType.IntegerVariable && leftType == LexicalEnumType.FloatVariable)
        {
          coercionNode.Children.Add(rightNode);
          node.Children[1] = coercionNode;
        }
      }

          node.Type = (leftType == LexicalEnumType.FloatVariable || rightType == LexicalEnumType.FloatVariable)
            ? LexicalEnumType.FloatVariable
            : LexicalEnumType.IntegerVariable;
        }
      }
    }
