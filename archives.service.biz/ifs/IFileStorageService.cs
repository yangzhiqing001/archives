using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using archives.service.biz.web;
using archives.service.dal.Entity;

namespace archives.service.biz.ifs
{
    public interface IFileStorageService
    {
        Task<string> AddFile(archives.service.dal.Entity.FileStorage fileStorage);

        Task<List<string>> AddRangeFile(List<archives.service.dal.Entity.FileStorage> fileStorageList);

        Task<FileStorage> Get(string id);

        Task<List<FileStorage>> GetList(List<string> ids);

        Task<CommonResponse<ConfirmUploadResult>> ConfirmUpload(List<string> ids);
    }

}
