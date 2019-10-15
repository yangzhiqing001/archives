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
            int jumpcount = 0;
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

                            IRow row0 = sheet.GetRow(0);
                            if (row0.GetCell(0).StringCellValue.Trim() == "档号" && row0.GetCell(1).StringCellValue.Trim() == "分类号" && row0.GetCell(2).StringCellValue.Trim() == "案卷号" && row0.GetCell(3).StringCellValue.Trim() == "卷内序号" && row0.GetCell(4).StringCellValue.Trim() == "题名" && row0.GetCell(5).StringCellValue.Trim() == "项目名称" && row0.GetCell(6).StringCellValue.Trim() == "责任者" && row0.GetCell(7).StringCellValue.Trim() == "成文日期" && row0.GetCell(8).StringCellValue.Trim() == "页数" && row0.GetCell(9).StringCellValue.Trim() == "保管期限" && row0.GetCell(10).StringCellValue.Trim() == "密级" && row0.GetCell(11).StringCellValue.Trim() == "归档部门" && row0.GetCell(12).StringCellValue.Trim() == "归档日期" && row0.GetCell(13).StringCellValue.Trim() == "备注" && row0.GetCell(14).StringCellValue.Trim() == "目录号" && row0.GetCell(15).StringCellValue.Trim() == "提要")
                            {

                            }
                            else {
                                response.Data.ErrorList.Add($"Excel标题栏与预期不匹配，请确认顺序，应为：档号，分类号，案卷号，卷内序号，题名，项目名称，责任者，成文日期，页数，保管期限，密级，归档部门，归档日期，备注，目录号，提要");
                                continue;
                            }
                                                        
                            for (int j = 1; j <= sheet.LastRowNum; ++j)
                            {
                                IRow row = sheet.GetRow(j);
                                if (row == null) continue; //没有数据的行默认是null　　　　　　　

                                var cc = archivesList.Count(c => c.ArchivesNumber == row.GetCell(0).StringCellValue
                                    && c.CategoryId == row.GetCell(1).StringCellValue
                                    && c.FileNumber == row.GetCell(2).StringCellValue
                                    && c.OrderNumber == row.GetCell(3).StringCellValue);
                                if (cc > 0)
                                {
                                    response.Data.ErrorList.Add($"{item.OriginalFileName}中档号：{row.GetCell(0).StringCellValue}，分类号：{row.GetCell(1).StringCellValue}，案卷号：{row.GetCell(2).StringCellValue}卷内序号：{row.GetCell(3).StringCellValue} 重复;<br/>");
                                    if (response.Data.ErrorList.Count > 50)
                                    {
                                        response.Message = "发现大量重复数据";
                                        break;
                                    }
                                }
                                
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


                                var archivesNumber = row.GetCell(0).StringCellValue;
                                var categoryId = row.GetCell(1).StringCellValue;
                                var fileNumber = row.GetCell(2).StringCellValue;
                                var orderNumber = row.GetCell(3).StringCellValue;

                                var archives = await _db.ArchivesInfo.FirstOrDefaultAsync
                                    (c => c.ArchivesNumber == archivesNumber
                                        && c.CategoryId == categoryId
                                        && c.FileNumber == fileNumber
                                        && c.OrderNumber == orderNumber
                                    );
                                if (archives == null)
                                {
                                    var entity = new ArchivesInfo
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
                                        ImportFileName = item.OriginalFileName,
                                    };
                                    await _db.ArchivesInfo.AddAsync(entity);
                                    response.Data.AddTotoal++;
                                }
                                else {
                                    //档案存在
                                    //如果项目题名相同则跳过，否则中止
                                    if (archives.Title != row.GetCell(4).StringCellValue || archives.ProjectName != row.GetCell(5).StringCellValue) {
                                        response.Data.ErrorList.Add($"{item.OriginalFileName}中档号：{row.GetCell(0).StringCellValue}，分类号：{row.GetCell(1).StringCellValue}，案卷号：{row.GetCell(2).StringCellValue}卷内序号：{row.GetCell(3).StringCellValue} 与以前数据重复，并且题名或项目名称不一致;<br/>");
                                        if (response.Data.ErrorList.Count > 50)
                                        {
                                            response.Message = "发现大量重复数据";
                                            break;
                                        }
                                    }
                                    jumpcount++;
                                }
                            }                                                        
                        }
                    }
                    
                    if (response.Data.ErrorList.Any())
                    {
                        trans.Rollback();
                        //response.Message = string.Join("<br />", response.Data.ErrorList);
                        response.Message = $"导入发生数据异常";
                    }
                    else
                    {
                        //_db.ChangeTracker.AutoDetectChangesEnabled = false;
                        //await _db.ArchivesInfo.AddRangeAsync(archivesList);
                        //response.Data.AddTotoal = await _db.SaveChangesAsync();
                        await _db.SaveChangesAsync();
                        trans.Commit();
                        response.Success = true;
                        response.Message = $"跳过{jumpcount}条，添加{response.Data.AddTotoal}条数据";
                    }
                }
                catch (DbUpdateException ex)
                {
                    response.Message = $"导入发生数据异常";
                    trans.Rollback();
                    ApplicationLog.Error("ConfirmUpload", ex);
                    //if (ex.InnerException!= null && ex.InnerException is MySqlException)
                    //{
                    //    var exp = (MySqlException)ex.InnerException;
                    //    if (exp.Number == 1062)
                    //    {
                    //        response.Message = "导入发现重复数据";
                    //        for (int i = 0; i < archivesList.Count; i++)
                    //        {
                    //            var item = archivesList[i];
                    //            var cc = await _db.ArchivesInfo.CountAsync(c => c.ArchivesNumber == item.ArchivesNumber
                    //                && c.CatalogNumber == item.CatalogNumber
                    //                && c.FileNumber == item.FileNumber
                    //                && c.OrderNumber == item.OrderNumber);
                    //            if (cc > 0)
                    //            {
                    //                response.Data.ErrorList.Add($"档号：{item.ArchivesNumber}，分类号：{item.CatalogNumber}，案卷号：{item.FileNumber}卷内序号：{item.OrderNumber} 已存在;");
                    //                if (response.Data.ErrorList.Count > archivesList.Count / 3 || response.Data.ErrorList.Count > 50)
                    //                {
                    //                    response.Message = "发现大量重复数据";
                    //                    break;
                    //                }
                    //            }
                    //        }
                    //        if (response.Data.ErrorList.Count() == 0 && ex.InnerException!=null && ex.InnerException.Message !=null) {
                    //            response.Data.ErrorList.Add("可能您上传的文件内有重数数据，请参考异常信息："+ex.InnerException.Message);
                    //        }
                    //    }
                    //}
                }
                catch (Exception ex)
                {
                    response.Message = $"导入发生系统异常，请重试";
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

