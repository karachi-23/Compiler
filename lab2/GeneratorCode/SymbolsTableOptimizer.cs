using lab2.LexicalAnalisator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2.GeneratorCode
{
  internal class SymbolsTableOptimizer
  {
    public SymbolsTableOptimizer()
    {
    }

    public SymbolsTable Optimize(SymbolsTable symbolsDic, List<ThreeAdressCode> threeAdressCodes)
    {
      foreach (var symbol in symbolsDic.GetSymbols())
      {
        if (!symbol.Value.Name.StartsWith("#T"))
        {
          bool used = false;
          foreach (var threeAdressCode in threeAdressCodes)
          {
            if (threeAdressCode.Result.Value == symbol.Value.Name)
            {
              used = true;
            }

            foreach (var operand in threeAdressCode.Tokens)
            {
              if (operand.Type == LexicalEnumType.Identifier)
              {
                if (operand.Value == symbol.Value.Name)
                {
                  used = true;
                }
              }

            }
          }
          if (!used)
          {
            symbolsDic.RemoveSymbolByName(symbol.Value.Name);
          }
        }


      }
      return symbolsDic;
    }
  }
}
