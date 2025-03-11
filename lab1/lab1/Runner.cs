using System.CodeDom.Compiler;

namespace lab.lab1
{
    internal class Runner
    {
        static void Main(string[] args)
        {
            try
            {
                if (args[0].Equals("G") || args[0].Equals("g"))
                {
                    if (args.Length == 0)
                    {
                        throw new ArgumentException("Список аргументов не может быть пустым");
                    }
                    else if (args.Length < 5)
                    {
                        throw new ArgumentException("Количество аргументов меньше 5");
                    }
                    else if (args.Length > 5)
                    {
                        throw new ArgumentException("Количество аргументов больше 5");
                    }
                    else if (!(int.TryParse(args[2], out _) && int.TryParse(args[3], out _) && int.TryParse(args[4], out _)))
                    {
                        throw new ArgumentException("3, 4 и 5 аргумент должны быть целыми, положительными числами");
                    }
                    else if (int.TryParse(args[1], out _))
                    {
                        throw new ArgumentException("2 аргумент должны быть строкой");
                    }
                    else if (!WorkWithFile.IsValidFile(args[1]))
                    {
                        throw new ArgumentException("Неверное название файла");
                    }
                    else if (int.Parse(args[2]) <= 0 || int.Parse(args[3]) <= 0 || int.Parse(args[4]) <= 0)
                    {
                        throw new ArgumentException("3, 4 и 5 аргумент должны быть больше 0");
                    }
                    else if (int.Parse(args[3]) >= int.Parse(args[4]))
                    {
                        throw new ArgumentException("Минимальное количество операндов должно быть меньше, чем максимальное количество операндов");
                    }
                    else
                    {
                        string lines = Generation.Generate(int.Parse(args[2]), int.Parse(args[3]), int.Parse(args[4]));
                        WorkWithFile.WriteFile(args[1], lines);
                        //Generation.GenerationFile(args[1], Int32.Parse(args[2]), Int32.Parse(args[3]), Int32.Parse(args[4]));
                    }
                }
                else if (args[0].Equals("T") || args[0].Equals("t"))
                {
                    if (args.Length == 0)
                    {
                        throw new ArgumentException("Список аргументов не может быть пустым");
                    }
                    else if (args.Length < 3)
                    {
                        throw new ArgumentException("Количество аргументов меньше 3");
                    }
                    else if (args.Length > 3)
                    {
                        throw new ArgumentException("Количество аргументов больше 3");
                    }
                    else if (int.TryParse(args[1], out _) || int.TryParse(args[2], out _))
                    {
                        throw new ArgumentException("2 и 3 аргумент должны быть строкой");
                    }
                    else if (!WorkWithFile.IsValidFile(args[1]))
                    {
                        throw new ArgumentException("Неверное название файла из которого необходимо считать информацию");
                    }
                    else if (!WorkWithFile.IsValidFile(args[2]))
                    {
                        throw new ArgumentException("Неверное название файла в который необходимо записать результат");
                    }
                    else
                    {
                        string lines = WorkWithFile.ReadFile(args[1]);
                        string linesTranslate = Translater.Translate(lines);
                        WorkWithFile.WriteFile(args[2], linesTranslate);
                        //lines =
                        //linesStr = Translater.TranslateFile(lines);// args[1], args[2]);
                        //wrie
                    }
                }
                else
                {
                    throw new ArgumentException("1 симовол должен быть либо G(g), либо T(t)");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
