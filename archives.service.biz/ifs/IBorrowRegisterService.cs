using System;
using System.Threading.Tasks;
using archives.service.biz.web;

namespace archives.service.biz.ifs
{
    public interface IBorrowRegisterService
    {
        Task<CommonResponse<BorrowRegisterResult>> BorrowRegister(BorrowRegisterRequest request);
    }
}
