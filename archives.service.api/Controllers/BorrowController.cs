using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using archives.service.biz.web;
using archives.service.biz.ifs;
using Microsoft.AspNetCore.Mvc;
using archives.service.dal.Entity;

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
            return await _borrowRegisterService.BorrowRegister(request);
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
            return await _borrowRegisterService.RenewBorrow(request);
        }

        /// <summary>
        /// 归还接口
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<CommonResponse<string>> ReturnBorrow([FromBody]ReturnBorrowRequest request)
        {
            return await _borrowRegisterService.ReturnArchives(request);
        }

        /// <summary>
        /// 确认借出（管理员使用）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<CommonResponse<string>> ConfirmBorrowed([FromBody]ConfirmBorrowedRequest request)
        {
            return await _borrowRegisterService.ConfirmBorrowed(request);
        }
    }
}
