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
using archives.common;

namespace archives.service.biz.impl
{
    public class BorrowRegisterService : IBorrowRegisterService
    {
        public ArchivesContext _db;
        public BorrowRegisterService(ArchivesContext db)
        {
            _db = db;
        }

        public async Task<List<SearchBorrowRegisterResult>> QueryAllBorrowRegisters()
        {
            var query = _db.BorrowRegister.AsNoTracking().Where(c => !c.Deleted);

            var list = await query.OrderBy(c => c.Status).ThenByDescending(c => c.CreateTime)
                    .Select(c => new SearchBorrowRegisterResult
                    {
                        Id = c.Id,
                        Borrower = c.Borrower,
                        Company = c.Company,
                        Department = c.Department,
                        Phone = c.Phone,
                        ReturnDate = c.ReturnDate,
                        SignPhoto = c.SignPhoto,
                        Status = c.Status,
                        CreateTime = c.CreateTime,
                        CreateTimeStr = c.CreateTime.ToString("yyyy-MM-dd"),
                        Receiver = c.Receiver
                    }).ToListAsync();

            var ids = list.Select(c => c.Id);

            var archivesList = await _db.BorrowRegisterDetail.AsNoTracking().Where(brd => ids.Contains(brd.BorrowRegisterId)).ToListAsync();
            list.ForEach(c =>
            {
                var arlist = archivesList.Where(j => j.BorrowRegisterId == c.Id);
                //c.ArchivesList = arlist;
                c.ArchivesStr = string.Join("，", arlist.Select(j => $"{j.ArchivesNumber}/{j.FileNumber}/{j.OrderNumber}"));
                c.ReturnDateStr = c.ReturnDate.ToString("yyyy-MM-dd");
                c.ProjectName = string.Join(",", arlist.Select(j=>j.ProjectName));
            });
            return list;
        }

        /// <summary>
        /// 申报借阅
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<CommonResponse<BorrowRegisterResult>> BorrowRegister(BorrowRegisterRequest request)
        {
            var response = new CommonResponse<BorrowRegisterResult>();
            if (request == null)
            {
                response.Message = "参数不能为空";
                return response;
            }
            if (string.IsNullOrEmpty(request.Phone))
            {
                response.Message = "手机不能为空";
                return response;
            }
            if (string.IsNullOrEmpty(request.Borrower))
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
                Borrower = request.Borrower,
                Phone = request.Phone,
                ReturnDate = request.ReturnDate,
                SignPhoto = request.SignPhoto,
                Status = BorrowRegisterStatus.Registered,
                Company = request.Company,
                Department = request.Department,
                CreateTime = DateTime.Now,
                Deleted = false,
                UpdateTime = DateTime.Now,
                ReturnNotified = false
            };

            using (var trans = await _db.Database.BeginTransactionAsync())
            {
                try
                {
                    //var archives = await _db.ArchivesInfo.AsNoTracking().Where(c => request.ArchivesId.Contains(c.Id) && !c.Deleted).ToListAsync();
                    //if (archives.Count != request.ArchivesId.Count)
                    //{
                    //    throw new BizException("请求档案数目与数据库可用档案不一致，可能是数据已删除，请重新查询后再提交");
                    //}

                    //var borrowedArchives = archives.Where(c => c.Status == ArchivesStatus.Borrowed);
                    //if (borrowedArchives.Any())
                    //{
                    //    throw new BizException($"请求档案：{string.Join(",", borrowedArchives.Select(c => c.ArchivesNumber))} 当前状态为已借阅");
                    //}

                    await _db.BorrowRegister.AddAsync(regEntity);
                    await _db.SaveChangesAsync();

                    response.Data = new BorrowRegisterResult { BorrowRegisterId = regEntity.Id };

                    await _db.BorrowRegisterDetail.AddRangeAsync(request.Details.Select(c => new dal.Entity.BorrowRegisterDetail
                    {
                        ArchivesId = 0,
                        BorrowRegisterId = regEntity.Id,
                        CreateTime = DateTime.Now,
                        ArchivesNumber = c.ArchivesNumber,
                        CategoryId1 = c.CategoryId1,
                        CategoryName1 = c.CategoryName1,
                        CategoryId2 = c.CategoryId2,
                        CategoryName2 = c.CategoryName2,
                        CategoryId3 = c.CategoryId3,
                        CategoryName3 = c.CategoryName3,
                        FileNumber = c.FileNumber,
                        CategoryNumber = c.CategoryNumber,
                        OrderNumber = c.OrderNumber,
                        ProjectId = c.ProjectId,
                        ProjectName = c.ProjectName,
                        Title = c.Title
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
                    ApplicationLog.Error("BorrowRegister", ex);
                }
            }

            return response;
        }

        public async Task<CommonSearchResponse<List<SearchBorrowRegisterResult>>> SearchBorrowRegister(SearchBorrowRegisterRequest request)
        {
            var response = new CommonSearchResponse<List<SearchBorrowRegisterResult>>();
            try
            {
                var query = _db.BorrowRegister.AsNoTracking().Where(c => !c.Deleted);
                if (!string.IsNullOrEmpty(request.Keyword))
                {
                    query = query.Where(c => c.Phone.Contains(request.Keyword.Trim()) || c.Borrower.Contains(request.Keyword.Trim()) || c.Company.Contains(request.Keyword.Trim()) || c.Department.Contains(request.Keyword.Trim()));
                }
                if (request.StartDate.HasValue)
                {
                    query = query.Where(c => c.CreateTime > request.StartDate.Value);
                }
                if (request.EndDate.HasValue)
                {
                    query = query.Where(c => c.CreateTime < request.EndDate.Value.AddDays(1));
                }
                var list = await query.OrderBy(c => c.Status).ThenBy(c => c.Id)
                        .Skip(request.PageNumber * request.PageSize)
                        .Take(request.PageSize)
                        .Select(c => new SearchBorrowRegisterResult
                        {
                            Id = c.Id,
                            Borrower = c.Borrower,
                            Company = c.Company,
                            Department = c.Department,
                            Phone = c.Phone,
                            ReturnDate = c.ReturnDate,
                            SignPhoto = c.SignPhoto,
                            Status = c.Status,
                            CreateTime = c.CreateTime,
                            CreateTimeStr = c.CreateTime.ToString("yyyy-MM-dd"),
                            Receiver = c.Receiver,
                        }).ToListAsync();

                var ids = list.Select(c => c.Id);

                var archivesList = await _db.BorrowRegisterDetail.AsNoTracking().Where(brd => ids.Contains(brd.BorrowRegisterId)).ToListAsync();

                list.ForEach(c =>
                {
                    var arlist = archivesList.Where(j => j.BorrowRegisterId == c.Id);
                    c.ArchivesStr = string.Join("，", arlist.Select(j => $"{j.ArchivesNumber}/{j.FileNumber}/{j.OrderNumber}"));
                    c.ReturnDateStr = c.ReturnDate.ToString("yyyy-MM-dd");
                    c.ProjectName = string.Join("，", arlist.Select(j => j.ProjectName));
                });

                var total = await query.CountAsync();

                response.Data = list;
                response.TotalPage = total.GetPages(request.PageSize);
                response.TotalCount = total;
                response.Success = true;

                //response.Data = list;
                //response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = "获取借阅列表失败";
                ApplicationLog.Error("SearchBorrowRegister", ex);
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
                if (request == null)
                    throw new BizException("参数不能为空");
                var borrowRegister = await _db.BorrowRegister.AsNoTracking().FirstOrDefaultAsync(c => c.Id == request.BorrowRegisterId && !c.Deleted);
                if (borrowRegister == null)
                    throw new BizException("借阅登记不存在");

                var details = await _db.BorrowRegisterDetail.AsNoTracking().Where(c => c.BorrowRegisterId == borrowRegister.Id).ToListAsync();

                response.Data = new GetBorrowDetailResult
                {
                    BorrowRegister = new BorrowRegisterSimple
                    {
                        Id = borrowRegister.Id,
                        Borrower = borrowRegister.Borrower,
                        Company = borrowRegister.Company,
                        Phone = borrowRegister.Phone,
                        ReturnDate = borrowRegister.ReturnDate,
                        ReturnDateStr = borrowRegister.ReturnDate.ToString("yyyy-MM-dd"),
                        Department = borrowRegister.Department,
                        SignPhoto = borrowRegister.SignPhoto,
                        CreateTime = borrowRegister.CreateTime,
                        CreateTimeStr = borrowRegister.CreateTime.ToString("yyyy-MM-dd"),
                        Status = borrowRegister.Status
                    },
                    ArchivesList = details.Select(c => new BorrowRegisterDetailSimple
                    {
                        Id = c.Id,
                        ArchivesNumber = c.ArchivesNumber,
                        CategoryId1 = c.CategoryId1 ?? 0,
                        CategoryId2 = c.CategoryId2 ?? 0,
                        CategoryId3 = c.CategoryId3 ?? 0,
                        CategoryName1 = c.CategoryName1,
                        CategoryName2 = c.CategoryName2,
                        CategoryName3 = c.CategoryName3,
                        CategoryNumber = c.CategoryNumber,
                        FileNumber = c.FileNumber,
                        OrderNumber = c.OrderNumber,
                        ProjectId = c.ProjectId ?? 0,
                        ProjectName = c.ProjectName,
                        Status = ArchivesStatus.Normal,
                        Title = c.Title
                    }).ToList(),
                };
                response.Success = true;
            }
            catch (BizException ex)
            {
                response.Message = ex.Message;
            }
            catch (Exception ex)
            {
                response.Message = "获取借阅详情发生异常";
                ApplicationLog.Error("GetBorrowDetail", ex);
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
                if (request == null)
                    throw new BizException("参数不能为空");

                if (request.RenewDate < DateTime.Now)
                    throw new BizException("续借日期不能小于当天");

                var borrowRegister = await _db.BorrowRegister.FirstOrDefaultAsync(c => c.Id == request.BorrowRegisterId && !c.Deleted);
                if (borrowRegister == null)
                    throw new BizException("借阅登记不存在");

                if (!(borrowRegister.Status == BorrowRegisterStatus.Renewed || borrowRegister.Status == BorrowRegisterStatus.Overdue || borrowRegister.Status == BorrowRegisterStatus.Borrowed))
                    throw new BizException("借阅登记状态为：已借出、延期、逾期 才能续借");

                borrowRegister.ReturnDate = request.RenewDate;
                borrowRegister.Status = BorrowRegisterStatus.Renewed;
                borrowRegister.UpdateTime = DateTime.Now;
                borrowRegister.ReturnNotified = false;
                await _db.SaveChangesAsync();
                response.Success = true;

                var projects = await _db.BorrowRegisterDetail.AsNoTracking().FirstOrDefaultAsync(c => c.BorrowRegisterId == borrowRegister.Id);
                try
                {
                    //var archivesFirst = await _db.ArchivesInfo.AsNoTracking().Join(_db.BorrowRegisterDetail.AsNoTracking().Where(j => j.BorrowRegisterId == borrowRegister.Id).Take(1),
                    //    a => a.Id, b => b.ArchivesId, (a, b) => new { a, b })
                    //    .Select(c => new
                    //    {
                    //        c.a.ProjectName
                    //    }).FirstOrDefaultAsync();
                    ApplicationLog.Info("SMS_171116665." + borrowRegister.Phone + "." + borrowRegister.Id);
                    var msgRes = OssHelper.SendSms("SMS_171116665", borrowRegister.Phone, $"{{\"name\":\"{borrowRegister.Borrower}\", \"PtName\":\"{(projects?.ProjectName)}\", \"RDate\":\"{borrowRegister.ReturnDate.ToString("yyyy-MM-dd")}\" }}");

                }
                catch (Exception ex1)
                {
                    ApplicationLog.Error("RenewBorrow notice exception", ex1);
                }

            }
            catch (BizException ex)
            {
                response.Message = ex.Message;
            }
            catch (Exception ex)
            {
                response.Message = "提交续借发生异常";
                ApplicationLog.Error("RenewBorrow", ex);
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
                if (request == null)
                    throw new BizException("参数不能为空");

                var borrowRegister = await _db.BorrowRegister.FirstOrDefaultAsync(c => c.Id == request.BorrowRegisterId && !c.Deleted);
                if (borrowRegister == null)
                    throw new BizException("借阅登记不存在");

                if (borrowRegister.Status == BorrowRegisterStatus.Returned)
                {
                    response.Message = "当前状态已经归还";
                    response.ErrorCode = 1;
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
                borrowRegister.Receiver = request.Receiver;
                //var archivesList = await _db.ArchivesInfo.Join(_db.BorrowRegisterDetail, a => a.Id, b => b.ArchivesId, (a, b) => new { a, b }).Where(j => j.b.BorrowRegisterId == borrowRegister.Id).Select(c => c.a).ToListAsync();
                //archivesList.ForEach(a => a.Status = ArchivesStatus.Normal);
                await _db.SaveChangesAsync();
                response.Success = true;
            }
            catch (BizException ex)
            {
                response.Message = ex.Message;
            }
            catch (Exception ex)
            {
                response.Message = "提交续借发生异常";
                ApplicationLog.Error("ReturnArchives", ex);
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
                    if (request == null)
                        throw new BizException("参数不能为空");

                    var borrowRegister = await _db.BorrowRegister.FirstOrDefaultAsync(c => c.Id == request.BorrowRegisterId && !c.Deleted);
                    if (borrowRegister == null)
                        throw new BizException("借阅登记不存在");

                    if (borrowRegister.Status == BorrowRegisterStatus.Borrowed)
                    {
                        //response.Message = "当前状态已经借出";
                        //response.ErrorCode = 1;
                        //response.Success = true;
                        //return response;
                        throw new BizException("当前借阅的状态为已借出");
                    }

                    if (borrowRegister.Status != BorrowRegisterStatus.Registered)
                    {
                        throw new BizException("借阅登记状态为：已登记 才能确认借出");
                    }
                    /*
                    var archives = await _db.ArchivesInfo.Join(_db.BorrowRegisterDetail, a => a.Id, b => b.ArchivesId, (a, b) => new { a, b })
                        .Where(j => j.b.BorrowRegisterId == borrowRegister.Id)
                        .Select(c => c.a).ToListAsync();
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
                                throw new BizException("您选择的档案可能已借出，无法再次借阅");
                            default:
                                throw new BizException("您选择的档案档案当前状态出错，无法确认借出");
                        }
                    });
                    */
                    borrowRegister.UpdateTime = DateTime.Now;
                    borrowRegister.Status = BorrowRegisterStatus.Borrowed;
                    borrowRegister.UpdateTime = DateTime.Now;

                    await _db.SaveChangesAsync();

                    trans.Commit();
                    response.Success = true;

                    var projects = await _db.BorrowRegisterDetail.AsNoTracking().FirstOrDefaultAsync(c => c.BorrowRegisterId == borrowRegister.Id);

                    ApplicationLog.Info("SMS_171116670." + borrowRegister.Phone + "." + borrowRegister.Id);
                    var msgRes = OssHelper.SendSms("SMS_171116670", borrowRegister.Phone, $"{{\"name\":\"{borrowRegister.Borrower}\", \"PtName\":\"{(projects?.ProjectName)}\", \"RDate\":\"{borrowRegister.ReturnDate.ToString("yyyy-MM-dd")}\" }}");

                }
                catch (BizException ex)
                {
                    trans.Rollback();
                    response.Message = ex.Message;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    response.Message = "提交续借发生异常";
                    ApplicationLog.Error("ConfirmBorrowed", ex);
                }
            }

            return response;
        }

        public async Task<CommonResponse<string>> ReturnWarn(ReturnWarnRequest request)
        {
            var response = new CommonResponse<string>();

            try
            {
                if (request == null)
                    throw new BizException("参数不能为空");

                var borrowRegister = await _db.BorrowRegister.FirstAsync(c => c.Id == request.BorrowRegisterId);
                if (borrowRegister == null)
                {
                    throw new BizException("借阅记录不存在");
                }

                if (!(borrowRegister.Status == BorrowRegisterStatus.Borrowed || borrowRegister.Status == BorrowRegisterStatus.Overdue || borrowRegister.Status == BorrowRegisterStatus.Renewed))
                {
                    throw new BizException("借阅登记状态为：已借出、延期、逾期 才能催还");
                }

                //var archives = await _db.ArchivesInfo.Join(_db.BorrowRegisterDetail.Where(j => j.BorrowRegisterId == borrowRegister.Id).Take(1), a => a.Id, b => b.ArchivesId, (a, b) => new { a, b })
                //    .Select(c => new
                //    {
                //        c.a.ProjectName,
                //        c.b.Id
                //    }).FirstOrDefaultAsync();
                var projects = await _db.BorrowRegisterDetail.AsNoTracking().FirstOrDefaultAsync(c => c.BorrowRegisterId == borrowRegister.Id);

                ApplicationLog.Info("SMS_171116662." + borrowRegister.Phone + "." + borrowRegister.Id);
                var msgRes = OssHelper.SendSms("SMS_171116662", borrowRegister.Phone, $"{{\"name\":\"{borrowRegister.Borrower}\", \"PtName\":\"{(projects?.ProjectName)}\", \"RDate\":\"{borrowRegister.ReturnDate.ToString("yyyy-MM-dd")}\" }}");

                if (msgRes.Code == "OK")
                {
                    borrowRegister.ReturnNotified = true;
                    borrowRegister.NotifyCount = borrowRegister.NotifyCount.GetValueOrDefault() + 1;
                    borrowRegister.UpdateTime = DateTime.Now;
                    await _db.SaveChangesAsync();
                    response.Data = borrowRegister.NotifyCount.ToString();
                }
                response.Success = true;
            }
            catch (BizException ex)
            {
                response.Message = ex.Message;
            }
            catch (Exception ex)
            {
                response.Message = "催还发生异常";
                ApplicationLog.Error("ReturnWarn", ex);
            }

            return response;
        }

        public async Task<CommonResponse<string>> BorrowRegisterNotify(int dayLimit)
        {
            var response = new CommonResponse<string>();
            try
            {
                var list = await _db.BorrowRegister.Where(c => !c.Deleted && c.ReturnDate <= DateTime.Now.AddDays(dayLimit) && (!c.ReturnNotified.HasValue || c.ReturnNotified.Value == false)
                && (c.Status == BorrowRegisterStatus.Borrowed || c.Status == BorrowRegisterStatus.Overdue || c.Status == BorrowRegisterStatus.Renewed))
                .OrderBy(c => c.Id).Take(50).ToListAsync();

                //var archivesList = await _db.ArchivesInfo.Join(_db.BorrowRegisterDetail, a => a.Id, b => b.ArchivesId, (a, b) => new { a, b })
                //    .Where(j => list.Select(l => l.Id).Contains(j.b.BorrowRegisterId))
                //    .Select(c => new
                //    {
                //        c.a.ProjectName,
                //        c.b.Id,
                //        c.b.BorrowRegisterId
                //    }).ToListAsync();

                var ids = list.Select(c => c.Id);
                var projects = await _db.BorrowRegisterDetail.AsNoTracking().Where(c => ids.Contains(c.BorrowRegisterId)).Select(c => new { c.ProjectName, c.BorrowRegisterId }).ToListAsync();

                if (list.Any())
                {
                    list.ForEach(c =>
                    {
                        var project = projects.FirstOrDefault(a => a.BorrowRegisterId == c.Id);

                        ApplicationLog.Info("task.SMS_171116662." + c.Phone + "." + c.Id);
                        var msgRes = OssHelper.SendSms("SMS_171116662", c.Phone, $"{{\"name\":\"{c.Borrower}\", \"PtName\":\"{(project?.ProjectName)}\", \"RDate\":\"{c.ReturnDate.ToString("yyyy-MM-dd")}\" }}");
                        //循环发送短信
                        if (msgRes.Code == "OK")
                        {
                            c.ReturnNotified = true;
                            c.NotifyCount = c.NotifyCount.GetValueOrDefault() + 1;
                            c.UpdateTime = DateTime.Now;
                        }

                    });
                    var data = list.Select(c => c.Id).Serialize();
                    await _db.OperationLog.AddAsync(new OperationLog
                    {
                        Action = OperationAction.Create,
                        Name = "催还短信",
                        CreateTime = DateTime.Now,
                        BeforeData = data
                    });
                    await _db.SaveChangesAsync();
                    response.Data = data;
                }
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                ApplicationLog.Error("BorrowRegisterNotify", ex);
            }

            return response;
        }

        public async Task<CommonResponse<string>> CloseBorrow(CloseBorrowRequest request)
        {
            var response = new CommonResponse<string>();
            try
            {
                if (request == null)
                    throw new BizException("参数不能为空");

                var borrowRegister = await _db.BorrowRegister.FirstOrDefaultAsync(c => c.Id == request.BorrowRegisterId && !c.Deleted);
                if (borrowRegister == null)
                    throw new BizException("借阅登记不存在");

                if (borrowRegister.Status != BorrowRegisterStatus.Registered)
                    throw new BizException("借阅登记状态为：已登记 才能关闭");

                borrowRegister.Status = BorrowRegisterStatus.Closed;
                borrowRegister.UpdateTime = DateTime.Now;
                await _db.SaveChangesAsync();
                response.Success = true;

            }
            catch (BizException ex)
            {
                response.Message = ex.Message;
            }
            catch (Exception ex)
            {
                response.Message = "关闭借阅发生异常";
                ApplicationLog.Error("RenewBorrow", ex);
            }
            return response;
        }
    }
}
