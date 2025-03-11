using lab2.LexicalAnalisator;
using lab2.SyntaxAnalisator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2.GeneratorCode
{
  internal class Generator
  {
    private SyntaxTree tree;
    private SymbolsTable symbolsTable;
    private int indexIdenty;
    private List<ThreeAdressCode> threeAdressCodes;
    private List<Token> postfixForm;
    private Stack<Token> availableTemp = new();

    public Generator(SyntaxTree tree, SymbolsTable symbolsTable)
    {
      this.tree = tree;
      this.symbolsTable = symbolsTable;
      this.indexIdenty = 1;
      this.threeAdressCodes = new List<ThreeAdressCode>();
      this.postfixForm = new List<Token>();

    }

    public List<ThreeAdressCode> GenerateThreeAdressCode(bool isOpt)
    {
      //var code = new StringBuilder();
      CreateThreeAdressCode(tree.Root, isOpt);
      return this.threeAdressCodes;
    }

    private Token CreateThreeAdressCode(SyntaxNode syntaxNode, bool isOpt)
    {
      if (syntaxNode == null)
      {
        return null;
      }
      if (IsOperator(syntaxNode.Token) && syntaxNode.Children.Count == 2)
      {
        var leftNode = CreateThreeAdressCode(syntaxNode.Children[0], isOpt);
        var rightNode = CreateThreeAdressCode(syntaxNode.Children[1], isOpt);
        Token tempVar;
        if (syntaxNode.Type == LexicalEnumType.Identifier)
        {
          tempVar = GetTempVariable(syntaxNode.Token.getSubType(symbolsTable));
        }
        else
        {
          tempVar = GetTempVariable(syntaxNode.Type);
        }

        ThreeAdressCode threeAdressCode = new ThreeAdressCode(syntaxNode.Token.Type, tempVar, new List<Token> { leftNode, rightNode });
        this.threeAdressCodes.Add(threeAdressCode);

        if (isOpt)
        {
          ReleaseTemporaryVariable(leftNode);
          ReleaseTemporaryVariable(rightNode);
        }

        return tempVar;
        /*int idTempVar = 0;
        if (syntaxNode.Type == LexicalEnumType.Identifier)
        {
          idTempVar = symbolsTable.addSymbols($"#T{indexIdenty++}", syntaxNode.Token.getSubType(symbolsTable));
        }
        else
        {
          idTempVar = symbolsTable.addSymbols($"#T{indexIdenty++}", syntaxNode.Type);
        }
       
        Token tempVar = new Token(LexicalEnumType.Identifier, $"#T{indexIdenty++}", idTempVar);
        ThreeAdressCode threeAdressCode = new ThreeAdressCode(syntaxNode.Token.Type, tempVar, new List<Token> { leftNode, rightNode });
        this.threeAdressCodes.Add(threeAdressCode);
        return tempVar;*/
      }
      else if (syntaxNode.Token.Type == LexicalEnumType.Coercion && syntaxNode.Children.Count == 1)
      {
        Token operandToken = CreateThreeAdressCode(syntaxNode.Children[0], isOpt);

        if (operandToken.getSubType(symbolsTable) == syntaxNode.Token.getSubType(symbolsTable))
        {
          return operandToken;
        }

        Token tempVar = GetTempVariable(syntaxNode.Token.getSubType(symbolsTable));
        ThreeAdressCode threeAdressCode = new ThreeAdressCode(syntaxNode.Token.Type, tempVar, new List<Token> { operandToken });

        if (isOpt)
        {
          ReleaseTemporaryVariable(operandToken);
        }

        threeAdressCodes.Add(threeAdressCode);

        return tempVar;
        /*int idTempVar = symbolsTable.addSymbols($"#T{indexIdenty++}", LexicalEnumType.FloatVariable);
        Token tempVar = new Token(LexicalEnumType.Identifier, $"#T{indexIdenty++}", idTempVar);

        ThreeAdressCode threeAdressCode = new ThreeAdressCode(syntaxNode.Token.Type, tempVar, new List<Token> { operandToken });
        this.threeAdressCodes.Add(threeAdressCode) ;
        return tempVar;*/
      }
      else if (syntaxNode.Token.Type == LexicalEnumType.Identifier || 
        syntaxNode.Token.Type == LexicalEnumType.IntegerConstant ||
        syntaxNode.Token.Type == LexicalEnumType.FloatConstant)
      {
        return syntaxNode.Token;
      }
      return null;
    }

    public List<Token> GeneratorPostfixForm()
    {
      //var postfixForm = new StringBuilder();
      CreatePostfixForm(tree.Root);
      return this.postfixForm;
    }

    private void CreatePostfixForm(SyntaxNode syntaxNode)
    {
      if (syntaxNode == null)
        return;

      if (IsOperator(syntaxNode.Token))
      {
        if (syntaxNode.Children.Count == 2)
        {
          CreatePostfixForm(syntaxNode.Children[0]);
          CreatePostfixForm(syntaxNode.Children[1]);
          postfixForm.Add(syntaxNode.Token);
        }
        else if(syntaxNode.Children.Count == 1)
        {
          CreatePostfixForm(syntaxNode.Children[0]);
          postfixForm.Add(syntaxNode.Token);
        }
      }
      else if (syntaxNode.Token.Type == LexicalEnumType.Identifier || syntaxNode.Token.Type == LexicalEnumType.IntegerConstant
        || syntaxNode.Token.Type == LexicalEnumType.FloatConstant)
      {
        postfixForm.Add(syntaxNode.Token);
      }
    }

    private bool IsOperator(Token token)
    {
      return token.Type == LexicalEnumType.AdditionOperation ||
            token.Type == LexicalEnumType.SubtractionOperation ||
            token.Type == LexicalEnumType.MultiplicationOperation ||
            token.Type == LexicalEnumType.DivisionOperation ||
            token.Type == LexicalEnumType.Coercion;
    }

    private Token GetTempVariable(LexicalEnumType type)
    {
      if (type == LexicalEnumType.Unknown)
      {
        type = LexicalEnumType.IntegerVariable;
      } else if (type == LexicalEnumType.Coercion)
      {
        type = LexicalEnumType.FloatVariable;
      }
      if(availableTemp.Count > 0)
      {
        var tempVar = availableTemp.Pop();

        if(tempVar.Type == LexicalEnumType.Identifier && tempVar.Value.StartsWith("#T"))
        {
          symbolsTable[tempVar.IdentificatorID].Type = type;
          return tempVar;
        }
      }

      string tempVarName = $"#T{indexIdenty++}";
      LexicalEnumType resolvedType = type;   
      int idTempVar = symbolsTable.addSymbols(tempVarName, resolvedType);
      var newTempVar = new Token(LexicalEnumType.Identifier, tempVarName, idTempVar);
      return newTempVar;
    }

    private void ReleaseTemporaryVariable(Token tempVar)
    {
      if (tempVar != null &&
          tempVar.Type == LexicalEnumType.Identifier &&
          tempVar.Value is string name &&
          name.StartsWith("#T")) // Проверяем, что это временная переменная
      {
        availableTemp.Push(tempVar);
      }
    }
  }
}
