using System;
using System.Collections.Generic;
using System.Linq;
using archives.service.biz.ifs;
using archives.service.dal;
using archives.service.dal.Entity;
using archives.service.biz.web;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using archives.service.biz.exp;
using archives.common;

namespace archives.service.biz.impl
{
    public class ArchivesInfoService : IArchivesInfoService
    {
        public ArchivesContext _db;
        public ArchivesInfoService(ArchivesContext db)
        {
            _db = db;
        }

        public async Task<List<ArchivesInfo>> QueryAllArchives()
        {
            return await _db.ArchivesInfo.ToListAsync();
        }

        public async Task<List<ArchivesInfo>> QueryExportArchives(ArchivesSearchRequest request)
        {

            var query = _db.ArchivesInfo.AsNoTracking();
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(c => c.Title.Contains(request.Keyword.Trim()) || c.ProjectName.Contains(request.Keyword.Trim()));
            }
            if (!string.IsNullOrEmpty(request.Label))
            {
                var projectNames = request.Label.Split(',', StringSplitOptions.RemoveEmptyEntries);
                query = query.Where(c => projectNames.Contains(c.ProjectName));
            }

            return await query.OrderBy(c => c.ArchivesNumber).ToListAsync();


            //return await _db.ArchivesInfo.ToListAsync();
        }

        public async Task<CommonSearchResponse<List<ArchivesSearchResult>>> SearchArchives(ArchivesSearchRequest request)
        {
            var response = new CommonSearchResponse<List<ArchivesSearchResult>>();
            try
            {
                var query = _db.ArchivesInfo.AsNoTracking().Where(c => !c.Deleted);
                if (!string.IsNullOrEmpty(request.Keyword))
                {
                    query = query.Where(c => c.Title.Contains(request.Keyword.Trim()) || c.ProjectName.Contains(request.Keyword.Trim()));
                }
                if (!string.IsNullOrEmpty(request.Label))
                {
                    var projectNames = request.Label.Split(',', StringSplitOptions.RemoveEmptyEntries);
                    query = query.Where(c => projectNames.Contains(c.ProjectName));
                }
                if (request.ShowBorrowed.HasValue)
                {
                    if (!request.ShowBorrowed.Value)
                    {
                        query = query.Where(c => c.Status != ArchivesStatus.Borrowed);
                    }

                }
                var list = await query.OrderBy(c => c.ArchivesNumber)
                    .Skip(request.PageNumber * request.PageSize)
                    .Take(request.PageSize)                    
                    .Select(c => new ArchivesSearchResult
                    {
                        Id = c.Id,
                        ArchivesNumber = c.ArchivesNumber,
                        CategoryId = c.CategoryId,
                        FileNumber = c.FileNumber,
                        ProjectName = c.ProjectName,
                        Title = c.Title,
                        OrderNumber = c.OrderNumber,
                        Status = c.Status,
                    }).ToListAsync();

                var total = await query.CountAsync();

                response.Data = list;
                response.TotalPage = total.GetPages(request.PageSize);
                response.TotalCount = total;
                response.Success = true;
                response.SEcho = request.SEcho;
            }
            catch(Exception ex)
            {
                response.Message = "获取案档列表发生异常";
                ApplicationLog.Error("SearchArchives", ex);
            }

            return response;
        }

        public async Task<CommonResponse<ArchivesInfo>> GetArchives(int id)
        {
            var response = new CommonResponse<ArchivesInfo>();
            try
            {
                var archives = await _db.ArchivesInfo.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id && !c.Deleted);
                if (archives == null)
                    throw new BizException("档案不存在或已删除");
                //var archivesDetails = await _db.ArchivesDetails.Where(c => c.ArchivesId == archivesId && !c.Deleted).ToListAsync();
                response.Data = archives;
                response.Success = true;
            }
            catch (BizException ex)
            {
                response.Message = ex.Message;
            }
            catch (Exception ex)
            {
                response.Message = "获取档案详情失败";
                ApplicationLog.Error("GetArchives", ex);
                //写日志可以用log4netcore
            }
            return response;
        }

        public async Task<List<string>> QueryAllProject()
        {
            var list = await _db.ArchivesInfo.AsNoTracking().Where(c => !c.Deleted && c.ProjectName != null && c.ProjectName != string.Empty).OrderByDescending(c => c.Id).Select(c => c.ProjectName).Distinct().ToListAsync();
            return list;
        }

        public async Task<CommonResponse<ArchivesEditResult>> Edit(ArchivesEditRequest request)
        {
            var response = new CommonResponse<ArchivesEditResult>();
            try
            {
                if (request == null)
                    throw new BizException("参数不能为空");

                var entity = await _db.ArchivesInfo.FirstOrDefaultAsync(c => c.Id == request.Id);
                if (entity == null)
                    throw new BizException("档案不存在");

                if (entity.Deleted)
                    throw new BizException("档案已删除，无法修改");//此条件需要放在where条件中么？

                //if (entity.Status != ArchivesStatus.Init)
                //    throw new BizException("档案已借阅过，无法再编辑");

                //entity.ArchivesNumber = request.ArchivesNumber;
                entity.UpdateTime = DateTime.Now;
                entity.Title = request.Title;
                entity.WrittenDate = request.WrittenDate;
                entity.Summary = request.Summary;
                entity.SecretLevel = request.SecretLevel;
                entity.ResponsibleObject = request.ResponsibleObject;
                entity.Remark = request.Remark;
                entity.ProjectName = request.ProjectName;
                entity.Pages = request.Pages;
                //entity.OrderNumber = request.OrderNumber;
                entity.IsPermanent = request.IsPermanent;
                //entity.FileNumber = request.FileNumber;
                //entity.CategoryId = request.CategoryId;
                entity.CatalogNumber = request.CatalogNumber;
                entity.ArchivingDepartment = request.ArchivingDepartment;
                entity.ArchivingDate = request.ArchivingDate;

                await _db.SaveChangesAsync();

                response.Data = new ArchivesEditResult();
                response.Success = true;
            }
            catch (BizException ex)
            {
                response.Message = ex.Message;
            }
            catch(Exception ex)
            {
                response.Message = "操作发生异常";
                ApplicationLog.Error("Edit", ex);
            }
            return response;
        }

        public async Task<CommonResponse<ArchivesAddResult>> Add(ArchivesAddRequest request)
        {
            var response = new CommonResponse<ArchivesAddResult>();
            try
            {
                if (request == null)
                    throw new BizException("参数不能为空");
                //var exists = await _db.ArchivesInfo.AnyAsync(c => c.ArchivesNumber == request.ArchivesNumber && c.CatalogNumber == request.CatalogNumber && c.FileNumber == request.FileNumber && c.CategoryId == request.CategoryId);
                //if (exists)
                //    throw new BizException("当前提交的档号、目录号、分类号、案卷号在数据库中已存在，无法重复添加，请查询是否已添加");

                var entity = new ArchivesInfo
                {
                    ArchivesNumber = request.ArchivesNumber,
                    Deleted = false,
                    FileNumber = request.FileNumber,
                    IsPermanent = request.IsPermanent,
                    OrderNumber = request.OrderNumber,
                    Pages = request.Pages,
                    CategoryId = request.CategoryId,
                    ArchivingDate = request.ArchivingDate,
                    ArchivingDepartment = request.ArchivingDepartment,
                    CatalogNumber = request.CatalogNumber,
                    CreateTime = DateTime.Now,
                    ProjectName = request.ProjectName,
                    Remark = request.Remark,
                    ResponsibleObject = request.ResponsibleObject,
                    SecretLevel = request.SecretLevel,
                    Status = ArchivesStatus.Init,
                    Summary = request.Summary,
                    Title = request.Title,
                    UpdateTime = DateTime.Now,
                    WrittenDate = request.WrittenDate
                };
                await _db.ArchivesInfo.AddAsync(entity);
                await _db.SaveChangesAsync();
                response.Data = new ArchivesAddResult
                {
                    Id = entity.Id
                };
                response.Success = true;
            }
            catch (BizException ex)
            {
                response.Message = ex.Message;

            }
            catch(Exception ex)
            {
                response.Message = "添加档案发生异常";
                ApplicationLog.Error("Add", ex);
            }
            return response;

        }

        public async Task<CommonResponse<ArchivesDeleteResult>> Delete(ArchivesDeleteRequest request)
        {
            var response = new CommonResponse<ArchivesDeleteResult>();
            try
            {
                if (request == null)
                    throw new BizException("参数不能为空");
                var archives = await _db.ArchivesInfo.FirstOrDefaultAsync(c => c.Id == request.ArchivesId);
                if (archives == null)
                {
                    throw new BizException("档案不存在，无法删除");
                }
                if (archives.Status != ArchivesStatus.Init)
                {
                    throw new BizException("档案已借阅过，无法删除");
                }
                _db.ArchivesInfo.Remove(archives);
                await _db.SaveChangesAsync();
                //if (!archives.Deleted)
                //{
                //    archives.Deleted = true;
                //    await _db.SaveChangesAsync();
                //}
                response.Data = new ArchivesDeleteResult();
                response.Success = true;
            }
            catch(BizException ex)
            {
                response.Message = ex.Message;
            }
            catch(Exception ex)
            {
                response.Message = "删除发生异常";
                ApplicationLog.Error("Delete", ex);
            }
            return response;

        }

        public async Task<CommonResponse<string>> ChangPassword(ChangePsdRequest request) {
            var response = new CommonResponse<string>();
            try
            {
                if (request == null)
                    throw new BizException("参数不能为空");
                var entity = await _db.AdminUser.FirstOrDefaultAsync(c => c.UserName == request.UserName);
                if (entity.Password != Md5.MD5Hash(request.OldPassword).ToLower())
                    throw new BizException("原密码错误");

                if (entity == null)
                    throw new BizException("用户不存在");

                entity.Password = Md5.MD5Hash(request.NewPassword).ToLower();

                await _db.SaveChangesAsync();

                response.Data = "ok";
                response.Success = true;
            }
            catch (BizException ex)
            {
                response.Message = ex.Message;

            }
            catch (Exception ex)
            {
                response.Message = "修改密码发生异常";
                ApplicationLog.Error("ChangPassword", ex);
            }
            return response;
        }

        public async Task<List<string>> QueryProject(string name)
        {
            if (!string.IsNullOrEmpty(name))
                return await _db.Project.Where(c => c.ProjectName.Contains(name.Trim())).OrderBy(c => c.ProjectName).Take(20).Select(c=>c.ProjectName).ToListAsync();
            return new List<string>();
        }

        public async Task<List<CategoryResult>> QueryAllCategory()
        {
            var categorys = await _db.Category.ToListAsync();
            return categorys.Where(c => c.Level == 1).Select(c => new CategoryResult {
                Id = c.Id,
                CategoryName = c.CategoryName,
                Level = c.Level,
                Children = categorys.Where(c2=>c2.ParentId == c.Id).Select(c2=>new CategoryResult {
                    Id = c2.Id,
                    CategoryName = c2.CategoryName,
                    Level = c2.Level,
                    Children = categorys.Where(c3=>c3.ParentId == c2.Id).Select(c3=>new CategoryResult {
                        Id = c3.Id,
                        CategoryName = c3.CategoryName,
                        Level = c3.Level
                    }).ToList()
                }).ToList()
            }).ToList();
        }
    }
}
