using lab2.LexicalAnalisator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2.GeneratorCode
{
  internal static class GenerationOperatorsFromLexicalTypes
  {

    public static GenerationOperationEnum ToGetOperation(this LexicalEnumType type)
    {
      return type switch
      {
        LexicalEnumType.AdditionOperation => GenerationOperationEnum.add,
        LexicalEnumType.SubtractionOperation => GenerationOperationEnum.sub,
        LexicalEnumType.MultiplicationOperation => GenerationOperationEnum.mul,
        LexicalEnumType.DivisionOperation => GenerationOperationEnum.div,
        LexicalEnumType.Coercion => GenerationOperationEnum.i2f,
        _ => GenerationOperationEnum.other,
      };
    }


    public static LexicalEnumType ToSetOperation(this GenerationOperationEnum type)
    {
      return type switch
      {
        GenerationOperationEnum.add => LexicalEnumType.AdditionOperation,
        GenerationOperationEnum.sub => LexicalEnumType.SubtractionOperation,
        GenerationOperationEnum.mul => LexicalEnumType.MultiplicationOperation,
        GenerationOperationEnum.div => LexicalEnumType.DivisionOperation,
        GenerationOperationEnum.i2f => LexicalEnumType.Coercion,
        _ => LexicalEnumType.Unknown,
      };
    }
  }
}
