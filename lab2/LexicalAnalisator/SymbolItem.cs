using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2.LexicalAnalisator
{
    public class SymbolItem
    {
        public string Name { get; set; }
        public LexicalEnumType Type { get; set; }

        public dynamic Value { get; set; }

        public SymbolItem(string name, LexicalEnumType type = LexicalEnumType.IntegerVariable, dynamic value = null)
        {
            Name = name;
            Type = type;
            Value = value;
        }

        public void Serialize(BinaryWriter writer)
        {
          writer.Write(Name);
          writer.Write((int)Type);
        }

        public static SymbolItem Deserialize(BinaryReader reader)
        {
          string name = reader.ReadString();
          LexicalEnumType type = (LexicalEnumType)reader.ReadInt32();
          return new SymbolItem(name, type);  

        }

        public override string ToString()
        {
            return $"{Name} ({Type})";
        }

    }
}
