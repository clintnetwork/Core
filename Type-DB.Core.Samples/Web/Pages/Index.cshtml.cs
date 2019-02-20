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
        private IOptions<Settings> _settings;

        public IndexModel(IOptions<Settings> settings)
        {
            _settings = settings;
        }
        
        public void OnGet()
        {
            var x = _settings.Value.FullName;
        }
    }
}
