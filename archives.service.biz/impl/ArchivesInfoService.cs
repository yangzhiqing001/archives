using System;
using System.Collections.Generic;
using System.Linq;
using archives.service.biz.ifs;
using archives.service.dal;
using archives.service.dal.Entity;

namespace archives.service.biz.impl
{
    public class ArchivesInfoService : IArchivesInfoService
    {
        public ArchivesContext _db;
        public ArchivesInfoService(ArchivesContext db)
        {
            _db = db;
        }
        public List<ArchivesInfo> GetList()
        {
            return _db.ArchivesInfo.ToList();
        }
    }
}
