using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using archives.service.biz.web;
using archives.service.biz.ifs;
using Microsoft.AspNetCore.Mvc;
using archives.service.dal.Entity;
using archives.common;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace archives.service.api.Controllers
{
    /// <summary>
    /// 借阅相关接口
    /// </summary>
    [Route("api/[controller]/[action]")]
    public class BorrowController : ControllerBase
    {
		private readonly IBorrowRegisterService _borrowRegisterService;
        public BorrowController(IBorrowRegisterService borrowRegisterService)
        {
			_borrowRegisterService = borrowRegisterService;
		}

        /// <summary>
        /// 借阅登记
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<CommonResponse<BorrowRegisterResult>> Register([FromBody]BorrowRegisterRequest request)
        {
            request.SerializeToLog("Register");
            var response = await _borrowRegisterService.BorrowRegister(request);
            response.SerializeToLog("Register");
            return response;
        }

        /// <summary>
        /// 借阅列表查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<CommonSearchResponse<List<SearchBorrowRegisterResult>>> SearchBorrowRegister(SearchBorrowRegisterRequest request)
        {
            return await _borrowRegisterService.SearchBorrowRegister(request);
        }

        /// <summary>
        /// 获取借阅详情（根据借阅ID，供借阅列表选中使用）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<CommonResponse<GetBorrowDetailResult>> GetBorrowDetail(GetBorrowDetailRequest request)
        {
            return await _borrowRegisterService.GetBorrowDetail(request);
        }

        /// <summary>
        /// 续借（延期）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<CommonResponse<string>> RenewBorrow([FromBody]RenewBorrowRequest request)
        {
            request.SerializeToLog("RenewBorrow");
            var response = await _borrowRegisterService.RenewBorrow(request);
            response.SerializeToLog("RenewBorrow");
            return response;
        }

        /// <summary>
        /// 归还接口
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<CommonResponse<string>> ReturnBorrow([FromBody]ReturnBorrowRequest request)
        {
            request.SerializeToLog("ReturnArchives");
            var response = await _borrowRegisterService.ReturnArchives(request);
            response.SerializeToLog("ReturnArchives");
            return response;
        }

        /// <summary>
        /// 确认借出（管理员使用）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<CommonResponse<string>> ConfirmBorrowed([FromBody]ConfirmBorrowedRequest request)
        {
            request.SerializeToLog("ConfirmBorrowed");
            var response = await _borrowRegisterService.ConfirmBorrowed(request);
            response.SerializeToLog("ConfirmBorrowed");
            return response;
        }

        /// <summary>
        /// 催还（管理员使用）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<CommonResponse<string>> ReturnWarn([FromBody]ReturnWarnRequest request)
        {
            request.SerializeToLog("ReturnWarn");
            var response = await _borrowRegisterService.ReturnWarn(request);
            response.SerializeToLog("ReturnWarn");
            return response;
        }

        /// <summary>
        /// 短信通知借阅归还
        /// </summary>
        /// <param name="dayLimit"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<CommonResponse<string>> BorrowReturnNotify([FromQuery]int dayLimit)
        {
            return await _borrowRegisterService.BorrowRegisterNotify(dayLimit);
        }
    }
}
