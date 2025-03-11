using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace lab2.LexicalAnalisator
{
    internal class Analisator
    {
        private static int position = 0;
        static List<string> _genSymbol = new List<string> { "(", "+", "-", "*", "/", ")" };
        static SymbolsTable symbolsTable = new SymbolsTable();

        static List<Token> token = new List<Token>();


        static int id = 1;

        public static (List<Token> tokens, SymbolsTable symbolsTable) Analis(string parString)
        {
            while (position < parString.Length)
            {
                char res = parString[position];
                if (' '.Equals(parString[position]))
                {
                    position++;
                }
                else if (char.IsDigit(parString[position]))
                {
                    CheckDigit(parString);
                }
                else if (_genSymbol.Contains(parString[position].ToString()))
                {
                    CheckSymbol(parString);
                }
                else if ('_'.Equals(parString[position]) ||
                  char.IsLetter(parString[position]) && (parString[position] >= 'A'
                  && parString[position] <= 'Z' || parString[position] >= 'a' && parString[position] <= 'z'))
                {
                    CheckIdentificator(parString);
                }
                else
                {
                    CheckOther(parString);
                }
            }
            return (token, symbolsTable);
        }

        private static void CheckDigit(string parString)
        {
            StringBuilder res = new StringBuilder();
            int startPosition = position;
            int endPosition = position;
            int hasDecimalPoint = 0;
            int positionDoublePoint = 0;
            while (endPosition + 1 < parString.Length && (char.IsDigit(parString[endPosition + 1])
              || '.'.Equals(parString[endPosition + 1])))
            {
                if (parString[endPosition] == '.')
                {
                    hasDecimalPoint++;
                    positionDoublePoint = endPosition + 1;
                }
                endPosition++;
            }

            if (endPosition + 1 < parString.Length && char.IsLetter(parString[endPosition + 1]))
            {
                while (endPosition + 1 < parString.Length && char.IsLetter(parString[endPosition + 1])
                  && (parString[endPosition + 1] >= 'A' && parString[endPosition + 1] <= 'Z' || parString[endPosition + 1]
                  >= 'a' && parString[position + 1] <= 'z'))
                {
                    endPosition++;
                }

                res.Append(parString.Substring(startPosition, endPosition - startPosition + 1));
                Console.WriteLine($"Лексическая ошибка! Идентификатор  «{res}» не может начинаться" +
                  $" с цифры на позиции {startPosition + 1}");

                position = endPosition + 1;
            }
            else if (hasDecimalPoint > 1)
            {
                res.Append(parString.Substring(startPosition, endPosition - startPosition + 1));
                Console.WriteLine($"Лексическая ошибка! Неправильно задана константа «{res}» на позиции {positionDoublePoint}");
                position = endPosition + 1;
            }
            else
            {
                res.Append(parString.Substring(startPosition, endPosition - startPosition + 1));
                if (int.TryParse(res.ToString(), out _))
                {
                    token.Add(new Token(LexicalEnumType.IntegerConstant, res.ToString(), -1));
                }
                else
                {
                    token.Add(new Token(LexicalEnumType.FloatConstant, res.ToString(), -1));
                }
                position = endPosition + 1;
            }
        }

        private static void CheckSymbol(string parString)
        {
            if ('('.Equals(parString[position]))
            {
                token.Add(new Token(LexicalEnumType.OpeningBracket, parString[position].ToString()));
            }
            else if ('+'.Equals(parString[position]))
            {
                token.Add(new Token(LexicalEnumType.AdditionOperation, parString[position].ToString()));
            }
            else if ('-'.Equals(parString[position]))
            {
                token.Add(new Token(LexicalEnumType.SubtractionOperation, parString[position].ToString()));
            }
            else if ('*'.Equals(parString[position]))
            {
                token.Add(new Token(LexicalEnumType.MultiplicationOperation, parString[position].ToString()));
            }
            else if ('/'.Equals(parString[position]))
            {
                token.Add(new Token(LexicalEnumType.DivisionOperation, parString[position].ToString()));
            }
            else if (')'.Equals(parString[position]))
            {
                token.Add(new Token(LexicalEnumType.ClosingBracket, parString[position].ToString()));
            }

            position++;
            //token.Add($"<{parString[position]}> - {_genSymbol[parString[position].ToString()]}");
            //position++;
        }

    private static void CheckIdentificator(string parString)
    {
      StringBuilder res = new StringBuilder();
      int startPosition = position;
      int endPosition = position;
      while (endPosition + 1 < parString.Length && (char.IsDigit(parString[endPosition + 1]) ||
        char.IsLetter(parString[endPosition + 1]) &&
        (parString[endPosition + 1] >= 'A' && parString[endPosition + 1] <= 'Z'
        || parString[endPosition + 1] >= 'a' && parString[endPosition + 1] <= 'z') || '_'.Equals(parString[endPosition + 1])))
      {
        endPosition++;
      }
      res.Append(parString.Substring(startPosition, endPosition - startPosition + 1));
      position = endPosition + 1;
      //int idTmp = symbolsTable.addSymbols(res.ToString());
      if (position < parString.Length && parString[position] == '[')
      {
        // Проверяем наличие 'f' после '['
        int nextPosition = position + 1;
        if (nextPosition < parString.Length && parString[nextPosition] == 'f')
        {
          int idTmp = symbolsTable.addSymbols(res.ToString(), LexicalEnumType.FloatVariable);
          token.Add(new Token(LexicalEnumType.Identifier, res.ToString(), idTmp));
          position = nextPosition + 1;  // Переходим к позиции после ']'
        }
        else if (nextPosition < parString.Length && parString[nextPosition] == 'i')
        {
          int idTmp = symbolsTable.addSymbols(res.ToString(), LexicalEnumType.IntegerVariable);
          token.Add(new Token(LexicalEnumType.Identifier, res.ToString(), idTmp));
          position = nextPosition + 1;  // Переходим к позиции после ']'
        }
        else
        {
          throw new Exception($"Неизвестный тип данных {parString[position+1]} на позиции{position + 1}");
        }
        if (position < parString.Length && parString[position] == ']')
        {
          position++;
        }
        else
        {
          throw new Exception($"Лексическая ошибка. Отсутствует закрывающая скобка для идетификатора {res} на позиции {position + 1}");
        }

      }
      else
      {
        int idTmp = symbolsTable.addSymbols(res.ToString(), LexicalEnumType.IntegerVariable);
        token.Add(new Token(LexicalEnumType.Identifier, res.ToString(), idTmp));
      }
    }

        private static void CheckOther(string parString)
        {
            StringBuilder res = new StringBuilder();
            int startPosition = position;
            int endPosition = position;
            while (endPosition + 1 < parString.Length && (char.IsDigit(parString[endPosition + 1]) || char.IsLetter(parString[endPosition + 1]) && (parString[endPosition + 1] >= 'A'
              && parString[endPosition + 1] <= 'Z' || parString[endPosition + 1] >= 'a'
              && parString[endPosition + 1] <= 'z') || '.'.Equals(parString[endPosition + 1])))
            {
                endPosition++;
            }
            res.Append(parString.Substring(startPosition, endPosition - startPosition + 1));
            if (startPosition == endPosition)
            {
                Console.WriteLine($"Лексическая ошибка! Недопустимый символ {parString[position]}" +
                    $" на позиции {position + 1}");
            }
            else if (IsNumber(res.ToString().Substring(1)))
            {
                Console.WriteLine($"Лексическая ошибка! Неправильно задана константа  «{res}» " +
                  $"на позиции {startPosition + 1}");
            }
            else if (char.IsDigit(res.ToString()[0]))
            {
                Console.WriteLine($"Лексическая ошибка! Идентификатор  «{res}» не может начинаться" +
                 $" с цифры на позиции {startPosition + 1}");
            }
            else
            {
                Console.WriteLine($"Лексическая ошибка! Недопустимый символ «{res}»" +
                 $" на позиции {startPosition + 1}");
            }


            position = endPosition + 1;
        }

        private static bool IsNumber(string str)
        {
            if (int.TryParse(str, out _))
            {
                return true;
            }
            if (double.TryParse(str, NumberStyles.Float, CultureInfo.InvariantCulture, out _))
            {
                return true;
            }
            return false;
        }

        public static SymbolsTable GetSymbolTable()
        {
          return symbolsTable;
        }
    }
}
