using ascsite.Core.AscSci.Calculator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AscSite.Tests.calc
{
    public class CalcTest : UnitTest
    {
        private static List<string> Samples = new List<string>() {
            "1+1",
            "1+b",
            "a^b",
            "2x + 3y4 - 4sqrt(x)",
            "derivative x+3",
            "2^b where b=3",
            "derivative 2^b",
            "integrate 2x",
            "solve for x x+3",
            "integrate for x pi",
            "derivative integrate x",
            "solve derivative x3 - x",
            "derivative for t where z = sqrt(x - t) derivative z4 - z2",
            "boolean a == b",
            "boolean a & b -> 0",
            "boolean 0",
            "boolean a&b&c&!d",
            "derivative for x where z=5*x solve for r r2*z - 3r*x - 4x*z2 + t for t=1 + z",
            "(x + 2) / 2x",
            "x where z=3",
            "derivative integrate sin(x) * tan(x) - sqrt(log(sqrt(3), 3) - x) + y3x - 6cos(y + x)",
            "solve for var integrate var2 - 2var + 1",

        };
        public override UnitTestReport Test()
        {
            var report = new UnitTestReport();
            foreach (var exp in Samples)
            {
                var calc = new AscCalc(exp);
                try
                {
                    calc.Compute();
                } catch(Exception e)
                {
                    report.Add(exp, e.Message);
                }
            }
            return report;
        }
    }
}
