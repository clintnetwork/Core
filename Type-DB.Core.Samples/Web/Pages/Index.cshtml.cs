using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace TypeDB.Samples
{
    public class IndexModel : PageModel
    {
        private readonly Database _settings;

        public IndexModel(Database settings)
        {
            _settings = settings;
        }
        
        public void OnGet()
        {
            ViewData["Test"] = _settings.Get<DateTime>("date_time");
        }
    }
}
