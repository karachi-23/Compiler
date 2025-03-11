using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lab2.LexicalAnalisator;

namespace lab2.SyntaxAnalisator
{
  internal class SyntaxNode
  {
    public Token Token { get; set; }
    public List<SyntaxNode> Children { get; set; }
    public LexicalEnumType Type { get; set; }

    public SyntaxNode(Token token)
    {
      Token = token;
      Children = new List<SyntaxNode>();
    }

    public override string ToString()
    {
      return $"|--<{Token.Value}>";
    }
  }
}
