using lab.lab2;
using lab2.LexicalAnalisator;
using lab2.SemanticAnalisator;
using lab2.SyntaxAnalisator;
using lab2.GeneratorCode;
using System.Runtime.CompilerServices;
using System.Text;

namespace lab2
{
    internal class Runner
  {
    static void Main(string[] args)
    {
      //string lines = WorkWithFile.ReadFile(args[0]);
      //var (tokens, symbols) = Analisator.Analis(lines);
      //Console.WriteLine(symbols.ToString());
      /*foreach (Token token in tokens)
      {
        Console.WriteLine(token.ToString());
      }*/
      if (!WorkWithFile.IsValidFile(args[1]))
      {
        throw new FileException("Неверное название входного файла");
        
      }else if (!WorkWithFile.IsValidFile(args[2]))
      {
        throw new FileException("Неверное название выходного файла токенов");
      } 
      else if (!WorkWithFile.IsValidFile(args[3]))
      {
        throw new FileException("Неверное название выходного файла символов");
      }
      else
      {
        try
        {
          string operation = args[0].ToUpper();
          string lines = WorkWithFile.ReadFile(args[1]);
          var (tokens, symbols) = Analisator.Analis(lines);
          if (operation.Equals("LEX"))
          {

            StringBuilder result = new StringBuilder();

            foreach (Token token in tokens)
            {
              result.Append(token.ToString()).Append("\n");
            }
            WorkWithFile.WriteFile(args[2], result.ToString());

            result.Clear();

            result.AppendLine(symbols.ToString());
            WorkWithFile.WriteFile(args[3], result.ToString());
          }
          else if (operation.Equals("SYN"))
          {
            try
            {
              var parser = new SyntaxParser(tokens);
              var syntaxTree = parser.Parse();
              string treeVisualization = syntaxTree.Visualize();
              WorkWithFile.WriteFile("syntax_tree.txt", treeVisualization);
            }
            catch (SyntaxException e)
            {
              Console.WriteLine(e.Message);
            }
          }
          else if (operation.Equals("SEM"))
          {
            try
            {
              var parser = new SyntaxParser(tokens);
              var syntaxTree = parser.Parse();

              SemanticAnalyzer semanticAnalisator = new SemanticAnalyzer(syntaxTree, Analisator.GetSymbolTable());
              WorkWithFile.WriteFile("syntax_tree_mod.txt", syntaxTree.Visualize());
            }
            catch (SyntaxException e)
            {
              Console.WriteLine(e.Message);
            }
          }
          else if (operation.Equals("GEN1"))
          {
            try
            {
              SyntaxParser parser = new SyntaxParser(tokens);
              SyntaxTree syntaxTree = parser.Parse();

              SymbolsTable symbolsTable = Analisator.GetSymbolTable();


              SemanticAnalyzer semanticAnalisator = new SemanticAnalyzer(syntaxTree, symbolsTable);
              WorkWithFile.WriteFile("syntax_tree_mod.txt", syntaxTree.Visualize());

              if (args.Length == 5 && args[4].ToUpper() == "OPT")
              {
                SyntaxTree optimizeTree = SyntaxTreeOptimazer.OptimizeTree(syntaxTree);
                WorkWithFile.WriteFile("OPTsyntax_tree_mod.txt", optimizeTree.Visualize());

                Generator generator = new Generator(optimizeTree, symbolsTable);
                List<ThreeAdressCode> threeAdressCodeOptimize = generator.GenerateThreeAdressCode(true);

                SymbolsTableOptimizer symbolsTableOptimizer = new SymbolsTableOptimizer();
                symbolsTable = symbolsTableOptimizer.Optimize(symbolsTable, threeAdressCodeOptimize);
               // WorkWithFile.WriteFile("symbol.txt", symbolsTable);
                WorkWithFile.WriteThreeAddressCodeToFile("OPTportable_code.txt", threeAdressCodeOptimize);
                WorkWithFile.WriteFile("OPTsymbol.txt", symbolsTable.ToString());
              }
              else 
              { 
              Generator generator = new Generator(syntaxTree, symbolsTable); 
              List<ThreeAdressCode> threeAdressCode = generator.GenerateThreeAdressCode(false);
              WorkWithFile.WriteThreeAddressCodeToFile("portable_code.txt", threeAdressCode);
              WorkWithFile.WriteFile("symbol.txt", symbolsTable.ToString());
              }
            }
            catch (SyntaxException e)
            {
              Console.WriteLine(e.Message);
            }

          }
          else if (operation.Equals("GEN2"))
          {
            SyntaxParser parser = new SyntaxParser(tokens);
            SyntaxTree syntaxTree = parser.Parse();

            SymbolsTable symbolsTable = Analisator.GetSymbolTable();

            SemanticAnalyzer semanticAnalisator = new SemanticAnalyzer(syntaxTree, symbolsTable);

            string postfixFile = "postfix.txt";
            if (args.Length == 5 && args[4].ToUpper() == "OPT")
            {
              syntaxTree = SyntaxTreeOptimazer.OptimizeTree(syntaxTree);
              postfixFile = "OPT" + postfixFile;
            }
            Generator generator = new Generator(syntaxTree, symbolsTable);

            List<Token> postfixForm = generator.GeneratorPostfixForm();

            List<ThreeAdressCode> threeAdressCodeOptimize = generator.GenerateThreeAdressCode(true);

            SymbolsTableOptimizer symbolsTableOptimizer = new SymbolsTableOptimizer();
            symbolsTable = symbolsTableOptimizer.Optimize(symbolsTable, threeAdressCodeOptimize);

            WorkWithFile.WritePostfixFormToFile(postfixForm, postfixFile);
            WorkWithFile.WriteFile("symbols.txt", symbolsTable.ToString());

          }
          else if (operation.Equals("GEN3"))
          {
            SyntaxParser parser = new SyntaxParser(tokens);
            SyntaxTree syntaxTree = parser.Parse();

            SymbolsTable symbolsTable = Analisator.GetSymbolTable();

            SemanticAnalyzer semanticAnalisator = new SemanticAnalyzer(syntaxTree, symbolsTable);
            
            syntaxTree = SyntaxTreeOptimazer.OptimizeTree(syntaxTree);

            SyntaxTree optimizeTree = SyntaxTreeOptimazer.OptimizeTree(syntaxTree);

            Generator generator = new Generator(optimizeTree, symbolsTable);

            List<ThreeAdressCode> threeAdressCodeOptimize = generator.GenerateThreeAdressCode(true);

            SymbolsTableOptimizer symbolsTableOptimizer = new SymbolsTableOptimizer();
            symbolsTable = symbolsTableOptimizer.Optimize(symbolsTable, threeAdressCodeOptimize);

            string binFileName = "post_code.bin";
            WorkWithFile.CreateBinFile(symbolsTable, threeAdressCodeOptimize, binFileName);
          }
        }
        catch (Exception e) { Console.WriteLine(e.Message); }




      }
      
    }
  }
}
