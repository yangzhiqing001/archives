using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using archives.service.biz.web;
using archives.service.biz.exp;
using NPOI.SS.UserModel;
using archives.gateway.Models;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace archives.gateway.Controllers
{
    [Authorize]
    public class DAController : BaseController
    {
        [Authorize]
        public IActionResult Manage()
        {
            return View(getUser());
        }

        [Authorize]
        public IActionResult New()
        {
            return View(getUser());
        }

        [Authorize]
        public IActionResult Edit(string id)
        {
            EditArchiveModel model = new EditArchiveModel(getUser());
            model.Id = id;
            return View(model);
        }

        [Authorize]
        public IActionResult Upload()
        {
            return View();
        }

    }
}