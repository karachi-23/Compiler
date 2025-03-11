using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2.LexicalAnalisator
{
    public class Token
    {
        public LexicalEnumType Type { get; set;  }
       // public string Value { get; }
       public dynamic Value { get; }

        public int IdentificatorID { get; set; }
        
        private LexicalEnumType LexicalEnumType { get; }

        //public LexicalEnumType? SubType { get; set; }

       
      

        public Token(LexicalEnumType type, dynamic value, int id = -1)
        {
            Type = type;
            Value = value;
            IdentificatorID = id;
            
        }

        public LexicalEnumType getSubType(SymbolsTable symbolsTable)
        {
          if (Type == LexicalEnumType.IntegerConstant) return LexicalEnumType.IntegerVariable;
          else if (Type == LexicalEnumType.FloatConstant) return LexicalEnumType.FloatVariable;
          else if (Type == LexicalEnumType.Identifier) return symbolsTable[symbolsTable.addSymbols(Value)].Type;
          else if (Type == LexicalEnumType.Coercion) return LexicalEnumType.Coercion;
          else return LexicalEnumType.Unknown;
        }

    public void Serialize(BinaryWriter writer)
    {
      writer.Write((int)Type);
      WriteValue(writer, Value);
      writer.Write(IdentificatorID);
    }

    public static Token Deserialize(BinaryReader reader)
    {
      LexicalEnumType type = (LexicalEnumType)reader.ReadInt32();
      dynamic value = ReadValue(reader);
      int identifierID = reader.ReadInt32();
      return new Token(type, value, identifierID);
    }

    private static void WriteValue(BinaryWriter writer, dynamic value)
    {
      if (value is int)
      {
        writer.Write((byte)0);
        writer.Write((int)value);
      }
      else if (value is double)
      {
        writer.Write((byte)1);
        writer.Write((double)value);
      }
      else if (value is string)
      {
        writer.Write((byte)2);
        writer.Write((string)value);
      }
      else
      {
        throw new InvalidCastException($"Unsupported value type: {value.GetType()}");
      }
    }

    private static dynamic ReadValue(BinaryReader reader)
    {
      byte typeIndicator = reader.ReadByte(); // Читаем признак типа

      switch (typeIndicator)
      {
        case 0:
          return reader.ReadInt32(); // Читаем int
        case 1:
          return reader.ReadDouble(); // Читаем double
        case 2:
          return reader.ReadString(); // Читаем string
        default:
          throw new InvalidCastException($"Unsupported value type indicator: {typeIndicator}");
      }
    }

    public override string ToString()
        {
  
          return Type == LexicalEnumType.Identifier
          ? $"<{Type.ToDescribString()},{IdentificatorID}> - {Type.ToDetailedString()} {Value}"
          : $"<{Value}> - {Type.ToDetailedString()}";

    }
    }
}
