using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2.SyntaxAnalisator
{
  public class SyntaxException : Exception
  {
    public SyntaxException(string message) : base(message) { }
  }
}
