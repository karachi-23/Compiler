using lab2.GeneratorCode;
using lab2.LexicalAnalisator;

namespace Interpreter
{
  internal class Runner
  {
    static void Main(string[] args)
    {
      SymbolsTable deserializedSymbolTable;
      List<ThreeAdressCode> deserializedThreeAdressCode;

      string filePath = @"C:\RGRTU\7 семак\Разработка компиляторов\Лабы\lab1\lab2\bin\Debug\net8.0\post_code.bin";
      using (FileStream fs = new FileStream(filePath, FileMode.Open))
      using (BinaryReader reader = new BinaryReader(fs))
      {
        deserializedSymbolTable = SymbolsTable.Desiarialize(reader);
        Console.WriteLine("Десериализованная таблица символов:");
        foreach (var symbol in deserializedSymbolTable.GetSymbols())
        {
          Console.WriteLine($"{symbol.Key}: {symbol.Value}");
        }

        int threeAdressCodeCount = reader.ReadInt32();//Количество строк трехадресного кода
        deserializedThreeAdressCode = new List<ThreeAdressCode>();
        for (int i = 0; i < threeAdressCodeCount; i++)
        {
          deserializedThreeAdressCode.Add(ThreeAdressCode.Deserialize(reader));
        }
        Console.WriteLine("Десериализация трехадресного кода:");
        foreach (var code in deserializedThreeAdressCode)
        {
          Console.WriteLine(code);
        }
      }

      foreach (var symbol in deserializedSymbolTable.GetSymbols())
      {
        if (!symbol.Value.Name.StartsWith("#T"))
        {
          bool validInput = false;
          while (!validInput)
          {
            Console.WriteLine($"Введите значение переменной {symbol.Value.Name} (тип переменной:{symbol.Value.Type}): ");
            string input = Console.ReadLine();

            try
            {
              if (symbol.Value.Type == LexicalEnumType.IntegerVariable)
              {
                if (int.TryParse(input, System.Globalization.CultureInfo.InvariantCulture, out int intValue))
                {
                  symbol.Value.Value = intValue;
                  validInput = true;
                }
                else
                {
                  Console.WriteLine("Тип символа не соответствует заданному. Введите целое число.");
                }
              }
              else if (symbol.Value.Type == LexicalEnumType.FloatVariable || symbol.Value.Type == LexicalEnumType.FloatConstant)
              {
                if (double.TryParse(input, System.Globalization.CultureInfo.InvariantCulture, out double doubleValue))
                {
                  symbol.Value.Value = doubleValue;
                  validInput = true;
                }
                else
                {
                  Console.WriteLine("Тип символа не соответствует заданному. Введите число с плавающей точкой.");
                }
              }
              else
              {
                symbol.Value.Value = input;
                validInput = true;
              }
            }
            catch (Exception ex)
            {
              Console.WriteLine($"Ошибка:{ex.Message}. Попробуйте еще раз.");
            }
          }
          Console.WriteLine($"Для {symbol.Value.Name} установлено значение: {symbol.Value.Value}");
        }
      }
      Calculator calc = new Calculator(deserializedSymbolTable, deserializedThreeAdressCode);
      calc.ExecuteThreeAdressCode();
    }
  }
}
