using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2.LexicalAnalisator
{
    public class SymbolsTable
    {
        Dictionary<int, SymbolItem> _symbols;

        public int count;

        public SymbolsTable()
        {
            _symbols = new Dictionary<int, SymbolItem>();
        }

        public int addSymbols(string identify, LexicalEnumType lexicalEnumType = LexicalEnumType.IntegerVariable)
        {
            foreach (var symbol in _symbols)
            {
                if (symbol.Value.Name == identify)
                {
                if (symbol.Value.Type != lexicalEnumType)
                {
                  throw new Exception($"Ошибка: Идентификатор \"{identify}\" уже существует с другим типом ({symbol.Value.Type.ToDescribString()}).");
                }
                return symbol.Key;
                }
            }
            _symbols[count] = new SymbolItem(identify, lexicalEnumType);
            return count++;
        }

        public SymbolItem this[int id]
        {
          get
          {
            if (_symbols.ContainsKey(id))
              return _symbols[id];
            throw new KeyNotFoundException("Ключ в словаре не найден");
          }
        }

        public Dictionary<int, SymbolItem> GetSymbols()
        {
          return _symbols;
        }

    public bool RemoveSymbolByName(string name)
    {
      // Находим ключ по имени
      var keyToRemove = _symbols.FirstOrDefault(x => x.Value.Name == name).Key;

      // Если ключ найден, удаляем запись
      if (keyToRemove != null)
      {
        return _symbols.Remove(keyToRemove);
      }

      // Если элемент с таким именем не найден
      return false;
    }

    //Сериализация
    public void Serialize(BinaryWriter writer)
    {
      writer.Write(_symbols.Count);
      foreach (var symbol in _symbols)
      {
        writer.Write(symbol.Key);
        symbol.Value.Serialize(writer);
      }
    }

    public static SymbolsTable Desiarialize(BinaryReader reader)
    {
      var symbolTable = new SymbolsTable();
      int count = reader.ReadInt32();

      for (int i = 0; i < count; i++)
      {
        int key = reader.ReadInt32();
        var value = SymbolItem.Deserialize(reader);
        symbolTable._symbols[key] = value;
      }
      return symbolTable;
    }

    public override string ToString()
        {
          StringBuilder sb = new StringBuilder();
          foreach (var symbol in _symbols)
          {
            string variableType = symbol.Value.Type switch
            {
              LexicalEnumType.IntegerVariable => "целого типа",
              LexicalEnumType.FloatVariable => "вещественного типа",
              _ => "неизвестного типа"
            };
        sb.Append($"{symbol.Key} - {symbol.Value.Name} ({variableType})").Append("\n");
              }
              return sb.ToString();
        }

    }
}
