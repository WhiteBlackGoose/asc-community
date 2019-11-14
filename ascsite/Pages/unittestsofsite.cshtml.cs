using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AscSite.Tests.calc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ascsite.Pages
{
    public class unittestsofsiteModel : PageModel
    {
        public string Res { get; set; }
        public void OnGet()
        {
            var test = new CalcTest();
            Res = test.Test().Compile();
        }
    }
}