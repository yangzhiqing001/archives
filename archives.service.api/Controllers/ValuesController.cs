using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using archives.service.biz.ifs;
using archives.service.dal;
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
        public ActionResult<IEnumerable<string>> Test1()
        {
            //var list = archivesInfoService.GetList();
            
            return new string[] { "Test1", "Test1" };
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Test2()
        {
            //var list = archivesInfoService.GetList();

            return new string[] { "Test2", "Test2" };
        }
    }
}
