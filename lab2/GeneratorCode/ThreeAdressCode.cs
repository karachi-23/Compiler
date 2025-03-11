using lab2.GeneratorCode;
using lab2.LexicalAnalisator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2.GeneratorCode
{
  public class ThreeAdressCode
  {
    public GenerationOperationEnum Operation { get; set; }
    public Token Result { get; set; }
    public List<Token> Tokens { get; set; }

    public ThreeAdressCode(LexicalEnumType type, Token result, List<Token> tokens)
    {
      Operation = SetOperation(type);
      //Operation = operation;
      Result = result;
      Tokens = tokens;
    }

    private GenerationOperationEnum SetOperation(LexicalEnumType type)
    {
      if (type == LexicalEnumType.AdditionOperation)
      {
        Operation = GenerationOperationEnum.add;
      }
      else if (type == LexicalEnumType.SubtractionOperation)
      {
        Operation = GenerationOperationEnum.sub;
      }
      else if (type == LexicalEnumType.MultiplicationOperation)
      {
        Operation = GenerationOperationEnum.mul;
      }
      else if (type == LexicalEnumType.DivisionOperation)
      {
        Operation = GenerationOperationEnum.div;
      }
      else if (type == LexicalEnumType.Coercion)
      {
        Operation = GenerationOperationEnum.i2f;
      }
      else if (type == LexicalEnumType.Unknown)
      {
        Operation = GenerationOperationEnum.other;
      }
      return Operation;
    }

    public void Serialize(BinaryWriter writer)
    {
      writer.Write((int)Operation);
      Result.Serialize(writer);
      writer.Write(Tokens.Count);
      foreach (var token in Tokens)
      {
        token.Serialize(writer);
      }
    }

    public static ThreeAdressCode Deserialize(BinaryReader reader)
    {
      GenerationOperationEnum operation = (GenerationOperationEnum)reader.ReadInt32();
      Token res = Token.Deserialize(reader);

      int tokenCount = reader.ReadInt32();
      List<Token> tokens = new List<Token>();
      for (int i = 0; i < tokenCount; i++)
      {
        tokens.Add(Token.Deserialize(reader));
      }

      return new ThreeAdressCode(GenerationOperatorsFromLexicalTypes.ToSetOperation(operation), res, tokens);
    }
    


    public override string ToString()
    {
      if (Tokens.Count == 1)
      {
        return $"{Operation} {Result.ToString().Split(" - ")[0]} {Tokens[0].ToString().Split(" - ")[0]}";
      }
      else
      {
        return $"{Operation} {Result.ToString().Split(" - ")[0]} {Tokens[0].ToString().Split(" - ")[0]} {Tokens[1].ToString().Split(" - ")[0]}";
      }
    }
  }
}
