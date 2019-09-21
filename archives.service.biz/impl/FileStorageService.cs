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

        public async Task<List<string>> ConfirmUpload(List<string> ids)
        {
            var list = await this.GetList(ids);
            if (!list.Any())
                throw new BizException("没有获取到上传的文件");

            var trans = await _db.Database.BeginTransactionAsync();
            var index = 1;
            var errorList = new List<string>();
            try
            {
                foreach (var item in list)
                {
                    using (var fs = new FileStream(item.StoragePath, FileMode.Open, FileAccess.Read))
                    {
                        IWorkbook workbook = WorkbookFactory.Create(fs);
                        ISheet sheet = workbook.GetSheetAt(0);
                        if (sheet == null)
                        {
                            errorList.Add($"第{index}个上传文件不不存在sheet，请确认");
                            continue;
                        }

                        for (int j = 1; j <= sheet.LastRowNum; ++j)
                        {
                            IRow row = sheet.GetRow(j);
                            if (row == null) continue; //没有数据的行默认是null　　　　　　　

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
                                    ArchivesNumber = archivesNumber,
                                    CategoryId = categoryId,
                                    FileNumber = fileNumber,
                                    OrderNumber = orderNumber,
                                    Title = row.GetCell(4).StringCellValue,
                                    ProjectName = row.GetCell(5).StringCellValue,
                                    ResponsibleObject = row.GetCell(6).StringCellValue,
                                    WrittenDate = row.GetCell(7).DateCellValue,
                                    Pages = (int)row.GetCell(8).NumericCellValue,
                                    IsPermanent = row.GetCell(9).StringCellValue,
                                    SecretLevel = row.GetCell(10).StringCellValue,
                                    ArchivingDepartment = row.GetCell(11).StringCellValue,
                                    ArchivingDate = row.GetCell(12).DateCellValue,
                                    Remark = row.GetCell(13).StringCellValue,
                                    CatalogNumber = row.GetCell(14).StringCellValue,
                                    Summary = row.GetCell(15).StringCellValue,
                                    CreateTime = DateTime.Now,
                                    Status = ArchivesStatus.Init,
                                    UpdateTime = DateTime.Now,
                                    Deleted = false,
                                };
                                await _db.ArchivesInfo.AddAsync(entity);
                            }
                            else
                            {
                                //档案已存在
                                errorList.Add($"第{index}个上传档案号：{archivesNumber}在数据库中已存在，请确认");
                            }
                        }
                    }

                    index++;
                }

                if (errorList.Any())
                {
                    trans.Rollback();
                }
                else
                    trans.Commit();
            }
            catch
            {
                trans.Rollback();
            }
            return errorList;
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

