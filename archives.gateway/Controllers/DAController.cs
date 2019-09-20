using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace archives.gateway.Controllers
{
    [Authorize]
    public class DAController : Controller
    {
        [Authorize]
        public IActionResult Manage()
        {
            return View();
        }

        public IActionResult Upload()
        {
            return View();
        }
    }
}
