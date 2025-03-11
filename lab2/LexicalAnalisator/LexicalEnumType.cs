using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2.LexicalAnalisator
{
    public enum LexicalEnumType
    {
        IntegerConstant,
        FloatConstant,
        Identifier,
        OpeningBracket,
        ClosingBracket,
        AdditionOperation,
        SubtractionOperation,
        MultiplicationOperation,
        DivisionOperation,

        IntegerVariable,
        FloatVariable,

        Coercion,

        Unknown,
    }
}
