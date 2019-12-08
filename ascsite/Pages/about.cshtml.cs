using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace ascsite.Pages
{
    public class AboutModel : PageModel
    {
        private readonly ILogger<AboutModel> logger;

        public AboutModel(ILogger<AboutModel> logger)
        {
            this.logger = logger;
        }
        
        public void OnGet()
        {
            
        }
    }
}
