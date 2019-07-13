using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using archives.service.biz.ifs;
using archives.service.biz.web;
using archives.service.dal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace archives.service.api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IArchivesInfoService archivesInfoService;
        public ValuesController(IArchivesInfoService archivesInfoService)
        {
            this.archivesInfoService = archivesInfoService;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<CommonResponse<IEnumerable<string>>> Test1()
        {
            //var list = archivesInfoService.GetList();
            var response = new CommonResponse<IEnumerable<string>>
            {
                Data = new string[] { "Test1", "anonymous" },
                Success = true
            };
            return response;
        }

        [HttpGet]
        //[Authorize(Roles = "admin")]
        public ActionResult<CommonResponse<IEnumerable<string>>> Test2()
        {
            //var list = archivesInfoService.GetList();
            var response = new CommonResponse<IEnumerable<string>>
            {
                Data = new string[] { "Test2", "login" },
                Success = true
            };
            return response;
        }

        [HttpGet]
        public ActionResult<CommonResponse<IEnumerable<string>>> Test3()
        {
            //var list = archivesInfoService.GetList();
            var response = new CommonResponse<IEnumerable<string>>
            {
                Data = new string[] { "Test3", "allow" },
                Success = true
            };
            return response;
        }
    }
}
