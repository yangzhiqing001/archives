using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using archives.service.biz.ifs;
using archives.service.biz.web;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace archives.service.api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class ArchivesController : Controller
    {
        private readonly IArchivesInfoService _archivesService;
        public ArchivesController(IArchivesInfoService archivesService)
        {
            _archivesService = archivesService;
        }

        [HttpGet]
        public async Task<CommonSearchResponse<List<ArchivesSearchResult>>> SearchArchives([FromQuery]ArchivesSearchRequest request)
        {
            return await _archivesService.SearchArchives(request);
        }

        [HttpGet]
        public async Task<CommonResponse<ArchivesDteailResult>> GetArchives([FromQuery]string archivesId)
        {
            return await _archivesService.GetArchives(archivesId);
        }
    }
}
