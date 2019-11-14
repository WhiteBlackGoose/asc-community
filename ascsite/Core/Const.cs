using System;
using System.Runtime.Serialization;

namespace ascsite.Core
{
    public static class Const
    {
        /*  ERROR MESSAGES  */
        public static readonly string ERMSG_PREOUTPUT = "During parsing an error occurred: ";
        public static readonly string ERMSG_PREINVALIDTOKEN = "invalid token `";
        public static readonly string ERMSG_POSTINVALIDTOKEN = "`";
        public static readonly string ERMSG_EMPTYREQ = "Received empty request.";
        public static readonly string ERMSG_SEQURITY = "Your request was ignored due to security reasons.";
        public static readonly string ERMSG_UNRESOLVED_SYMBOL = "unresolved symbol: ";
        public static readonly string ERMSG_MSL_STARTERR = "Server cannot process MSL program: ";
        public static readonly string ERMSG_UNABLE_TO_SOLVE = "Unable to respond to the request";
        public static readonly string ERMSG_EXECUTE_TIMEOUT = "Execution cancelled because of timeout";
        public static readonly string ERMSG_EXECUTE_CPUMEMOUT = "CPU or memory out";
        public static readonly string ERMSG_PYINTERFACE_RESP = "Response with an error from py interface";
        public static readonly string ERMSG_BOOLENGAMEXC = "Amount of variables exceeded";
        public static readonly string ERMSG_BOOLENG_UNRESOLVEDSYMBOL = "Invalid syntax";
        public static readonly string ERMSG_INVALID_SYNTAX = "Invalid syntax";
        public static readonly string ERMSG_INVALID_SYMBOL = "Invalid symbol";
        public static readonly string ERMSG_INTERNAL_ERROR = "Internal error";

        /*  PATHS  */
        public static readonly string PATH_PYTHON = @"D:\main\programs\miniconda3\python.exe";
        public static readonly string PATH_MSL = @"Resources\MSL.exe";
        public static readonly string PATH_SUBSAMPLES = @"Pages\msl\source";
        public static readonly string PATH_ASSOCPRJS = @"Pages\projects\assoc";
        public static readonly string PATH_LAYOUT = @"Pages\Shared\_Layout.cshtml";
        public static readonly string PATH_PYOUT = @"Core/PyInterface/pyout"; /// BE CAREFUL WITH THIS PATH! IT IS CALLED IN PYTHON

        /*  TITLES  */
        public static readonly string TITLE_INTERPRETEDAS = "Interpreted as";
        public static readonly string TITLE_COMPUTATIONAL_RESULT = "Result";
        public static readonly string TITLE_ANALYTICAL_SIMPLIFY = "Analytical representation: ";
        public static readonly string TITLE_APPROXIMATE_SIMPLIFY = "Approximate representation: ";

        /*  PREFIX  */
        public static readonly string PREFIX_MSL_CALC_EXECUTE = "ASC_EXECUTE_COMMAND";
        public static readonly string PREFIX_MSL_INPUT_WRITE = "ASC_INPUT_WRITE";
        public static readonly string PREFIX_MSL_END_FILEREAD = "MSL_END_FILEREAD";

        /*  OPERATORS  */
        public static readonly string MATHOP_DOT = ".";
        public static readonly string MATHOP_OPLIST = "+-/*%^<>=,&!;";

        /*  LIMITS  */
        public static readonly int LIMIT_REQLEN = 400;
        public static readonly int LIMIT_MSL_EXECUTE_MS = 10000;
        public static readonly int LIMIT_CALC_EXECUTE_MS = 12000;
        public static readonly int LIMIT_FILE_CREATED = 100;
        public static readonly int LIMIT_BATCH_TIME_AWAIT = 10;
        public static readonly int LIMIT_PYMAXRESPONSESIZE = 262144;
        public static readonly int LIMIT_BOOLENGVARS = 6;

        /*  PYINTERFACE  */
        public static readonly int PYINTERFACE_PORT = 7000;
        public static readonly string PYINTERFACE_ADDRESS = "127.0.0.1";
        public static readonly char PYINTERFACE_ERROR = 'E';
        public static readonly char PYINTERFACE_EVALREQ = 'E';

        /*  DELIMITERS  */
        public static readonly string DEL_PRJSPAGES = "|";
        public static readonly string DEL_ANNSTART = "@annstart";
        public static readonly string DEL_ANNEND = "@annend";

        /*  ADDDATA  */
        public static readonly string ADD_LATEXSCRIPT = "<script id=\"MathJax-script\" async src=\"https://cdn.jsdelivr.net/npm/mathjax@3/es5/tex-mml-chtml.js\"></script>" + "<script src=\"https://polyfill.io/v3/polyfill.min.js?features=es6\"></script>";

        public static char DEL_RANGE => throw new Exception("change DEL_RANGE to char");
        public static char DEL_LIST => throw new Exception("change DEL_LIST to char");
    }
}