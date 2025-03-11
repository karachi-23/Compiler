using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2.LexicalAnalisator
{
  public static class LexicalEnumTypeExtension
  {
    public static string ToDetailedString(this LexicalEnumType type)
    {
      return type switch
      {
        LexicalEnumType.IntegerConstant => "константа целочисленного типа",
        LexicalEnumType.FloatConstant => "константа вещественного типа",
        LexicalEnumType.Identifier => "идентификатор с именем",
        LexicalEnumType.OpeningBracket => "открывающая скобка",
        LexicalEnumType.ClosingBracket => "закрывающая скобка",
        LexicalEnumType.AdditionOperation => "операция сложения",
        LexicalEnumType.SubtractionOperation => "операция вычитания",
        LexicalEnumType.MultiplicationOperation => "операция умножения",
        LexicalEnumType.DivisionOperation => "операция деления",
        LexicalEnumType.IntegerVariable => "целого типа",
        LexicalEnumType.FloatVariable => "вещественного типа",
        LexicalEnumType.Coercion => "преобразование типа",
        LexicalEnumType.Unknown => "неизвестного типа"
      };
    }

    public static string ToDescribString(this LexicalEnumType type)
    {
      return type switch
      {
        LexicalEnumType.IntegerConstant => "Int",
        LexicalEnumType.FloatConstant => "Float",
        LexicalEnumType.Identifier => "id",
        LexicalEnumType.OpeningBracket => "(",
        LexicalEnumType.ClosingBracket => ")",
        LexicalEnumType.AdditionOperation => "+",
        LexicalEnumType.SubtractionOperation => "-",
        LexicalEnumType.MultiplicationOperation => "*",
        LexicalEnumType.DivisionOperation => "/",
        LexicalEnumType.IntegerVariable => "целый",
        LexicalEnumType.FloatVariable => "вещественный",
        LexicalEnumType.Coercion => "Int2Float",
        LexicalEnumType.Unknown => "неизвестного типа"
      };
    }
  }
}
