using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using archives.common;
using archives.gateway.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace archives.gateway.Controllers
{
    public class BaseController : Controller
    {
        // GET: /<controller>/
        protected LoginUserModel getUser()
        {
            if (HttpContext != null && HttpContext.User != null && HttpContext.User.Identity != null && HttpContext.User.Identity.IsAuthenticated)
            {
                return JsonHelper.Deserialize<LoginUserModel>(HttpContext.User.Identity.Name);
            }
            else
                return null;
        }
    }
}
