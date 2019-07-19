using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using archives.service.biz.web;
using archives.service.dal.Entity;

namespace archives.service.biz.ifs
{
    public interface IBorrowRegisterService
    {
        Task<CommonResponse<BorrowRegisterResult>> BorrowRegister(BorrowRegisterRequest request);

        Task<CommonSearchResponse<List<SearchBorrowRegisterResult>>> SearchBorrowRegister(SearchBorrowRegisterRequest request);

        Task<CommonResponse<string>> RenewBorrow(RenewBorrowRequest request);

        Task<CommonResponse<GetBorrowDetailResult>> GetBorrowDetail(GetBorrowDetailRequest request);

        Task<CommonResponse<string>> ReturnArchives(ReturnBorrowRequest request);

        Task<CommonResponse<string>> ConfirmBorrowed(ConfirmBorrowedRequest request);

        Task<CommonResponse<string>> BorrowRegisterNotify(int dayLimit);
    }
}
