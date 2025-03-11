using lab2.LexicalAnalisator;
using System.Collections.Generic;

namespace lab2.SyntaxAnalisator
{
  internal class SyntaxParser
  {
    private readonly List<Token> _tokens;
    private int _currentTokenIndex;

    public SyntaxParser(List<Token> tokens)
    {
      _tokens = tokens;
      _currentTokenIndex = 0;
    }

    public SyntaxTree Parse()
    {
      var root = ParseExpression();

      if (_currentTokenIndex < _tokens.Count)
      {
        throw new SyntaxException("Лишние токены после завершения разбора выражения");
      }

      return new SyntaxTree(root);
    }

    private SyntaxNode ParseExpression()
    {
      var node = ParseTerm();

      while (_currentTokenIndex < _tokens.Count &&
             (_tokens[_currentTokenIndex].Type == LexicalEnumType.AdditionOperation ||
              _tokens[_currentTokenIndex].Type == LexicalEnumType.SubtractionOperation))
      {
        var operatorNode = new SyntaxNode(_tokens[_currentTokenIndex]);
        _currentTokenIndex++;
        var rightNode = ParseTerm();

        operatorNode.Children.Add(node);
        operatorNode.Children.Add(rightNode);
        node = operatorNode;
      }

      return node;
    }

    private SyntaxNode ParseTerm()
    {
      var node = ParseFactor();

      while (_currentTokenIndex < _tokens.Count &&
             (_tokens[_currentTokenIndex].Type == LexicalEnumType.MultiplicationOperation ||
              _tokens[_currentTokenIndex].Type == LexicalEnumType.DivisionOperation))
      {
        var operatorNode = new SyntaxNode(_tokens[_currentTokenIndex]);
        _currentTokenIndex++;
        var rightNode = ParseFactor();

        operatorNode.Children.Add(node);
        operatorNode.Children.Add(rightNode);
        node = operatorNode;
      }

      return node;
    }

    private SyntaxNode ParseFactor()
    {
      if (_currentTokenIndex >= _tokens.Count)
      {
        throw new SyntaxException("Ожидался токен, но список токенов завершен");
      }

      Token token = _tokens[_currentTokenIndex];
      _currentTokenIndex++;

      switch (token.Type)
      {
        case LexicalEnumType.IntegerConstant:
        case LexicalEnumType.FloatConstant:
        case LexicalEnumType.Identifier:
          if (_currentTokenIndex < _tokens.Count &&
              _tokens[_currentTokenIndex].Type == LexicalEnumType.Identifier)
          {
            throw new SyntaxException($"Синтаксическая ошибка! Два идентификатора идут подряд на позиции {_currentTokenIndex - 1}.");
          }
          return new SyntaxNode(token);
        case LexicalEnumType.OpeningBracket:
          var node = ParseExpression();
          if (_currentTokenIndex >= _tokens.Count || _tokens[_currentTokenIndex].Type != LexicalEnumType.ClosingBracket)
          {
            throw new SyntaxException($"Синтаксическая ошибка! У открывающей скобки <{token.Value}> на позиции {_currentTokenIndex} отсутствует закрывающая скобка.");
          }
          _currentTokenIndex++;
          return node;
        default:
          throw new SyntaxException($"Синтаксическая ошибка! Неизвестный токен <{token.Value}> на позиции {_currentTokenIndex}.");
      }
    }
  }
}
