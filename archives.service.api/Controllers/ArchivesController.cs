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

        /// <summary>
        /// 档案查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<CommonSearchResponse<List<ArchivesSearchResult>>> SearchArchives([FromQuery]ArchivesSearchRequest request)
        {
            return await _archivesService.SearchArchives(request);
        }

        /// <summary>
        /// 获取单个档案详情
        /// </summary>
        /// <param name="archivesId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<CommonResponse<ArchivesInfo>> GetArchives([FromQuery]int archivesId)
        {
            return await _archivesService.GetArchives(archivesId);
        }

        [HttpGet]
        public async Task<CommonResponse<List<string>>> QueryAllProjectName()
        {
            var response = new CommonResponse<List<string>>();
            try
            {
                response.Data = await _archivesService.QueryAllProject();
                response.Success = true;
            }
            catch
            {
                response.Message = "获取项目名称发生异常";
            }
            return response;
        }

        [HttpPost]
        public async Task<CommonResponse<ArchivesEditResult>> EditArchives(ArchivesEditRequest request)
        {
            return await _archivesService.Edit(request);
        }

        [HttpPost]
        public async Task<CommonResponse<ArchivesAddResult>> AddArchives(ArchivesAddRequest request)
        {
            return await _archivesService.Add(request);
        }

        [HttpPost]
        public async Task<CommonResponse<ArchivesDeleteResult>> DeleteArchives(ArchivesDeleteRequest request)
        {
            return await _archivesService.Delete(request);
        }
    }
}
