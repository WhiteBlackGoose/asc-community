using AscSite.Core.AscSci.AscMath;
using AngouriMath;
using Processor;
using Processor.lexicProcessor;
using Processor.syntaxProcessor;
using Processor.syntaxProcessor.tokens;
using Processor.syntaxProcessor.tokens.types;
using System;
using System.Collections.Generic;
using System.Linq;


namespace ascsite.Core.AscSci.Calculator
{
    public class CalcResult
    {
        public List<string> InterpretedAs { get; set; }
        public string Result { get; set; }
        public string LatexResult { get; set; }
        public string SolidResult { get { return Result.Replace("\n", ", ").Replace('I', 'i'); } }

        public string ProcessedResult
        {
            get
            {
                return Result.Replace("\n", "<br>");
            }
        }
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
            SOLVE,
            SIMPLIFICATION,
            PLOT
        }

        private static List<FieldType> MathAnalysis = new List<FieldType>()
        {
            FieldType.DERIVATIVE,
            FieldType.SOLVE,
        };

        private readonly bool latex;
        public AscCalc(string expr, bool latex = false)
        {
            rawExpression = expr.Trim();
            this.latex = latex;
        }

        public List<MajorSegmentList> DivideByField(MajorSegmentList list)
        {
            var res = new List<MajorSegmentList>();
            foreach (var elem in list)
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

            try
            {
                // INITIALIZATION OF STUFF
                Lexer lexer = new Lexer(rawExpression);
                TokenList tokenList = lexer.GenerateTokens();
                Parser parser = new Parser(tokenList);
                PostTokenList postTokenList = parser.Parse();
                postTokenList.Freeze();
                MathPreprocessor mathPreprocessor = new MathPreprocessor(postTokenList);
                mathPreprocessor.Process();
                MajorSegmentList majorSegmentList = mathPreprocessor.GetSegments();
                List<ExpressionSegment> expressionSegments = new List<ExpressionSegment>();
                expressionSegments.Add(
                    (ExpressionSegment)majorSegmentList.Select("expression").Item()
                    );

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
                        expressionSegments[0].Substitute(v, expr);
                    }
                    else
                        majorSegmentList[i].Ignored = true;
                }

                MajorSegmentList newExpression = majorSegmentList.CutTill(expressionSegments[0]);
                res.InterpretedAs.Add(expressionSegments[0].Build());
                List<MajorSegmentList> list = DivideByField(newExpression);
                var simplOpt = new MajorSegmentList();
                simplOpt.Add(new FieldSegment("simplify", "", Field.Type.SIMPLIFICATION));
                list.Insert(0, simplOpt);

                // SEQUENTIAL PROCESSING
                list.Reverse();
                foreach (var fieldOpts in list)
                {
                    bool isLast = fieldOpts == list[list.Count - 1];

                    // PROCESSING SOME `WHERE` AND `FOR` TOKENS
                    foreach (var expressionSegment in expressionSegments)
                    {
                        for (int i = 1; i < fieldOpts.Count; i++)
                            if (fieldOpts[i] is EqKeyword &&
                                ((fieldOpts[i] as EqKeyword).Type == Keyword.Type.WHERE ||
                                (fieldOpts[i] as EqKeyword).Type == Keyword.Type.FOR) &&
                                CustomData.GetType((fieldOpts[i] as EqKeyword).GetAfterEq()) == CustomData.Type.FIXED &&
                                CustomData.GetType((fieldOpts[i] as EqKeyword).GetBeforeEq()) == CustomData.Type.FIXED &&
                                FixedKeyword.IsVariable((fieldOpts[i] as EqKeyword).GetBeforeEq()))
                                expressionSegment.Substitute((fieldOpts[i] as EqKeyword).GetBeforeEq(), (fieldOpts[i] as EqKeyword).GetAfterEq());
                        res.InterpretedAs.Add(fieldOpts.Build() + " " + expressionSegment.Build());
                    }

                    var fieldType = FindField(fieldOpts[0].Keyname);

                    List<string> newExprs = new List<string>();
                    List<string> newLatexExprs = new List<string>();
                    bool quickExit = false;
                    foreach (var expressionSegment in expressionSegments)
                    {
                        if (fieldType == FieldType.BOOL)
                        {
                            var vars = expressionSegment.tokens.ExtractVariables();
                            vars = Functions.MakeUnique(vars);
                            foreach (var v in vars)
                                if (v.Length > 1)
                                    throw new ParsingException("Multi-symbol vars are forbidden in boolean mode");
                            var be = new BoolEng(expressionSegment.Build());
                            string newExpr;
                            newExpr = be.CompileTable();
                            string line = "{";
                            foreach (var onevar in vars)
                                line += ": " + onevar + " %";
                            line += ": F %}\n";
                            newExpr = line + newExpr;
                            newExprs.Add(newExpr);
                            {
                                newExpr = newExpr.Replace(":", "<th>");
                                newExpr = newExpr.Replace("%", "</th>");
                                newExpr = newExpr.Replace("{", "<tr>");
                                newExpr = newExpr.Replace("}", "</tr>");
                                newExpr = newExpr.Replace("[", "<td class=\"cellbool-res\">");
                                newExpr = newExpr.Replace("]", "</td>");
                                newExpr = newExpr.Replace("\n", "");
                                newExpr = "<table id=\"bool-res\">" + newExpr + "</table>";
                            }
                            newLatexExprs.Add(newExpr);
                            quickExit = true;
                            break;
                        }
                        else if (AscCalc.MathAnalysis.Contains(fieldType))
                        {
                            expressionSegment.tokens.AddOmittedOps();
                            var vars = expressionSegment.tokens.ExtractVariables();
                            string diffVar;
                            vars = Functions.MakeUnique(vars);
                            if (fieldOpts.Select(Names.FOR).Count == 0)
                            {
                                if (vars.Contains("x") || vars.Count == 0)
                                    diffVar = "x";
                                else if (vars.Contains("y"))
                                    diffVar = "y";
                                else
                                    diffVar = vars[0];
                                res.InterpretedAs.Add("Interpreted as `" + Names.FOR + " " + diffVar + "`");
                            }
                            else if (fieldOpts.Select(Names.FOR).Count == 1)
                                diffVar = (fieldOpts.Select(Names.FOR).Item() as FixedKeyword).GetVariable();
                            else
                                diffVar = (fieldOpts.Select(Names.FOR)[0] as FixedKeyword).GetVariable();
                            vars.Add(diffVar);
                            vars = Functions.MakeUnique(vars);
                            string req = expressionSegment.Build();
                            if (fieldType == FieldType.DERIVATIVE)
                            {
                                newExprs.Add(MathS.FromString(req).Derive(MathS.Var(diffVar)).ToString());
                                if (isLast && latex)
                                    newLatexExprs.Add("$$" + MathS.FromString(req).Derive(MathS.Var(diffVar)).Latexise().ToString() + "$$");
                            }
                            else if (fieldType == FieldType.SOLVE)
                            {
                                expressionSegment.tokens.AddOmittedOps();
                                if (vars.Count > 1)
                                    throw new InvalidRequestException();
                                var roots = MathS.FromString(req).SolveNt(MathS.Var(diffVar), precision: 400);
                                foreach(var root in roots)
                                    newExprs.Add(root.ToString());
                                if (isLast && latex)
                                    foreach (var root in roots)
                                        newExprs.Add("$$" + root.ToString() + "$$");
                            }
                        }
                        
                        else if (fieldType == FieldType.SIMPLIFICATION)
                        {
                            expressionSegment.tokens.AddOmittedOps();
                            var vars = expressionSegment.tokens.ExtractVariables();
                            vars = Functions.MakeUnique(vars);
                            string req = expressionSegment.Build();
                            string newExpr;
                            newExpr = MathS.FromString(req).Simplify(6).ToString();
                            newExprs.Add(newExpr);
                            if(isLast && latex)
                            {
                                newExpr = MathS.FromString(req).Simplify(6).Latexise();
                                newLatexExprs.Add("$$" + newExpr + "$$");
                            }
                        }
                    }
                    if (newExprs.Count == 0)
                    {
                        res.Result = "No answer";
                        break;
                    }
                    if (!isLast && !quickExit)
                    {
                        expressionSegments = new List<ExpressionSegment>();
                        foreach (var nexp in newExprs)
                            expressionSegments.Add(new ExpressionSegment("expression", nexp));
                    }
                    else
                    {
                        res.Result = "";
                        foreach (var segm in newExprs)
                            res.Result += segm + " ";
                        res.LatexResult = "";
                        foreach (var segm in newLatexExprs)
                            res.LatexResult += segm + " ";
                        break; // For quickExit case
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return res;
        }

        static FieldType FindField(string fieldName)
        {
            if (fieldName == Names.BOOLEAN)
                return FieldType.BOOL;
            else if (fieldName == Names.DERIVATIVE)
                return FieldType.DERIVATIVE;
            else if (fieldName == Names.SOLVE)
                return FieldType.SOLVE;
            else if (fieldName == Names.PLOT)
                return FieldType.PLOT;
            else
                return FieldType.SIMPLIFICATION;
        }
    }
}
