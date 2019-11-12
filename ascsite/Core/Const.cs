using System;
using System.Runtime.Serialization;

namespace ascsite.Core
{
    public static class Const
    {
        public static readonly string ERMSG_BOOLENGAMEXC                 = "";
        public static readonly string ERMSG_PREINVALIDTOKEN              = "";
        public static readonly string ERMSG_POSTINVALIDTOKEN             = "";
        public static readonly string ERMSG_PYINTERFACE_RESP             = "";
        public static readonly string ERMSG_SEQURITY                     = "";
        public static readonly string ERMSG_EXECUTE_TIMEOUT              = "Execution timeout";
        public static readonly string ERMSG_MSL_STARTERR                 = "";
        public static readonly string ERMSG_PREOUTPUT                    = "";
        public static readonly string ERMSG_EMPTYREQ                     = "";
        public static readonly string ERMSG_UNRESOLVED_SYMBOL            = "";
        public static readonly string ERMSG_UNABLE_TO_SOLVE              = "";
        public static readonly string ERMSG_BOOLENG_UNRESOLVEDSYMBOL     = "";

        public static readonly string PATH_ASSOCPRJS                     = "Pages/projects/assoc";
        public static readonly string PATH_PYOUT                         = "PyOut";
        public static readonly string PATH_SUBSAMPLES                    = "Pages/msl/samples";
        public static readonly string PATH_MSL                           = "Resources/MSL.exe";

        public static readonly int    LIMIT_BOOLENGVARS                  = 6;
        public static readonly int    LIMIT_MSL_EXECUTE_MS               = 10000;
        public static readonly int    LIMIT_PYMAXRESPONSESIZE            = 65536;
        public static readonly int    LIMIT_CALC_EXECUTE_MS              = 10000;
        public static readonly int    LIMIT_REQLEN                       = 400;

        public static readonly string DEL_ANNSTART                       = "@annstart";
        public static readonly string DEL_ANNEND                         = "@annend";
        public static readonly char   DEL_PRJSPAGES                      = '|';

        public static readonly string TITLE_ANALYTICAL_SIMPLIFY          = "Result";
        public static readonly string TITLE_APPROXIMATE_SIMPLIFY         = "Interpreted As";

        public static readonly string PYINTERFACE_ADDRESS                = "127.0.0.1";
        public static readonly int    PYINTERFACE_PORT                   = 7000;
        public static readonly char   PYINTERFACE_ERROR                  = 'E';
        public static readonly char   PYINTERFACE_EVALREQ                = 'E';

        public static readonly string PREFIX_MSL_CALC_EXECUTE            = "ASC_EXECUTE_COMMAND";
        public static readonly string PREFIX_MSL_END_FILEREAD            = "MSL_END_FILEREAD";

        public static readonly string ADD_LATEXSCRIPT                    = "<script id=\"MathJax-script\" async src=\"https://cdn.jsdelivr.net/npm/mathjax@3/es5/tex-mml-chtml.js\">";
    }
}