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

namespace archives.service.biz.impl
{
    public class ArchivesInfoService : IArchivesInfoService
    {
        public ArchivesContext _db;
        public ArchivesInfoService(ArchivesContext db)
        {
            _db = db;
        }

        public async Task<CommonSearchResponse<List<ArchivesSearchResult>>> SearchArchives(ArchivesSearchRequest request)
        {
            var response = new CommonSearchResponse<List<ArchivesSearchResult>>();
            try
            {
                var query = _db.ArchivesInfo.Where(c => !c.Deleted);
                if (!string.IsNullOrEmpty(request.Keyword))
                {
                    query = query.Where(c => c.Title.Contains(request.Keyword.Trim()));
                }
                var list = await query.Skip(request.PageNumber * request.PageSize)
                    .Take(request.PageSize)
                    .OrderBy(c => c.ArchivesNumber)
                    .Select(c => new ArchivesSearchResult
                    {
                        Id = c.Id,
                        ArchivesNumber = c.ArchivesNumber,
                        CategoryId = c.CategoryId,
                        FileNumber = c.FileNumber,
                        ProjectName = c.ProjectName,
                        Title = c.Title
                    }).ToListAsync();

                var total = await query.CountAsync();

                response.Data = list;
                response.TotalPage = total.GetPages(request.PageSize);
                response.TotalCount = total;
                response.Success = true;
            }
            catch
            {
                response.Message = "获取案档列表发生异常";
            }

            return response;
        }

        public async Task<CommonResponse<ArchivesInfo>> GetArchives(int id)
        {
            var response = new CommonResponse<ArchivesInfo>();
            try
            {
                var archives = await _db.ArchivesInfo.FirstOrDefaultAsync(c => c.Id == id && !c.Deleted);
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
                //写日志可以用log4netcore
            }
            return response;
        }
    }
}
