using lab2.GeneratorCode;
using lab2.LexicalAnalisator;
using lab2.SyntaxAnalisator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab.lab2
{
  internal class WorkWithFile
  {
    public static string ReadFile(string parFile)
    {
      try
      {
        //return File.ReadAllText(parFile).Replace(" ", "").Replace("\t", "");
        return File.ReadAllText(parFile);
      }
      catch
      {
        throw new FileException("Не удалось прочитать файл");
      }
    }
    public static void WriteFile(string parFile, string parString)
    {
      File.WriteAllText(parFile, parString);
      // File.AppendAllText(parFile, parString + Environment.NewLine);
    }


    public static void WriteToFile(string filename, SyntaxTree syntaxTree)
    {
      using (var writer = new StreamWriter(filename))
      {
        writer.Write(syntaxTree.ToString());
      }
    }

    public static void WriteThreeAddressCodeToFile(string fileName, List<ThreeAdressCode> threeAddressCode)
    {
      var lines = threeAddressCode.Select(instruction => instruction.ToString()).ToList();
      File.WriteAllLines(fileName, lines);
    }

    public static void WritePostfixFormToFile(List<Token> postfixForm, string fileName)
    {
      string line = string.Join("", postfixForm.Select(token => token.ToString().Split(" - ")[0]));

      File.WriteAllText(fileName, line);
    }

    public static bool IsValidFile(string parFile)
    {
      if (!parFile.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
      {
        throw new ArgumentException("Неверное расширение файла");
      }
      char[] invalidChars = Path.GetInvalidFileNameChars();
      foreach (char c in invalidChars)
      {
        if (parFile.Contains(c))
        {
          throw new ArgumentException("В названии файла использованы недопустимые символы");
        }
      }
      return true;
    }

    public static void CreateBinFile(SymbolsTable symbolsTable, List<ThreeAdressCode> threeAdressCodes, string fileName)
    {
      using (FileStream fs = new FileStream(fileName, FileMode.Create))
      using (BinaryWriter writer = new BinaryWriter(fs))
      {
        symbolsTable.Serialize(writer);

        writer.Write(threeAdressCodes.Count);
        foreach (var threeAdressCode in threeAdressCodes)
        {
          threeAdressCode.Serialize(writer);
        }
      }
    }
  }
}
