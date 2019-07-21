using System;
using System.Threading.Tasks;
using archives.service.biz.ifs;
using archives.service.dal;
using archives.service.dal.Entity;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using archives.service.biz.exp;

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

        public async Task<FileStorage> Get(string id)
        {
            var entity = await _db.FileStorage.FirstOrDefaultAsync(c => c.Id == id);
            if (entity == null)
                throw new BizException("文件不存在");
            return entity;
        }
    }
}
