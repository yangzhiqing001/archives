using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using archives.common;
using archives.service.biz.ifs;
using archives.service.biz.web;
using archives.service.dal.Entity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace archives.service.api.Controllers
{
    /// <summary>
    /// 档案管理相关接口
    /// </summary>
    [Route("api/[controller]/[action]")]
    public class ArchivesController : ControllerBase
    {
        /// <summary>
        /// 档案接口
        /// </summary>
        private readonly IArchivesInfoService _archivesService;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="archivesService"></param>
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
            var response = await _archivesService.SearchArchives(request);
            return response;
        }

        /// <summary>
        /// 获取单个档案详情
        /// </summary>
        /// <param name="archivesId">档案Id(唯一值，也就是查询档案返回的Id)</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<CommonResponse<ArchivesInfo>> GetArchives([FromQuery]int archivesId)
        {
            return await _archivesService.GetArchives(archivesId);
        }

        /// <summary>
        /// 获取所有档案项目名（用在前端搜索档案的label参数）
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 编辑档案
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<CommonResponse<ArchivesEditResult>> EditArchives([FromBody]ArchivesEditRequest request)
        {
            request.SerializeToLog("SearchArchives request");
            var response = await _archivesService.Edit(request);
            response.SerializeToLog("SearchArchives response");
            return response;
        }

        /// <summary>
        /// 添加档案
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<CommonResponse<ArchivesAddResult>> AddArchives([FromBody]ArchivesAddRequest request)
        {
            request.SerializeToLog("AddArchives request");
            var response = await _archivesService.Add(request);
            response.SerializeToLog("AddArchives response");
            return response;
        }

        /// <summary>
        /// 删除档案
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<CommonResponse<ArchivesDeleteResult>> DeleteArchives([FromBody]ArchivesDeleteRequest request)
        {
            request.SerializeToLog("DeleteArchives request");
            var response = await _archivesService.Delete(request);
            response.SerializeToLog("DeleteArchives response");
            return response;
        }
    }
}
