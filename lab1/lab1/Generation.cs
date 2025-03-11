using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab.lab1
{
    class Generation
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

        private static string GenerateOperand()
        {
            Random rand = new Random();
            return _genNumber.Keys.ElementAt(rand.Next(0, _genNumber.Count));
        }

        private static string GenerateOperator()
        {
            Random rand = new Random();
            return _genSymbol.Keys.ElementAt(rand.Next(0, _genSymbol.Count));
        }

        private static string GenerateString(int parMinNumberOperand, int parMaxNumberOperand)
        {
            Random rand = new Random();
            int numberOperand = rand.Next(parMinNumberOperand, parMaxNumberOperand);
            StringBuilder sb = new StringBuilder();
            sb.Append(GenerateOperand());
            for (int i = 0; i < numberOperand; i++)
            {
                if (i <= numberOperand - 1)
                {
                    sb.Append(" ");
                    sb.Append(GenerateOperator());
                    sb.Append(" ");
                    sb.Append(GenerateOperand());
                }
                else
                {
                    sb.Append(GenerateOperand());
                }
            }
            return sb.ToString();
        }

        public static string Generate(int parNumberString, int parMinNumberOperand, int parMaxNumberOperand)
        {
            StringBuilder line = new StringBuilder();
            for (int i = 0; i < parNumberString; i++)
            {
                line.Append(GenerateString(parMinNumberOperand, parMaxNumberOperand));
                line.Append('\n');
            }
            return line.ToString();
        }
    }
}
