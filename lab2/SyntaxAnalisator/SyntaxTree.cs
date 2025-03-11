using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2.SyntaxAnalisator
{
  internal class SyntaxTree
  {
    public SyntaxNode Root { get; private set; }

    public SyntaxTree(SyntaxNode root)
    {
      Root = root;
    }

    public string Visualize()
    {
      return VisualizeNode(Root, 0);
    }

    private string VisualizeNode(SyntaxNode node, int indent)
    {
      if (node == null) return string.Empty;

      var result = new string(' ', indent * 4) + node.ToString() + "\n";
      foreach (var child in node.Children)
      {
        result += VisualizeNode(child, indent + 1);
      }

      return result;
    }
  }
}
