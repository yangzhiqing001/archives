using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using archives.service.biz.web;
using archives.service.biz.ifs;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace archives.service.api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class BorrowController : ControllerBase
    {
		private readonly IBorrowRegisterService _borrowRegisterService;
        public BorrowController(IBorrowRegisterService borrowRegisterService)
        {
			_borrowRegisterService = borrowRegisterService;
		}

        public async Task<CommonResponse<BorrowRegisterResult>> Register(BorrowRegisterRequest request)
        {
            return await _borrowRegisterService.BorrowRegister(request);
        }

    }
}
