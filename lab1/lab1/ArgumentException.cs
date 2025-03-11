using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab.lab1
{
    class ArgumentException : Exception
    {
        public ArgumentException(string message) : base(message) { }
    }
}
