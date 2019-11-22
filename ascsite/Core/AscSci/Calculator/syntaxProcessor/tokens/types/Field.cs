using System;
using System.Collections.Generic;
using System.Text;

namespace Processor.syntaxProcessor.tokens.types
{
    public static class Field
    {
        public enum Type
        {
            DERIVATIVE,
            INTEGRAL, 
            SOLVE,
            BOOLEAN,
            PLOT,
            SIMPLIFICATION,
            NONE
        }
    }
}
