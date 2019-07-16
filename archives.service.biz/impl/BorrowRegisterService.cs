using archives.service.biz.ifs;
using archives.service.biz.web;
using archives.service.dal;
using archives.service.dal.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using archives.service.biz.exp;

namespace archives.service.biz.impl
{
    public class BorrowRegisterService : IBorrowRegisterService
    {
        public ArchivesContext _db;
        public BorrowRegisterService(ArchivesContext db)
        {
            _db = db;
        }

        /// <summary>
        /// 申报借阅
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<CommonResponse<BorrowRegisterResult>> BorrowRegister(BorrowRegisterRequest request)
        {
            var response = new CommonResponse<BorrowRegisterResult>();

            if (string.IsNullOrEmpty(request.Phone))
            {
                response.Message = "手机不能为空";
                return response;
            }
            if (string.IsNullOrEmpty(request.Borrwoer))
            {
                response.Message = "借阅人不能为空";
                return response;
            }
            if (request.ReturnDate < DateTime.Now)
            {
                response.Message = "归还日期不能早于今天";
                return response;
            }

            var regEntity = new dal.Entity.BorrowRegister
            {
                Borrower = request.Borrwoer,
                Phone = request.Phone,
                ReturnDate = request.ReturnDate,
                SignPhoto = request.SignPhoto,
                Status = BorrowRegisterStatus.Registered,
                Company = request.Company,
                Department = request.Department,
                CreateTime = DateTime.Now,
                Deleted = false,
                UpdateTime = DateTime.Now
            };

            using (var trans = await _db.Database.BeginTransactionAsync())
            {
                try
                {
                    var archives = await _db.ArchivesInfo.AsNoTracking().Where(c => request.ArchivesId.Contains(c.Id) && !c.Deleted).ToListAsync();

                    if (archives.Count != request.ArchivesId.Count)
                    {
                        throw new BizException("请求档案数目与数据库可用档案不一致，可能是数据已删除，请重新查询后再提交");
                    }

                    var borrowedArchives = archives.Where(c => c.Status == ArchivesStatus.Borrowed);
                    if (borrowedArchives.Any())
                    {
                        throw new BizException($"请求档案：{string.Join(",", borrowedArchives.Select(c => c.ArchivesNumber))} 当前状态为已借阅");
                    }

                    await _db.BorrowRegister.AddAsync(regEntity);
                    await _db.SaveChangesAsync();

                    response.Data = new BorrowRegisterResult { BorrowRegisterId = regEntity.Id };

                    await _db.BorrowRegisterDetail.AddRangeAsync(request.ArchivesId.Select(c => new dal.Entity.BorrowRegisterDetail
                    {
                        ArchivesId = c,
                        BorrowRegisterId = regEntity.Id,
                        CreateTime = DateTime.Now
                    }));

                    await _db.SaveChangesAsync();

                    trans.Commit();
                    response.Success = true;
                }
                catch (BizException ex)
                {
                    trans.Rollback();
                    response.Message = ex.Message;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    response.Message = "提交申请借阅发生异常";
                }
            }

            return response;
        }

        public async Task<CommonSearchResponse<List<BorrowRegister>>> SearchBorrowRegister(SearchBorrowRegisterRequest request)
        {
            var response = new CommonSearchResponse<List<BorrowRegister>>();
            try
            {
                var query = _db.BorrowRegister.Where(c => c.Status != BorrowRegisterStatus.Returned && !c.Deleted);
                if (!string.IsNullOrEmpty(request.Keyword))
                {
                    query = query.Where(c => c.Phone.Contains(request.Keyword.Trim()) || c.Borrower.Contains(request.Keyword.Trim()) || c.Company.Contains(request.Keyword.Trim()) || c.Department.Contains(request.Keyword.Trim()));
                }
                var list = await query.OrderBy(c => c.Status).ThenBy(c => c.Id)
                        .Skip(request.PageNumber * request.PageSize)
                        .Take(request.PageSize)
                        .ToListAsync();

                response.Data = list;
                response.Success = true;
            }
            catch
            {
                response.Message = "获取借阅列表失败";
            }

            return response;
        }

        /// <summary>
        /// 获取借阅详情
        /// </summary>
        public async Task<CommonResponse<GetBorrowDetailResult>> GetBorrowDetail(GetBorrowDetailRequest request)
        {
            var response = new CommonResponse<GetBorrowDetailResult>();
            try
            {
                var borrowRegister = await _db.BorrowRegister.FirstOrDefaultAsync(c => c.Id == request.BorrowRegisterId && !c.Deleted);
                if (borrowRegister == null)
                    throw new BizException("借阅登记不存在");

                var archivesList = await _db.ArchivesInfo.Join(_db.BorrowRegisterDetail, a => a.Id, b => b.ArchivesId, (a, b) => new { a, b }).Where(j => j.b.BorrowRegisterId == borrowRegister.Id).Select(c => new ArchivesSearchResult
                {
                    Id = c.a.Id,
                    ArchivesNumber = c.a.ArchivesNumber,
                    CategoryId = c.a.CategoryId,
                    FileNumber = c.a.FileNumber,
                    ProjectName = c.a.ProjectName,
                    Title = c.a.Title
                }).ToListAsync();

                response.Data = new GetBorrowDetailResult
                {
                    BorrowRegister = borrowRegister,
                    ArchivesList = archivesList
                };
                response.Success = true;
            }
            catch (BizException ex)
            {
                response.Message = ex.Message;
            }
            catch
            {
                response.Message = "获取借阅详情发生异常";
            }
            return response;
        }

        /// <summary>
        /// 续借
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<CommonResponse<string>> RenewBorrow(RenewBorrowRequest request)
        {
            var response = new CommonResponse<string>();
            try
            {
                var borrowRegister = await _db.BorrowRegister.FirstOrDefaultAsync(c => c.Id == request.BorrowRegisterId && !c.Deleted);
                if (borrowRegister == null)
                    throw new BizException("借阅登记不存在");

                if (borrowRegister.Status == BorrowRegisterStatus.Renewed || borrowRegister.Status == BorrowRegisterStatus.Overdue || borrowRegister.Status == BorrowRegisterStatus.Borrowed)
                    throw new BizException("借阅登记状态为：已借出、延期、逾期 才能续借");

                borrowRegister.ReturnDate = request.RenewDate;
                borrowRegister.Status = BorrowRegisterStatus.Renewed;
                borrowRegister.UpdateTime = DateTime.Now;
                await _db.SaveChangesAsync();
            }
            catch (BizException ex)
            {
                response.Message = ex.Message;
            }
            catch
            {
                response.Message = "提交续借发生异常";
            }
            return response;

        }

        /// <summary>
        /// 归还
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<CommonResponse<string>> ReturnArchives(ReturnBorrowRequest request)
        {
            var response = new CommonResponse<string>();
            try
            {
                var borrowRegister = await _db.BorrowRegister.FirstOrDefaultAsync(c => c.Id == request.BorrowRegisterId && !c.Deleted);
                if (borrowRegister == null)
                    throw new BizException("借阅登记不存在");

                if (borrowRegister.Status == BorrowRegisterStatus.Returned)
                {
                    response.Success = true;
                    return response;
                }

                if (borrowRegister.Status != BorrowRegisterStatus.Borrowed && borrowRegister.Status != BorrowRegisterStatus.Overdue && borrowRegister.Status != BorrowRegisterStatus.Renewed)
                {
                    throw new BizException("借阅登记状态为：已借出、延期、逾期 才需要归还");
                }
                borrowRegister.UpdateTime = DateTime.Now;
                borrowRegister.Status = BorrowRegisterStatus.Returned;
                borrowRegister.UpdateTime = DateTime.Now;

                var archivesList = await _db.ArchivesInfo.Join(_db.BorrowRegisterDetail, a => a.Id, b => b.ArchivesId, (a, b) => new { a, b }).Where(j => j.b.BorrowRegisterId == borrowRegister.Id).Select(c => c.a).ToListAsync();
                archivesList.ForEach(a => a.Status = ArchivesStatus.Normal);
                await _db.SaveChangesAsync();
                response.Success = true;
            }
            catch (BizException ex)
            {
                response.Message = ex.Message;
            }
            catch
            {
                response.Message = "提交续借发生异常";
            }
            return response;
        }

        public async Task<CommonResponse<string>> ConfirmBorrowed(ConfirmBorrowedRequest request)
        {
            var response = new CommonResponse<string>();

            using (var trans = await _db.Database.BeginTransactionAsync())
            {
                try
                {
                    var borrowRegister = await _db.BorrowRegister.FirstOrDefaultAsync(c => c.Id == request.BorrowRegisterId && !c.Deleted);
                    if (borrowRegister == null)
                        throw new BizException("借阅登记不存在");

                    if (borrowRegister.Status == BorrowRegisterStatus.Borrowed)
                    {
                        response.Success = true;
                        return response;
                    }

                    if (borrowRegister.Status != BorrowRegisterStatus.Registered)
                    {
                        throw new BizException("借阅登记状态为：已登记 才能确认借出");
                    }

                    var archives = await _db.ArchivesInfo.Join(_db.BorrowRegisterDetail, a => a.Id, b => b.ArchivesId, (a, b) => new { a, b }).Where(j => j.b.BorrowRegisterId == borrowRegister.Id).Select(c => c.a).ToListAsync();
                    //如果当前状态为init，将档案改为
                    archives.ForEach(c =>
                    {
                        switch (c.Status)
                        {
                            case ArchivesStatus.Init:
                            case ArchivesStatus.Normal:
                                c.Status = ArchivesStatus.Borrowed;
                                break;
                            case ArchivesStatus.Borrowed:
                                throw new BizException($"档案：{c.ArchivesNumber}当前状态为 已借出，无法再次借出");
                            default:
                                throw new BizException($"档案：{c.ArchivesNumber}当前状态出错，无法确认借出");
                        }
                    });
                    borrowRegister.UpdateTime = DateTime.Now;
                    borrowRegister.Status = BorrowRegisterStatus.Borrowed;
                    borrowRegister.UpdateTime = DateTime.Now;

                    await _db.SaveChangesAsync();

                    trans.Commit();
                    response.Success = true;
                }
                catch (BizException ex)
                {
                    trans.Rollback();
                    response.Message = ex.Message;
                }
                catch
                {
                    trans.Rollback();
                    response.Message = "提交续借发生异常";
                }
            }

            return response;
        }
    }
}
