﻿using System;
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
            var response = await _borrowRegisterService.BorrowRegisterNotify(dayLimit);
            response.SerializeToLog("BorrowReturnNotify");
            return response;
        }

        [HttpPost]
        public async Task<CommonResponse<string>> CloseBorrow([FromBody]CloseBorrowRequest request)
        {
            request.SerializeToLog("CloseBorrow");
            var response = await _borrowRegisterService.CloseBorrow(request);
            response.SerializeToLog("CloseBorrow");
            return response;
        }

        [HttpGet]
        public async Task<ActionResult> Export(SearchBorrowRegisterRequest request)
        {
            try
            {
                var list = await _borrowRegisterService.QueryAllBorrowRegisters(request);

                System.IO.MemoryStream output = new System.IO.MemoryStream();

                System.IO.StreamWriter writer = new System.IO.StreamWriter(output, System.Text.Encoding.UTF8);
                writer.Write("借阅时间,借阅单位,借阅人,工程名称,借阅条目,归还日期,接收人,备注");

                writer.WriteLine();

                //输出内容
                list.ForEach(a => {
                    writer.Write($"\"{a.CreateTimeStr}\",\"");//第一列
                    writer.Write($"{a.Department}\",\"");
                    writer.Write($"{a.Borrower}\",\"");
                    writer.Write($"{a.ProjectName}\",\"");
                    writer.Write($"{a.ArchivesStr}\",\"");
                    writer.Write($"{a.ReturnDateStr}\",\"");
                    writer.Write($"{a.Receiver}\",\"");
                    writer.Write($"{a.StatusDesc}\",");
                    writer.WriteLine();
                });

                writer.Flush();

                output.Position = 0;

                return File(output, "application/ms-excel", "借阅记录.csv");
            }
            catch (Exception ex)
            {
                ApplicationLog.Error("Export Excetpion", ex);
                return Ok(ex.Message);
            }
        }

        /// <summary>
        /// 获取所有接收人名称
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<CommonResponse<List<string>>> QueryReceiver()
        {
            var response = new CommonResponse<List<string>>();
            try
            {
                response.Data = await _borrowRegisterService.QueryReceiver();
                response.Success = true;
            }
            catch
            {
                response.Message = "获取接收人发生异常";
            }
            return response;
        }
    }
}
