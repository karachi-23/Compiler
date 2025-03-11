using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab.lab1
{
    class WorkWithFile
    {
        public static string ReadFile(string parFile)
        {
            try
            {
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
    }
}
