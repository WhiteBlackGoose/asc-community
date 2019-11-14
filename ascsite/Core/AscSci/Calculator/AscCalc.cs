using ascsite.Core.PyInterface.PyMath;
using AscSite.Core.AscSci.AscMath;
using processor;
using processor.lexicProcessor;
using processor.syntaxProcessor;
using processor.syntaxProcessor.tokens;
using processor.syntaxProcessor.tokens.types;
using System;
using System.Collections.Generic;
using System.Linq;


namespace ascsite.Core.AscSci.Calculator
{
    public class CalcResult
    {
        public List<string> InterpretedAs { get; set; }
        public string Result { get; set; }
        public string SolidResult { get { return Result.Replace("\n", ", ").Replace('I', 'i'); } }

        public string ProcessedResult { get
            {
                return Result.Replace("\n", "<br>");
            } }
        public CalcResult()
        {
            InterpretedAs = new List<string>();
        }
    }

    public class AscCalc
    {
        string rawExpression;
        public enum FieldType
        {
            DERIVATIVE,
            BOOL,
            INTEGRAL,
            SOLVE,
            SIMPLIFICATION
        }

        private static List<FieldType> MathAnalysis = new List<FieldType>()
        {
            FieldType.DERIVATIVE,
            FieldType.INTEGRAL,
            FieldType.SOLVE,
        };

        private bool latex;

        public AscCalc(string expr, bool latex=false)
        {
            rawExpression = expr.Trim();
            this.latex = latex;
        }

        public List<MajorSegmentList> DivideByField(MajorSegmentList list)
        {
            var res = new List<MajorSegmentList>();
            foreach(var elem in list)
            {
                if (elem is FieldSegment)
                {
                    res.Add(new MajorSegmentList());
                    res[res.Count - 1].Add(elem);
                }
                else if (elem is KeywordSegment)
                {
                    if (res.Count > 0)
                        res[res.Count - 1].Add(elem);
                }
                else
                    break;
            }
            return res;
        }

        public CalcResult Compute()
        {
            CalcResult res = new CalcResult();

            PyMath pymath = new PyMath();
            try
            {
                // INITIALIZATION OF STUFF
                Lexer lexer = new Lexer(rawExpression);
                TokenList tokenList = lexer.GenerateTokens();
                Parser parser = new Parser(tokenList);
                PostTokenList postTokenList = parser.Parse();
                MathPreprocessor mathPreprocessor = new MathPreprocessor(postTokenList);
                mathPreprocessor.Process();
                MajorSegmentList majorSegmentList = mathPreprocessor.GetSegments();
                ExpressionSegment expressionSegment = (ExpressionSegment)majorSegmentList.Select("expression").Item();

                // PROCESSING SOME `WHERE` AND `FOR` TOKENS
                for (int i = majorSegmentList.Count - 1; i >= 0; i--)
                {
                    if (majorSegmentList[i] is ExpressionSegment)
                        break;
                    if (majorSegmentList[i] is EqKeyword 
                          && 
                          (((EqKeyword)majorSegmentList[i]).Type == Keyword.Type.WHERE || 
                          ((EqKeyword)majorSegmentList[i]).Type == Keyword.Type.FOR)
                          &&
                        FixedKeyword.IsVariable(((EqKeyword)majorSegmentList[i]).GetBeforeEq())
                          &&
                        (CustomData.GetType(((EqKeyword)majorSegmentList[i]).GetAfterEq()) == CustomData.Type.FIXED))
                    {
                        string v = ((EqKeyword)majorSegmentList[i]).GetBeforeEq();
                        string expr = ((EqKeyword)majorSegmentList[i]).GetAfterEq();
                        expressionSegment.Substitute(v, expr);
                    }
                    else
                        majorSegmentList[i].Ignored = true;
                }

                MajorSegmentList newExpression = majorSegmentList.CutTill(expressionSegment);
                res.InterpretedAs.Add(expressionSegment.Build());
                List<MajorSegmentList> list = DivideByField(newExpression);
                var simplOpt = new MajorSegmentList();
                simplOpt.Add(new FieldSegment("simplify", "", Field.Type.SIMPLIFICATION));
                list.Insert(0, simplOpt);

                // SEQUENTIAL PROCESSING  
                list.Reverse();
                foreach (var fieldOpts in list)
                {
                    bool isLast = fieldOpts == list[list.Count - 1];
                    pymath.latex = isLast && latex;

                    // PROCESSING SOME `WHERE` AND `FOR` TOKENS
                    for (int i = 1; i < fieldOpts.Count; i++)
                        if (fieldOpts[i] is EqKeyword &&
                            ((fieldOpts[i] as EqKeyword).Type == Keyword.Type.WHERE ||
                            (fieldOpts[i] as EqKeyword).Type == Keyword.Type.FOR) &&
                            CustomData.GetType((fieldOpts[i] as EqKeyword).GetAfterEq()) == CustomData.Type.FIXED &&
                            CustomData.GetType((fieldOpts[i] as EqKeyword).GetBeforeEq()) == CustomData.Type.FIXED &&
                            FixedKeyword.IsVariable((fieldOpts[i] as EqKeyword).GetBeforeEq()))
                            expressionSegment.Substitute((fieldOpts[i] as EqKeyword).GetBeforeEq(), (fieldOpts[i] as EqKeyword).GetAfterEq());
                    res.InterpretedAs.Add(fieldOpts.Build() + " " + expressionSegment.Build());

                    var fieldType = FindField(fieldOpts[0].Keyname);

                    string newExpr = expressionSegment.Build();

                    // BOOLEAN FIELD
                    if (fieldType == FieldType.BOOL)
                    {
                        if (!isLast)
                            continue;
                        var vars = expressionSegment.tokens.ExtractVariables();
                        vars = Functions.MakeUnique(vars);
                        foreach (var v in vars)
                            if (v.Length > 1)
                                throw new Exception(); // TODO
                        var be = new BoolEng(expressionSegment.Build());
                        newExpr = be.CompileTable();
                        string line = "{";
                        foreach (var onevar in vars)
                            line += ": " + onevar + " %";
                        line += ": F %}\n";
                        newExpr = line + newExpr;
                        if (latex)
                        {
                            newExpr = newExpr.Replace(":", "<th>");
                            newExpr = newExpr.Replace("%", "</th>");
                            newExpr = newExpr.Replace("{", "<tr>");
                            newExpr = newExpr.Replace("}", "</tr>");
                            newExpr = newExpr.Replace("[", "<td class=\"cellbool-res\">");
                            newExpr = newExpr.Replace("]", "</td>");
                            newExpr = newExpr.Replace("\n", "");
                            newExpr = "<table id=\"bool-res\">" + res.Result + "</table>";
                        }
                    }
                    else if(AscCalc.MathAnalysis.Contains(fieldType))
                    {
                        expressionSegment.tokens.AddOmittedOps();
                        var vars = expressionSegment.tokens.ExtractVariables();
                        string diffVar;
                        if (fieldOpts.Select(Names.FOR).Count == 0)
                        {
                            if (vars.Contains("x"))
                                diffVar = "x";
                            else if (vars.Contains("y"))
                                diffVar = "y";
                            else
                                diffVar = vars[0];
                            res.InterpretedAs.Add("As " + Names.FOR + " " + diffVar); // TODO
                        }
                        else if(fieldOpts.Select(Names.FOR).Count == 1)
                            diffVar = (fieldOpts.Select(Names.FOR).Item() as FixedKeyword).GetVariable();
                        else
                            diffVar = (fieldOpts.Select(Names.FOR)[0] as FixedKeyword).GetVariable();
                        vars = Functions.MakeUnique(vars);
                        string req = expressionSegment.Build();
                        if(fieldType == FieldType.DERIVATIVE)
                            newExpr = pymath.Derivative(req, diffVar, vars);
                        else if(fieldType == FieldType.INTEGRAL)
                            newExpr = pymath.Integral(req, diffVar, vars);
                        else if(fieldType == FieldType.SOLVE)
                            newExpr = pymath.Integral(req, diffVar, vars);
                    }
                    else if(fieldType == FieldType.SIMPLIFICATION)
                    {
                        expressionSegment.tokens.AddOmittedOps();
                        var vars = expressionSegment.tokens.ExtractVariables();
                        vars = Functions.MakeUnique(vars);
                        string req = expressionSegment.Build();
                        if (isLast)
                        {
                            newExpr = pymath.Simplify(req, vars, true);
                            res.Result = newExpr;
                            break;
                        }
                        else
                            newExpr = pymath.Simplify(req, vars, false);
                    }

                    expressionSegment = new ExpressionSegment(expressionSegment.Keyname, newExpr);
                    res.Result = expressionSegment.Build();
                }

                
                
            }
            catch (Exception e)
            {
                pymath.Destroy();
                throw e;
            }
            pymath.Destroy();
            return res;
        }

        static FieldType FindField(string fieldName)
        {
            if (fieldName == Names.BOOLEAN)
                return FieldType.BOOL;
            else if (fieldName == Names.DERIVATIVE)
                return FieldType.DERIVATIVE;
            else if (fieldName == Names.INTEGRAL)
                return FieldType.INTEGRAL;
            else if (fieldName == Names.SOLVE)
                return FieldType.SOLVE;
            else
                return FieldType.SIMPLIFICATION;
        }
    }
}
