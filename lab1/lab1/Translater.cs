using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab.lab1
{
    public class Translater
    {
        static Dictionary<string, string> _genNumber = new Dictionary<string, string>
    {
      {"1","один"},
      {"2", "два" },
      {"3", "три" },
      {"4", "четыре" },
      {"5", "пять" },
      {"6", "шесть" },
      {"7", "семь" },
      {"8", "восемь" },
      {"9", "девять" },
    };
        static Dictionary<string, string> _genSymbol = new Dictionary<string, string>
    {
      {"+", "плюс"},
      {"-", "минус"},
      {"*", "умножить на"},
      {":", "разделить на" },
    };

        private static bool CheckString(string parString)
        {
            for (int i = 0; i < parString.Length; i++)
            {
                if (!(_genNumber.ContainsKey(parString[i].ToString()) || _genSymbol.ContainsKey(parString[i].ToString()) || parString[i].Equals(' ') || parString[i].Equals('\n')))
                {
                    throw new FileException("В файле содержатся недопустимые данные");
                }
            }
            return true;
        }


        public static string Translate(string parString)
        {
            //      string readText = WorkWithFile.ReadFile(parOutFile);
            StringBuilder result = new StringBuilder();
            if (CheckString(parString))
            {
                for (int i = 0; i < parString.Length; i++)
                {
                    if (_genNumber.ContainsKey(parString[i].ToString()))
                    {
                        result.Append(_genNumber[parString[i].ToString()]);
                    }
                    else if (_genSymbol.ContainsKey(parString[i].ToString()))
                    {
                        result.Append(_genSymbol[parString[i].ToString()]);
                    }
                    else if (parString[i].Equals(' '))
                    {
                        result.Append(" ");
                    }
                    else
                    {
                        result.Append("\n");
                    }
                }
            }
            return result.ToString();
        }

        /*public static void TranslateFile(string parInputFile, string parOutputFile)
        {
          string result = Translate(parInputFile);
          WorkWithFile.WriteFile(parOutputFile, result);
        }*/
    }
}
