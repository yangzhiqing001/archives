using System;
using System.Threading.Tasks;
using archives.service.biz.ifs;
using archives.service.dal;
using archives.service.dal.Entity;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using archives.service.biz.exp;
using System.Collections.Generic;
using System.IO;
using NPOI.SS.UserModel;
using archives.service.biz.web;
using archives.common;
using MySql.Data.MySqlClient;

namespace archives.service.biz.impl
{
    public class FileStorageService : IFileStorageService
    {
        public ArchivesContext _db;
        public FileStorageService(ArchivesContext db)
        {
            _db = db;
        }

        public async Task<string> AddFile(FileStorage fileStorage)
        {
            await _db.FileStorage.AddAsync(fileStorage);
            await _db.SaveChangesAsync();
            return fileStorage.Id;
        }

        public async Task<List<string>> AddRangeFile(List<FileStorage> fileStorageList)
        {
            await _db.FileStorage.AddRangeAsync(fileStorageList);
            await _db.SaveChangesAsync();
            return fileStorageList.Select(c => c.Id).ToList();
        }

        public async Task<CommonResponse<ConfirmUploadResult>> ConfirmUpload(List<string> ids)
        {
            var response = new CommonResponse<ConfirmUploadResult>
            {
                Data = new ConfirmUploadResult
                {
                    ErrorList = new List<string>()
                },
            };
            try
            {
                var list = await this.GetList(ids);
                if (!list.Any())
                    throw new BizException("没有获取到上传的文件");

                var trans = await _db.Database.BeginTransactionAsync();
                var archivesList = new List<ArchivesInfo>();
                try
                {
                    foreach (var item in list)
                    {
                        using (var fs = new FileStream(item.StoragePath, FileMode.Open, FileAccess.Read))
                        {
                            IWorkbook workbook = WorkbookFactory.Create(fs);
                            ISheet sheet = workbook.GetSheet("卷盒内文件"); //workbook.GetSheetAt(0);
                            if (sheet == null)
                            {
                                response.Data.ErrorList.Add($"Excel不存在sheet:卷盒内文件");
                                continue;
                            }
                            
                            for (int j = 1; j <= sheet.LastRowNum; ++j)
                            {
                                IRow row = sheet.GetRow(j);
                                if (row == null) continue; //没有数据的行默认是null　　　　　　　

                                archivesList.Add(new ArchivesInfo
                                {
                                    ArchivesNumber = row.GetCell(0).StringCellValue,
                                    CategoryId = row.GetCell(1).StringCellValue,
                                    FileNumber = row.GetCell(2).StringCellValue,
                                    OrderNumber = row.GetCell(3).StringCellValue,
                                    Title = row.GetCell(4).StringCellValue,
                                    ProjectName = row.GetCell(5).StringCellValue,
                                    ResponsibleObject = row.GetCell(6).StringCellValue,
                                    WrittenDate = row.GetCell(7).StringCellValue,
                                    Pages = row.GetCell(8).StringCellValue,
                                    IsPermanent = row.GetCell(9).StringCellValue,
                                    SecretLevel = row.GetCell(10).StringCellValue,
                                    ArchivingDepartment = row.GetCell(11).StringCellValue,
                                    ArchivingDate = row.GetCell(12).StringCellValue,
                                    Remark = row.GetCell(13).StringCellValue,
                                    CatalogNumber = row.GetCell(14).StringCellValue,
                                    Summary = row.GetCell(15).StringCellValue,
                                    CreateTime = DateTime.Now,
                                    Status = ArchivesStatus.Init,
                                    UpdateTime = DateTime.Now,
                                    Deleted = false,
                                });
                            }                                                        
                        }
                    }
                    
                    if (response.Data.ErrorList.Any())
                    {
                        trans.Rollback();
                        response.Message = string.Join("<br />", response.Data.ErrorList);
                    }
                    else
                    {
                        _db.ChangeTracker.AutoDetectChangesEnabled = false;
                        await _db.ArchivesInfo.AddRangeAsync(archivesList);
                        response.Data.AddTotoal = await _db.SaveChangesAsync();
                        trans.Commit();
                        response.Success = true;
                        response.Message = $"成功添加{response.Data.AddTotoal}条数据";
                    }
                }
                catch (DbUpdateException ex)
                {
                    response.Message = $"导入发生数据异常";
                    trans.Rollback();
                    if (ex.InnerException!= null && ex.InnerException is MySqlException)
                    {
                        var exp = (MySqlException)ex.InnerException;
                        if (exp.Number == 1062)
                        {
                            response.Message = "导入发现重复数据";
                            for (int i = 0; i < archivesList.Count; i++)
                            {
                                var item = archivesList[i];
                                var cc = await _db.ArchivesInfo.CountAsync(c => c.ArchivesNumber == item.ArchivesNumber
                                    && c.CatalogNumber == item.CatalogNumber
                                    && c.FileNumber == item.FileNumber
                                    && c.OrderNumber == item.OrderNumber);
                                if (cc > 0)
                                {
                                    response.Data.ErrorList.Add($"档号：{item.ArchivesNumber}，分类号：{item.CatalogNumber}，案卷号：{item.FileNumber}卷内序号：{item.OrderNumber} 已存在;");
                                    if (response.Data.ErrorList.Count > archivesList.Count / 3 || response.Data.ErrorList.Count > 50)
                                    {
                                        response.Message = "发现大量重复数据";
                                        break;
                                    }
                                }
                            }
                            if (response.Data.ErrorList.Count() == 0 && ex.InnerException!=null && ex.InnerException.Message !=null) {
                                response.Data.ErrorList.Add("可能您上传的文件内有重数数据，请参考异常信息："+ex.InnerException.Message);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    response.Message = $"导入发生异常";
                    trans.Rollback();
                    ApplicationLog.Error("ConfirmUpload", ex);
                }
            }
            catch(BizException ex)
            {
                response.Message = ex.Message;
            }
            catch(Exception ex)
            {
                ApplicationLog.Error("ConfirmUpload", ex);
                response.Message = "导入发生异常";
            }
            
            return response;
        }

        public async Task<FileStorage> Get(string id)
        {
            var entity = await _db.FileStorage.FirstOrDefaultAsync(c => c.Id == id);
            if (entity == null)
                throw new BizException("文件不存在");
            return entity;
        }

        public async Task<List<FileStorage>> GetList(List<string> ids)
        {
            var list = await _db.FileStorage.Where(c => ids.Contains(c.Id)).ToListAsync();
            return list;
        }
    }
}

