﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace archives.gateway.Controllers
{
    public class BorrowController : BaseController
    {
        // GET: /<controller>/
        [Authorize]
        public IActionResult List()
        {
            return View(getUser());
        }
    }
}
