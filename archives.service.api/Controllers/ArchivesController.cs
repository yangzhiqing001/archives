using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using archives.service.biz.ifs;
using archives.service.biz.web;
using archives.service.dal.Entity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace archives.service.api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class ArchivesController : ControllerBase
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
        public async Task<CommonResponse<ArchivesInfo>> GetArchives([FromQuery]int archivesId)
        {
            return await _archivesService.GetArchives(archivesId);
        }
    }
}
