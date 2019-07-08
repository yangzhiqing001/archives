using System;
using System.Threading.Tasks;
using archives.service.biz.exp;
using archives.service.biz.ifs;
using archives.service.biz.web;
using archives.service.dal;
using archives.service.dal.Entity;
using System.Linq;

namespace archives.service.biz.impl
{
    public class BorrowRegisterService : IBorrowRegisterService
    {
        public ArchivesContext _db;
        public BorrowRegisterService(ArchivesContext db)
        {
            _db = db;
        }

        public async Task<CommonResponse<BorrowRegisterResult>> BorrowRegister(BorrowRegisterRequest request)
        {
            var response = new CommonResponse<BorrowRegisterResult>();
            var regEntity = new dal.Entity.BorrowRegister
            {
                Borrower = request.Borrwoer,
                Phone = request.Phone,
                ReturnDate = request.ReturnDate,
                SignPhoto = request.SignPhoto,
                Status = BorrowRegisterStatus.Registered
            };

            using (var trans = await _db.Database.BeginTransactionAsync())
            {
                try
                {
                    await _db.BorrowRegister.AddAsync(regEntity);
                    await _db.SaveChangesAsync();

                    response.Data = new BorrowRegisterResult { BorrowRegisterId = regEntity.Id };

                    await _db.BorrowRegisterDetail.AddRangeAsync(request.ArchivesId.Select(c => new dal.Entity.BorrowRegisterDetail
                    {
                        ArchivesId = c,
                        BorrowRegisterId = regEntity.Id,
                        CreateTime = DateTime.Now
                    }));

                    trans.Commit();
                    response.Success = true;
                }
                catch
                {
                    trans.Rollback();
                    response.Success = false;
                    response.Message = "提交申请借阅发生异常";
                }
            }

            return response;
        }
    }
}
