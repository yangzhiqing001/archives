using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace archives.gateway.Controllers
{
    public class ArchivesController : Controller
    {
        public IActionResult BatchImport()
        {
            return View();
        }
    }
}