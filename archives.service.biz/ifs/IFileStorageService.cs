using System;
using System.Threading.Tasks;
using archives.service.dal.Entity;

namespace archives.service.biz.ifs
{
    public interface IFileStorageService
    {
        Task<string> AddFile(archives.service.dal.Entity.FileStorage fileStorage);

        Task<FileStorage> Get(string id);
    }

}
