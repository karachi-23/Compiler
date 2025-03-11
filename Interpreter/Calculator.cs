using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lab2.LexicalAnalisator;
using lab2.GeneratorCode;


namespace Interpreter
{
  internal class Calculator
  {
    private SymbolsTable symbolsTable;
    private List<ThreeAdressCode> threeAdessCodes;

    public Calculator(SymbolsTable symbolsTable, List<ThreeAdressCode> threeAdressCodes)
    {
      this.symbolsTable = symbolsTable;
      this.threeAdessCodes = threeAdressCodes;
    }

    public void ExecuteThreeAdressCode()
    {
      foreach(var code in threeAdessCodes)
      {
        var resultToken = code.Result;

        if (code.Tokens.Count == 2)
        {
          var operand1 = code.Tokens[0];
          double value1 = GetValue(operand1);
          var type1 = operand1.Type;

          var operand2 = code.Tokens[1];
          double value2 = GetValue(operand2);
          var type2 = operand2.Type;

          if (operand1.Type == LexicalEnumType.IntegerConstant || operand1.Type == LexicalEnumType.FloatConstant)
          {
            type1 = LexicalEnumType.IntegerConstant == operand1.Type ? LexicalEnumType.IntegerVariable : LexicalEnumType.FloatVariable;
          }
          else
          {
            type1 = symbolsTable[operand1.IdentificatorID].Type;
          }

          if (operand2.Type == LexicalEnumType.IntegerConstant || operand2.Type == LexicalEnumType.FloatConstant)
          {
            type2 = LexicalEnumType.IntegerConstant == operand2.Type ? LexicalEnumType.IntegerVariable : LexicalEnumType.FloatVariable;
          }
          else
          {
            type2 = symbolsTable[operand2.IdentificatorID].Type;
          }

          double result = 0;

          switch (code.Operation)
          {
            case GenerationOperationEnum.add:
              result = value1 + value2; break;
            case GenerationOperationEnum.sub:
              result = value1 - value2; break;
            case GenerationOperationEnum.mul:
              result = value1 * value2; break;
            case GenerationOperationEnum.div:
              result = value1 / value2; break;
            default:
              Console.WriteLine("Неизвестная операция");
              break;
          }

          LexicalEnumType resType = SetResultType(type1, type2);
          symbolsTable[resultToken.IdentificatorID].Value = result;
          symbolsTable[resultToken.IdentificatorID].Type = resType;

          Console.WriteLine($"Результат операции {code.Operation}: {Math.Round(result, 2)}; тип результата: {resType}");
        }
        else if (code.Tokens.Count == 1) 
        {
          var operand = code.Tokens[0];
          double value = GetValue(operand);
          var operandType = operand.Type;

          if (operandType == LexicalEnumType.IntegerConstant || operand.Type == LexicalEnumType.FloatConstant)
          {
            operandType = LexicalEnumType.IntegerConstant == operand.Type ? LexicalEnumType.IntegerVariable : LexicalEnumType.FloatVariable;
          }
          else
          {
            operandType = symbolsTable[operand.IdentificatorID].Type;
          }

          double result = 0;
          switch (code.Operation)
          {
            case GenerationOperationEnum.i2f:
              result = value; break;
            default:
              Console.WriteLine("Неизвестная операция"); break;
          }

          LexicalEnumType resType = operandType == LexicalEnumType.IntegerVariable ? LexicalEnumType.FloatVariable : operandType;
          symbolsTable[resultToken.IdentificatorID].Value = result;
          symbolsTable[resultToken.IdentificatorID].Type = resType;

          Console.WriteLine($"Результат операции {code.Operation}: {Math.Round(result, 2)}; тип результата: {resType}");
        }
      }
    }

    private double GetValue(Token token)
    {
      if (token.Type == LexicalEnumType.IntegerConstant || token.Type == LexicalEnumType.FloatConstant)
      {
        return Convert.ToDouble(token.Value, System.Globalization.CultureInfo.InvariantCulture);
      }
      else
      {
        var symbol = symbolsTable[token.IdentificatorID];
        if(symbol.Type == LexicalEnumType.IntegerVariable || symbol.Type == LexicalEnumType.FloatVariable)
        {
          return Convert.ToDouble(symbol.Value, System.Globalization.CultureInfo.InvariantCulture);
        }
        else
        {
          return Convert.ToDouble(symbol.Value, System.Globalization.CultureInfo.InvariantCulture);
        }
      }
    }

    private LexicalEnumType SetResultType(LexicalEnumType type1, LexicalEnumType type2)
    {
      if(type1 == LexicalEnumType.IntegerVariable && type2 == LexicalEnumType.IntegerVariable)
      {
        return LexicalEnumType.IntegerVariable;
      }
      else
      {
        return LexicalEnumType.FloatVariable;
      }
    }


    
  }
}
