using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab.lab1
{
    class FileException : Exception
    {
        public FileException(string message) : base(message) { }
    }
}
